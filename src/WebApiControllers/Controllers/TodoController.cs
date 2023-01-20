using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApiControllers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    //private readonly ApplicationDbContext _dbContext;

    public TodoController(ApplicationDbContext dbContext, ILogger<TodoController> logger)
    {
        //_dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> Get(ApplicationDbContext _dbContext)
    {
        return await _dbContext.Todos.AsNoTracking().ToListAsync();
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetById(int id, ApplicationDbContext _dbContext)
    {
        var todo = await _dbContext.Todos.Where(t => t.Id == id).SingleOrDefaultAsync();

        return todo == null ? NotFound() : Ok(todo);
    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<ActionResult<Todo>> Post(ApplicationDbContext _dbContext, Todo todo)
    {
        if (todo == null)
        {
            return BadRequest();
        }

        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        return Created($"/api/Todo/{todo.Id}", todo);
    }
}
