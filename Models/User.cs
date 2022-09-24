using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JKNews.Models;

public class User : IdentityUser<int>
{
    public User()
    {
        Posts = new List<Post>();
    }

    public override string? PhoneNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public IList<Post> Posts { get; set; }
}