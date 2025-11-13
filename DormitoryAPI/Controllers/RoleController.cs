using Dormitory.BUS.Interfaces;
using Dormitory.DTO.Role;
using Dormitory.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleBUS roleBUS;

        public RoleController(IRoleBUS roleBUS)
        {
            this.roleBUS = roleBUS;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            IEnumerable<Role> rs = await this.roleBUS.GetAllRolesAsync();
            return Ok(rs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleByID(string id)
        {
            try
            {
                Role? b = await this.roleBUS.GetRoleByIDAsync(id);
                if (b == null)
                    return NotFound(new { message = $"Role with id {id} not found." });
                return Ok(b);
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
        public async Task<IActionResult> AddNewRole([FromBody] RoleCreateDTO dto)
        {
            try
            {
                Role r = new Role
                {
                    Rolename = dto.RoleName,
                    Roledescription = dto.RoleDescription,
                };

                await this.roleBUS.AddRoleAsync(r);
                return CreatedAtAction(nameof(GetRoleByID), new { id = r.Roleid }, r);
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
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleUpdateDTO dto)
        {
            try
            {
                Role r = new Role
                {
                    Roleid = id,
                    Rolename = dto.RoleName,
                    Roledescription = dto.RoleDescription,
                };

                await this.roleBUS.UpdateRoleAsync(r);
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
        public async Task<IActionResult> RemoveRole(string id)
        {
            try
            {
                await this.roleBUS.RemoveRoleAsync(id);
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
