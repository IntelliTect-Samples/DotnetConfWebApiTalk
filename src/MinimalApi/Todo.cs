﻿using System.ComponentModel.DataAnnotations;

namespace MinimalApi;

public class Todo
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = default!;
    public bool IsComplete { get; set; }
}
