using Microsoft.AspNetCore.Identity;

namespace JKNews.Models;

public class Role : IdentityRole<int>
{
    public string? Description { get; set; }
}