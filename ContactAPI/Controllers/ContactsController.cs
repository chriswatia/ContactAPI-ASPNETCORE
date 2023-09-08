using ContactAPI.Models;
using ContactsAPI.Data;
using ContactsAPI.Dtos;
using ContactsAPI.Dtos.Common;
using ContactsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ContactsController : ControllerBase
    {

        private IContactService _contactService;
        public ContactsController(IContactService contactService)
        {
            _contactService= contactService;
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<ContactDto>))]
        [ProducesResponseType(404)]
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var result = await _contactService.GetContactsAsync();
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<ContactDto>))]
        [ProducesResponseType(404)]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var result = await _contactService.GetContactAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<ContactDto>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<ContactDto>))]
        [HttpPost]
        public async Task<IActionResult> AddContact(ContactDto contactDto)
        {
            var result = await _contactService.AddContactAsync(contactDto);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<ContactDto>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<ContactDto>))]
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, ContactDto contactDto)
        {
            var result = await _contactService.UpdateContactAsync(id, contactDto);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [ProducesResponseType(200, Type = typeof(OperationResponse<ContactDto>))]
        [ProducesResponseType(400, Type = typeof(OperationResponse<ContactDto>))]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var result = await _contactService.DeleteContactAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
