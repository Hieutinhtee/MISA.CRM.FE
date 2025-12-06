using Microsoft.AspNetCore.Mvc;
using MISA.CRM.CORE.Exceptions;
using MISA.CRM.CORE.Interfaces.Services;

namespace MISA.CRM.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        protected readonly IBaseService<T> _service;

        public BaseController(IBaseService<T> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(new { data });
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var data = await _service.GetByIdAsync(id);
                return Ok(new { data });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] T entity)
        {
            try
            {
                var id = await _service.CreateAsync(entity);
                return StatusCode(201, new { id, message = "Created successfully" });
            }
            catch (ValidateException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(Guid id, [FromBody] T entity)
        {
            try
            {
                var rows = await _service.UpdateAsync(id, entity);
                return Ok(new { updated = rows, message = "Updated successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ValidateException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var rows = await _service.DeleteAsync(id);
                return Ok(new { deleted = rows, message = "Deleted successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}