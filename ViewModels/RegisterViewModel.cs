using System.ComponentModel.DataAnnotations;

namespace JKNews.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Informe o e-mail!")]
    [EmailAddress(ErrorMessage = "E-mail inválido!")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "Informe a senha!")]
    [MinLength(8, ErrorMessage = "Mínimo de 8 caracteres!")]
    [MaxLength(24, ErrorMessage = "Máximo de 24 caracteres!")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Confirmação de Senha")]
    [Required(ErrorMessage = "Informe a confirmação de senha!")]
    [MinLength(8, ErrorMessage = "Mínimo de 8 caracteres!")]
    [MaxLength(24, ErrorMessage = "Máximo de 24 caracteres!")]
    [Compare(nameof(Password), ErrorMessage = "As senhas não coincidem!")]
    public string PasswordConfirmation { get; set; } = string.Empty;
}
