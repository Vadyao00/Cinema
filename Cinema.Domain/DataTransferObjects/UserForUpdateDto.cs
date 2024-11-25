using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public class UserForUpdateDto
    {
        public string? OldUserName { get; set; }

        [Display(Name = "Имя")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string? Email { get; set; }

        [Display(Name = "Имя")]
        public string? FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string? SecondName { get; set; }

        [Display(Name = "Роль")]
        public string? UserRole { get; set; }
        public UserForUpdateDto()
        {
            UserRole = "User";
        }
    }
}