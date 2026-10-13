using P7CreateRestApi.Attribute;
using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.UserModel
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "OldPasswordRequired")]
        public string OldPassword { get; set; }

        [PasswordValid]
        public string NewPassword { get; set; }
    }
}
