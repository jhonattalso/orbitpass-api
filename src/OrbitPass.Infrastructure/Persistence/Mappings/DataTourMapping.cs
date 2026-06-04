using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbitPass.Domain.Entities;

namespace OrbitPass.Infrastructure.Persistence.Mappings;

public class DataTourMapping : IEntityTypeConfiguration<DataTour> {
    public void Configure(EntityTypeBuilder<DataTour> builder) {
        builder.ToTable("DATAS_TOUR");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("ID");

        builder.Property(d => d.Destino).HasColumnName("DESTINO")
               .HasMaxLength(100).IsRequired();

        builder.Property(d => d.DataPartida).HasColumnName("DATA_PARTIDA")
               .IsRequired();

        builder.Property(d => d.PrecoBase).HasColumnName("PRECO_BASE")
               .HasColumnType("NUMBER(10,2)").IsRequired();

        builder.HasData(
            new {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Destino = "Órbita Baixa Terrestre",
                DataPartida = new DateTime(2026, 10, 15, 8, 0, 0),
                PrecoBase = 50000.00m
            },
            new {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Destino = "Estação Lunar Artemis",
                DataPartida = new DateTime(2026, 12, 1, 10, 0, 0),
                PrecoBase = 250000.00m
            },
            new {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Destino = "Colônia de Marte",
                DataPartida = new DateTime(2027, 5, 20, 14, 30, 0),
                PrecoBase = 1500000.00m
            }
        );
    }
}