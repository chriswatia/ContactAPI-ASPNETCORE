namespace ContactsAPI.Dtos.Common
{
    public class OperationResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public T? Data { get; set; }
    }
}
