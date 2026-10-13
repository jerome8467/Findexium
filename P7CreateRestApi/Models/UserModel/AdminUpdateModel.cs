using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;


namespace P7CreateRestApi.Models.UserModel
{
    public class AdminUpdateModel
    {
        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "UsernameRequired")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "FullnameRequired")]
        public string Fullname { get; set; }

        [EnumDataType(typeof(UserRole),
            ErrorMessageResourceType = typeof(UserModelRessources),
            ErrorMessageResourceName = "RoleRequired")]
        public UserRole Role { get; set; }
    }
}
