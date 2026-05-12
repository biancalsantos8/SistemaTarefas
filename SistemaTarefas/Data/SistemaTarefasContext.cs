using Microsoft.EntityFrameworkCore;
using SistemaTarefas.Models;
namespace SistemaTarefas.Data
{
    public class SistemaTarefasContext : DbContext
    {
        public SistemaTarefasContext(DbContextOptions<SistemaTarefasContext> options)
            : base(options) { }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Tarefa> Tarefa { get; set; }
    }
}
