using P7CreateRestApi.Attribute;
using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.UserModel
{
    public class SignUpModel
    {
        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "UsernameRequired")]
        public string Username { get; set; }

        [PasswordValid]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "FullnameRequired")]
        public string Fullname { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "RoleRequired")]
        public string Role { get; set; }
    }
}
