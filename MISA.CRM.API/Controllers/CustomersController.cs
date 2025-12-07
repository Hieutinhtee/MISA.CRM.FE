using Microsoft.AspNetCore.Mvc;
using MISA.CRM.CORE.Entities;
using MISA.CRM.CORE.Exceptions;
using MISA.CRM.CORE.Interfaces.Repositories;
using MISA.CRM.CORE.Interfaces.Services;
using MISA.CRM.CORE.Services;

namespace MISA.CRM.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : BaseController<Customer>
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
            : base(customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("next-code")]
        public async Task<IActionResult> GetNextCode()
        {
            var code = await _customerService.NextCodeAsync();
            return Ok(new { CustomerCode = code });
        }
    }
}