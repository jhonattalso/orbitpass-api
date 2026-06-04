using Microsoft.EntityFrameworkCore;
using OrbitPass.Domain.Entities;
using OrbitPass.Infrastructure.Persistence.Mappings;

namespace OrbitPass.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Ingresso> Ingressos { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<DataTour> DatasTour { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfiguration(new DataTourMapping());
        modelBuilder.ApplyConfiguration(new IngressoMapping());
        modelBuilder.ApplyConfiguration(new PagamentoMapping());
        base.OnModelCreating(modelBuilder);
    }
}