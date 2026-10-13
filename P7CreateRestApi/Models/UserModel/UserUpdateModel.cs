using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models.UserModel
{
    public class UserUpdateModel
    {
        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "UsernameRequired")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "FullnameRequired")]
        public string Fullname { get; set; }
    }
}