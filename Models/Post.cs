using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace JKNews.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public Category? Category { get; set; }
    public User? User { get; set; }
    public IList<Tag>? Tags { get; set; }

    [NotMapped]
    public List<int>? TagsIds { get; set; }
}