using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public record UserDto
    {
        public string? Id { get; set; }

        [Display(Name = "Логин")]
        public string? UserName { get; set; }

        [Display(Name = "ФИО")]
        public string? Name { get; set; }

        [Display(Name = "Дата регистрации")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string? Email { get; set; }

        [Display(Name = "Роль")]
        public string? UserRole { get; set; }
    }
}