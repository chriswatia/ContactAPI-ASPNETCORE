using ContactAPI.Models;
using ContactsAPI.Data;
using ContactsAPI.Data.Mappers;
using ContactsAPI.Dtos;
using ContactsAPI.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly ContactsAPIDbContext _dbContext;
        public ContactService(ContactsAPIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResponse<ContactDto>> AddContactAsync(ContactDto contactDto)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                FullName = contactDto.FullName,
                Email = contactDto.Email,
                Phone = contactDto.Phone,
                Address = contactDto.Address
            };

            await _dbContext.Contacts.AddAsync(contact);
            await _dbContext.SaveChangesAsync();

            return new OperationResponse<ContactDto>
            {
                IsSuccess = true,
                Message = "Contact created successfully",
                Data = contactDto
            };
        }

        public async Task<OperationResponse<ContactDto>> DeleteContactAsync(Guid id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact is null)
            {
                return new OperationResponse<ContactDto>
                {
                    IsSuccess = false,
                    Message = "Client not found!"
                };
            }
            _dbContext.Contacts.Remove(contact);
            await _dbContext.SaveChangesAsync();

            return new OperationResponse<ContactDto>
            {
                IsSuccess = true,
                Message = "Contact has been deleted successfully!",
                Data = contact.ToContactDto()
            };
        }

        public async Task<OperationResponse<ContactDto>> GetContactAsync(Guid id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            if(contact is not null)
            {
                return new OperationResponse<ContactDto>
                {
                    IsSuccess = true,
                    Message = "Contact retrieved successfully!",
                    Data = contact.ToContactDto()
                };
            }

            return new OperationResponse<ContactDto>
            {
                IsSuccess = false,
                Message = "Contact not found!"
            };
        }

        public async Task<OperationResponse<List<ContactDto>>> GetContactsAsync()
        {
            var contacts = await _dbContext.Contacts
                .Select(c => c.ToContactDto())
                .ToListAsync();

            if (contacts is not null)
            {
                return new OperationResponse<List<ContactDto>>
                {
                    IsSuccess = true,
                    Message = "Contacts retrieved successfully!",
                    Data = contacts
                };
            }

            return new OperationResponse<List<ContactDto>>
            {
                IsSuccess = false,
                Message = "No contacts found!"
            };
        }

        public async Task<OperationResponse<ContactDto>> UpdateContactAsync(Guid id, ContactDto contactDto)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);
            if (contact is null)
            {
                return new OperationResponse<ContactDto>
                {
                    IsSuccess = false,
                    Message = "Contact not found!"
                };
            }                

            contact.FullName = contactDto.FullName;
            contact.Email = contactDto.Email;
            contact.Phone = contactDto.Phone;
            contact.Address = contactDto.Address;

            contactDto.Id = id;
            await _dbContext.SaveChangesAsync();
            return new OperationResponse<ContactDto>
            {
                IsSuccess = true,
                Message = "Contact updated successfully!",
                Data = contactDto
            };
        }
    }
}
