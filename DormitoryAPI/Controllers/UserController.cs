using Dormitory.BUS.Interfaces;
using Dormitory.DTO.User;
using Dormitory.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserBUS userBUS;

        public UserController(IUserBUS userBUS)
        {
            this.userBUS = userBUS;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            IEnumerable<User> users = await this.userBUS.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByID(string id)
        {
            try
            {
                User? u = await this.userBUS.GetUserByIDAsync(id);
                if (u == null)
                    return NotFound(new { message = $"No user with id {id} found." });
                return Ok(u);
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
        public async Task<IActionResult> AddUser([FromBody] UserCreateDTO dto)
        {
            try
            {
                User u = new User
                {
                  Username = dto.UserName,
                  Studentid = dto.StudentID,
                  Roleid = dto.RoleID
                };

                await this.userBUS.AddUserAsync(u);
                return CreatedAtAction(nameof(GetUserByID), new { id = u.Studentid }, u);
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
                return StatusCode(500, new { message = "An unexpected error occurred", detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDTO dto)
        {
            try
            {
                User u = new User
                {
                    Userid = id,
                    Username = dto.UserName,
                    Studentid = dto.StudentID,
                    Roleid = dto.RoleID
                };

                await this.userBUS.UpdateUserAsync(u);
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
                return StatusCode(500, new { message = "An unexpected error occurred", detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(string id)
        {
            await this.userBUS.RemoveUserAsync(id);
            return NoContent();
        }
    }
}
