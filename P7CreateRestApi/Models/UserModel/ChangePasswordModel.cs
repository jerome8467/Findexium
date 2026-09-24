using P7CreateRestApi.Attribute;

namespace P7CreateRestApi.Models.UserModel
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        [PasswordValid]
        public string NewPassword { get; set; }
    }
}
