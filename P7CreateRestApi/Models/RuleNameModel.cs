using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models
{
    public class RuleNameModel
    {
        [Required(ErrorMessageResourceType = typeof(RuleNameModelRessources),
            ErrorMessageResourceName = "NameRequired")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(RuleNameModelRessources),
            ErrorMessageResourceName = "DescriptionRequired")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(RuleNameModelRessources),
            ErrorMessageResourceName = "JsonRequired")]
        public string Json { get; set; }

        [Required(ErrorMessageResourceType = typeof(RuleNameModelRessources),
            ErrorMessageResourceName = "TemplateRequired")]
        public string Template { get; set; }

        [Required(ErrorMessageResourceType = typeof(RuleNameModelRessources),
            ErrorMessageResourceName = "SqlStrRequired")]
        public string SqlStr { get; set; }

        [Required(ErrorMessageResourceType = typeof(RuleNameModelRessources),
            ErrorMessageResourceName = "SqlPartRequired")]
        public string SqlPart { get; set; }
    }
}
