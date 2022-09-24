namespace JKNews.Models;

public class Tag
{
    public Tag()
    {
        Posts = new List<Post>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Desc { get; set; }
    public IList<Post>? Posts { get; set; }
}