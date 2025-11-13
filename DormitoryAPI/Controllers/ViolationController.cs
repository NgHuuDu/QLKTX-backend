using Dormitory.BUS.Interfaces;
using Dormitory.DTO.Violations;
using Dormitory.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViolationController : ControllerBase
    {
        private readonly IViolationBUS violationBUS;

        public ViolationController(IViolationBUS violationBUS)
        {
            this.violationBUS = violationBUS;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllViolations()
        {
            IEnumerable<Violation> vs = await this.violationBUS.GetAllViolationsAsync();
            return Ok(vs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetViolationByID(string id)
        {
            try
            {
                Violation? v = await this.violationBUS.GetViolationByIdAsync(id);
                if (v == null)
                    return NotFound(new { message = $"No Violation with id {id} found." });
                return Ok(v);
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

        [HttpPost]
        public async Task<IActionResult> AddNewViolation([FromBody] ViolationCreateDTO dto)
        {
            try
            {
                Violation v = new Violation
                {
                    Userid = dto.UserID,
                    Violationdate = dto.ViolationDate,
                    Violationtype = dto.ViolationType,
                    Penaltyfee = dto.PenaltyFee
                };

                await this.violationBUS.AddNewViolationAsync(v);
                return CreatedAtAction(nameof(GetViolationByID), new { id = v.Violationid }, v);
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
        public async Task<IActionResult> UpdateViolation(string id, [FromBody] ViolationUpdateDTO dto)
        {
            try
            {
                Violation v = new Violation
                {
                    Violationid = id,
                    Userid = dto.UserID,
                    Violationdate = dto.ViolationDate,
                    Violationtype = dto.ViolationType,
                    Penaltyfee = dto.PenaltyFee
                };

                await this.violationBUS.UpdateViolationAsync(v);
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
        public async Task<IActionResult> RemoveViolation(string id)
        {
            try
            {
                await this.violationBUS.RemoveViolationAsync(id);
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
