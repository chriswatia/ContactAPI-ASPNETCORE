using ContactsAPI.Dtos;
using ContactsAPI.Dtos.Common;

namespace ContactsAPI.Services
{
    public interface IContactService
    {
        Task<OperationResponse<List<ContactDto>>> GetContactsAsync();
        Task<OperationResponse<ContactDto>> GetContactAsync(Guid id);
        Task<OperationResponse<ContactDto>> AddContactAsync(ContactDto contactDto);
        Task<OperationResponse<ContactDto>> UpdateContactAsync(Guid id, ContactDto contactDto);
        Task<OperationResponse<ContactDto>> DeleteContactAsync(Guid id);
    }
}
