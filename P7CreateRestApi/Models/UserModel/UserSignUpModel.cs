using P7CreateRestApi.Attribute;
using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.UserModel
{
    public class UserSignUpModel
    {
        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "UsernameRequired")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "FullnameRequired")]
        public string Fullname { get; set; }

        [PasswordValid]
        public string Password { get; set; }


    }
}