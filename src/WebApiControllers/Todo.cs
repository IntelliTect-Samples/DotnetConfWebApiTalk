using System.ComponentModel.DataAnnotations;

namespace WebApiControllers;

public class Todo
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = default!;
    public bool IsComplete { get; set; }
}
