using System.ComponentModel.DataAnnotations;

namespace JKNews.ViewModels;

public class UploadImageViewModel
{
    [Required(ErrorMessage = "Imagem inválida!")]
    public string Base64Image { get; set; }
}
