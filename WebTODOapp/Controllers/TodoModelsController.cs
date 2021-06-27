using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTODOapp.Models;

namespace WebTODOapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoModelsController : ControllerBase
    {
        private readonly ToDoContext _context;

        public ToDoModelsController(ToDoContext context)
        {
            _context = context;
        }

        // GET: api/ToDoModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoModel>>> GetToDoTable()
        {
            return await _context.ToDoTable.ToListAsync();
        }


        // GET: api/ToDoModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoModel>> GetToDoModel(int id)
        {
            var toDoModel = await _context.ToDoTable.FindAsync(id);

            if (toDoModel == null)
            {
                return NotFound();
            }

            return toDoModel;
        }

        // PUT: api/ToDoModels/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoModel(int id, ToDoModel toDoModel)
        {
            if (id != toDoModel.id)
            {
                return BadRequest();
            }

            _context.Entry(toDoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // GET: api/TodoModels/count?Tasks=a
        [HttpGet("count")]
        [Produces("application/json")]
        public string OnGet(string Tasks)
        {
            IQueryable<ToDoModel> Todo = from s in _context.ToDoTable
                                         select s;
            var counter = Todo.Count();

            switch (Tasks)
            {
                case "completed":
                    Todo = Todo.Where(s => s.completed == true);
                    counter = Todo.Count();
                    break;
                case "uncompleted":
                    Todo = Todo.Where(s => s.completed == false);
                    counter = Todo.Count();
                    break;
                default:
                    Todo = Todo.OrderBy(s => s.id);
                    break;
            } 
            string jsonString = JsonSerializer.Serialize(counter);

            return jsonString;
        }

        // GET: api/TodoModels/post?sortOrder=a&searchString=b
        [HttpGet("post")]
        public async Task<List<ToDoModel>> OnGetAsync(string sortOrder, string searchString)
        {
            

            IQueryable<ToDoModel> Todo = from s in _context.ToDoTable
                                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                Todo = Todo.Where(s => s.title.Contains(searchString)
                                       || s.color.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "title":
                    Todo = Todo.OrderByDescending(s => s.title);
                    break;
                case "date":
                    Todo = Todo.OrderBy(s => s.Date_of_creation);
                    break;
                case "color":
                    Todo = Todo.OrderBy(s => s.color);
                    break;
                case "completed":
                    Todo = Todo.OrderByDescending(s => s.completed);
                    break;
                default:
                    Todo = Todo.OrderBy(s => s.id);
                    break;
            }
            return await Todo.AsNoTracking().ToListAsync();

        }


        // GET: /api/TodoModels/export
        [Route("export")]
        [HttpGet]
        public FileResult ExportToCSV()
        {
            var sb = new StringBuilder();
            var list = _context.ToDoTable.ToList();
            sb.AppendFormat("{0},{1},{2},{3},{4}", "title", "completed", "color", "date_of_creation", Environment.NewLine);
            foreach (var item in list)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4}", item.title, item.completed, item.color, item.Date_of_creation,  Environment.NewLine);
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", DateTime.Now+".csv");
        }

        // POST: api/ToDoModels
        // To protect from overposting attacks, please enable the specific properties you want to bind
        [HttpPost]
        public async Task<ActionResult<ToDoModel>> PostToDoModel(ToDoModel toDoModel)
        {
            _context.ToDoTable.Add(toDoModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToDoModel", new { id = toDoModel.id }, toDoModel);
        }

        // DELETE: api/ToDoModels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ToDoModel>> DeleteToDoModel(int id)
        {
            var toDoModel = await _context.ToDoTable.FindAsync(id);
            if (toDoModel == null)
            {
                return NotFound();
            }

            _context.ToDoTable.Remove(toDoModel);
            await _context.SaveChangesAsync();

            return toDoModel;
        }

        private bool ToDoModelExists(int id)
        {
            return _context.ToDoTable.Any(e => e.id == id);
        }
    }
}