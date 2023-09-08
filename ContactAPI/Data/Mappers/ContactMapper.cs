using ContactAPI.Models;
using ContactsAPI.Dtos;

namespace ContactsAPI.Data.Mappers
{
    public static class ContactMapper
    {
        public static ContactDto ToContactDto(this Contact contact)
        {
            return new ContactDto
            {
                Id = contact.Id,
                FullName = contact.FullName,
                Email = contact.Email,
                Phone = contact.Phone,
                Address = contact.Address
            };
        }

    }
}
