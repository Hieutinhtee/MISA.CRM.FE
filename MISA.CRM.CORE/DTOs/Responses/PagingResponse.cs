using MISA.CRM.Core.DTOs.Responses;

namespace MISA.CRM.Core.DTOs.Responses
{
    public class PagingResponse<T>
    {
        public List<T>? Data { get; set; }
        public Meta Meta { get; set; } = new Meta();
        public string? Error { get; set; }
    }
}