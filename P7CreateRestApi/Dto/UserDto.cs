using P7CreateRestApi.Models.UserModel;

namespace P7CreateRestApi.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public UserRole Role { get; set; }
    }
}
