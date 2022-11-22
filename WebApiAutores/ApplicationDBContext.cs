using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // tener esta parte es muy importante antes de hacer la migracion, siempre y cuando se haya sobreescrito el modelcreating
            //sino no pasa nada, pero sin eso no funciona

            modelBuilder.Entity<AutorLibro>().HasKey(al => new {al.LibroId, al.AutorId});   
        }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<AutorLibro> AutorLibro { get; set; }
    }
}
