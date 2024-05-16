using ESH.Master.TesteBatch.DataModel.models;
using Microsoft.EntityFrameworkCore;

namespace ESH.Master.TesteBatch.DataAccess.DB;

public class ContextoDataBase : DbContext
{
    public ContextoDataBase()
    {
        Database.EnsureCreated();
    }
   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(LocalDB)\\mssqllocaldb;database=ESH.Master.TesteBatchDB");
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Contato> Contatos { get; set; }
    public DbSet<TipoContato> TipoContatos { get; set; }
    public DbSet<TipoDeAtivo> TipoDeAtivos { get; set; }
    public DbSet<Ativo> Ativos { get; set; }
    public DbSet<Carteira> Carteiras { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>().Property(pessoa => pessoa.Nome).IsRequired();
        modelBuilder.Entity<Pessoa>().Property(pessoa => pessoa.Nome).HasColumnType("Varchar(max)");
        modelBuilder.Entity<Pessoa>().Property(pessoa => pessoa.DataNascimento).HasColumnType("Date");
        base.OnModelCreating(modelBuilder);
    }
}
