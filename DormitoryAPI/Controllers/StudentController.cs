using Dormitory.BUS.Interfaces;
using Dormitory.DTO.Students;
using Dormitory.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DormitoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentBUS studentBUS;

        public StudentController(IStudentBUS studentBUS)
        {
            this.studentBUS = studentBUS;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            IEnumerable<Student> students = await this.studentBUS.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentByID(string id)
        {
            try
            {
                Student? s = await this.studentBUS.GetStudentByIDAsync(id);
                if (s == null)
                    return NotFound(new { message = $"No student with id {id} found." });
                return Ok(s);
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
        public async Task<IActionResult> AddStudent([FromBody] StudentCreateDTO dto)
        {
            try
            {
                Student s = new Student
                {
                    Studentname = dto.StudentName,
                    Gender = dto.Gender,
                    Department = dto.Department,
                    Dateofbirth = dto.DateOfBirth,
                    Phonenumber = dto.PhoneNumber,
                    Email = dto.Email,
                    Address = dto.Address
                };

                await this.studentBUS.AddStudentAsync(s);
                return CreatedAtAction(nameof(GetStudentByID), new { id = s.Studentid }, s);
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
        public async Task<IActionResult> UpdateStudent(string id, [FromBody] StudentUpdateDTO dto)
        {
            try
            {
                Student s = new Student
                {
                    Studentid = id,
                    Studentname = dto.StudentName,
                    Gender = dto.Gender,
                    Department = dto.Department,
                    Dateofbirth = dto.DateOfBirth,
                    Phonenumber = dto.PhoneNumber,
                    Email = dto.Email,
                    Address = dto.Address
                };

                await this.studentBUS.UpdateStudentAsync(s);
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
        public async Task<IActionResult> RemoveStudent(string id)
        {
            await this.studentBUS.RemoveStudentAsync(id);
            return NoContent();
        }
    }
}
