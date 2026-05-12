using SistemaTarefas.Data;
using SistemaTarefas.Models;
using Microsoft.AspNetCore.Mvc;

namespace SistemaTarefa.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {

        private readonly SistemaTarefasContext _context;

        public TarefaController(SistemaTarefasContext context)
        {
            _context = context;
        }



        [HttpGet("{id}")]
        public IActionResult RetornaTarefa(int id)
        {
            var clientes = HttpContext.Session.GetString("IdCliente");

            if (clientes == null)
                return Unauthorized("Não autenticado");
            var tarefas = _context.Tarefa.Find(id);
            if (tarefas == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            return Ok(tarefas);
        }

        [HttpGet("tarefasCliente/{id}")]
        public IActionResult TarefasCliente(int Id)
        {
            var clientes = HttpContext.Session.GetString("IdCliente");

            if (clientes == null)
                return Unauthorized("Não autenticado");
            var resultado = from c in _context.Cliente
                            join t in _context.Tarefa
                            on c.Id equals t.IdCliente
                            where Id == c.Id
                            select new
                            {
                                Cliente = c.Nome,
                                c.Email,
                                c.Senha,
                                Tarefas = t.Descricao,
                                t.Status,
                                t.IdCliente,

                            };
            return Ok(resultado.ToList());
        }

        [HttpPost]
        public IActionResult CadastraTarefa(Tarefa tarefa)
        {
            var clientes = HttpContext.Session.GetString("IdCliente");

            if (clientes == null)
                return Unauthorized("Não autenticado");

            var id = Request.Cookies["IdCliente"];
            if (id != null)
                tarefa.IdCliente = int.Parse(id);

            _context.Tarefa.Add(tarefa);
            _context.SaveChanges();

            return Created("", tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaTarefa(int id, Tarefa tarefa)

        {

            var clientes = HttpContext.Session.GetString("IdCliente");
            if (clientes == null)
            {
                return Unauthorized("Não autenticado no sistem!");

            }

            var tarefaDoBanco = _context.Tarefa.Find(id);
            if (tarefaDoBanco == null)
            {
                return NotFound("Tarefa não existe no banco!");
            }
            tarefaDoBanco.Descricao = tarefa.Descricao;
            tarefaDoBanco.Status = tarefa.Status;

            _context.SaveChanges();
            return Ok("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaTarefa(int id)
        {

            var clientes = HttpContext.Session.GetString("IdCliente");
            if (clientes == null)
            {
                return Unauthorized("Não autenticado no sistem!");

            }

            var tarefaDoBanco = _context.Tarefa.Find(id);
            if (tarefaDoBanco == null)
            {
                return NotFound("Não encontrado!");
            }
            _context.Remove(tarefaDoBanco);
            _context.SaveChanges();
            return Ok("Deletado");
        }
    }
}