using P7CreateRestApi.Attribute;
using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.UserModel
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "UsernameRequired")]
        public string Username { get; set; }

        [PasswordValid]
        public string Password { get; set; }
    }
}
