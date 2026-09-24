using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service
{
    public class ServiceResult<T>
    {
        public List<ValidationResult> Errors { get; set; } = new List<ValidationResult>();
        public T? Data { get; set; }
    }
}
