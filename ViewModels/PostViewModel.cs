using JKNews.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JKNews.ViewModels;

public class PostViewModel
{
    [Display(Name = "Título")]
    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Descrição")]
    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    [AllowHtml]
    public string Desc { get; set; } = string.Empty;

    [Display(Name = "Categoria")]
    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    public int CategoryId { get; set; }

    [Display(Name = "Tags")]
    public List<int>? TagsIds { get; set; }

    [Display(Name = "Usuário")]
    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    public DateTime CreatedAt { get; set; }

    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    public DateTime UpdatedAt { get; set; }
}
