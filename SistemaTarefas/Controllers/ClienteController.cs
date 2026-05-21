using Microsoft.AspNetCore.Mvc;
using SistemaTarefas.Data;
using SistemaTarefas.Models;
namespace SistemaTarefas.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly SistemaTarefasContext _context;

        public ClienteController(SistemaTarefasContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public IActionResult RetornaCliente(int id)
        {
            var clientes = _context.Cliente.Find(id);
            if (clientes == null)
            {
                return NotFound("Não há clientes com esse Id!");
            }
            return Ok(clientes);

        }

        [HttpPost]
        public IActionResult CadastraCliente(Cliente cliente)
        {
            _context.Add(cliente);
            _context.SaveChanges();
            return Created("", cliente);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaCliente(Cliente cliente)
        {
            var clienteDoBanco = _context.Cliente.Find("IdCliente");
            if (clienteDoBanco == null)
            {
                return NotFound("Cliente não existe no banco!");
            }
            clienteDoBanco.Nome = cliente.Nome;
            clienteDoBanco.Email = cliente.Email;
            clienteDoBanco.Senha = cliente.Senha;
            _context.SaveChanges();
            return Ok("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaCliente(int id)
        {
            var clienteDoBanco = _context.Cliente.Find(id);
            if (clienteDoBanco == null)
            {
                return NotFound("Não encontrado!");
            }
            _context.Remove(clienteDoBanco);
            _context.SaveChanges();
            return Ok("Deletado");
        }


        [HttpPost("login")]
        public IActionResult Login(Cliente cliente)
        {
            var clientes = _context.Cliente
                 .Where(c => c.Email.Equals(cliente.Email) &&
              c.Senha.Equals(cliente.Senha)).ToList();
            if (!clientes.Any())
            {
                return Unauthorized("Usuário ou senha Inválidos!");
            }
            HttpContext.Session.SetString("IdCliente", Convert.ToString(clientes[0].Id));
            Response.Cookies.Append("IdCliente", clientes [0].Id.ToString(),
           new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            return Ok("login realizado com sucesso");

        }
    }
}
