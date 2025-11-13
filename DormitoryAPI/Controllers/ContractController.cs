using Dormitory.BUS.Interfaces;
using Dormitory.DTO.Contracts;
using Dormitory.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractController : ControllerBase
    {
        private readonly IContractBUS contractBUS;

        public ContractController(IContractBUS contractBUS)
        {
            this.contractBUS = contractBUS;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContracts()
        {
            IEnumerable<Contract> cs = await this.contractBUS.GetAllContractsAsync();
            return Ok(cs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContractByID(string id)
        {
            try
            {
                Contract? c = await this.contractBUS.GetContractByIDAsync(id);
                if (c == null)
                    return NotFound(new { message = $"Contract with ID {id} not found." });
                return Ok(c);
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
        public async Task<IActionResult> AddContract([FromBody] ContractCreateDTO dto)
        {
            try
            {
                Contract c = new Contract
                {
                    Userid = dto.UserID,
                    Roomid = dto.RoomID,
                    Starttime = dto.StartTime,
                    Endtime = dto.EndTime
                };

                await this.contractBUS.AddContractAsync(c);
                return CreatedAtAction(nameof(GetContractByID), new { id = c.Contractid }, c);
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
        public async Task<IActionResult> UpdateContract(string id, [FromBody] ContractUpdateDTO dto)
        {
            try
            {
                Contract c = new Contract
                {
                    Contractid = id,
                    Userid = dto.UserID,
                    Roomid = dto.RoomID,
                    Starttime = dto.StartTime,
                    Endtime = dto.EndTime
                };

                await this.contractBUS.UpdateContractAsync(c);
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
        public async Task<IActionResult> RemoveContract(string id)
        {
            try
            {
                await this.contractBUS.RemoveContractAsync(id);
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
