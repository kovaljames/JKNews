using System.ComponentModel.DataAnnotations;

namespace JKNews.ViewModels;

public class LoginViewModel
{
    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido!")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Senha")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    public string Password { get; set; } = string.Empty;
    public string? ReturnUrl { get; set; } = string.Empty;
}
