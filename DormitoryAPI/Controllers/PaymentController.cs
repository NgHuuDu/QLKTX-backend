using Dormitory.BUS.Interfaces;
using Dormitory.DTO.Paymenrs;
using Dormitory.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentBUS paymentBUS;

        public PaymentController(IPaymentBUS paymentBUS)
        {
            this.paymentBUS = paymentBUS;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            IEnumerable<Payment> ps = await this.paymentBUS.GetAllPaymentsAsync();
            return Ok(ps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentByID(string id)
        {
            try
            {
                Payment? p = await this.paymentBUS.GetPaymentByIDAsync(id);
                if (p == null)
                    return NotFound($"No payment with id {id} found.");
                return Ok(p);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred", detail = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPayment([FromBody] PaymentCreateDTO dto)
        {
            try
            {
                Payment p = new Payment
                {
                    Contractid = dto.ContractID,
                    Paymentdate = dto.PaymentDate,
                    Paymentmethod = dto.PaymentMethod,
                    Paymentstatus = dto.PaymentStatus
                };

                await this.paymentBUS.AddPaymentAsync(p);
                return CreatedAtAction(nameof(GetPaymentByID), new { id = p.Paymentid }, p);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(string id, [FromBody] PaymentUpdateDTO dto)
        {
            try
            {
                Payment p = new Payment
                {
                    Paymentid = id,
                    Contractid = dto.ContractID,
                    Paymentdate = dto.PaymentDate,
                    Paymentmethod = dto.PaymentMethod,
                    Paymentstatus = dto.PaymentStatus
                };

                await this.paymentBUS.UpdatePaymentAsync(p);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePayment(string id)
        {
            try
            {
                await this.paymentBUS.RemovePaymentAsync(id);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }
    }
}
