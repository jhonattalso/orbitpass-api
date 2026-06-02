using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbitPass.Domain.Entities;

namespace OrbitPass.Infrastructure.Persistence.Mappings;

public class IngressoMapping : IEntityTypeConfiguration<Ingresso> {
    public void Configure(EntityTypeBuilder<Ingresso> builder) {
        builder.ToTable("INGRESSOS");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id).HasColumnName("ID");
        builder.Property(i => i.UsuarioId).HasColumnName("USUARIO_ID").IsRequired();
        builder.Property(i => i.DataTourId).HasColumnName("DATA_TOUR_ID").IsRequired();
        builder.Property(i => i.CodigoUnico).HasColumnName("CODIGO_UNICO")
               .HasMaxLength(40).IsRequired();
        builder.Property(i => i.Status).HasColumnName("STATUS")
               .HasConversion<int>().IsRequired();
        builder.Property(i => i.DataCompra).HasColumnName("DATA_COMPRA").IsRequired();
        builder.Property(i => i.ValorPago).HasColumnName("VALOR_PAGO")
               .HasColumnType("NUMBER(10,2)").IsRequired();

        builder.HasOne(i => i.Pagamento)
               .WithOne(p => p.Ingresso)
               .HasForeignKey<Pagamento>(p => p.IngressoId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => i.CodigoUnico).IsUnique();
    }
}