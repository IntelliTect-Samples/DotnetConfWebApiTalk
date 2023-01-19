namespace MinimalApiNet6;

public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public bool IsComplete { get; set; }
}
