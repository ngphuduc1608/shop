using System.ComponentModel.DataAnnotations;

namespace proj_tt.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}