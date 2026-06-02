using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrbitPass.Domain.Entities;

namespace OrbitPass.Infrastructure.Persistence.Mappings;

public class PagamentoMapping : IEntityTypeConfiguration<Pagamento> {
    public void Configure(EntityTypeBuilder<Pagamento> builder) {
        builder.ToTable("PAGAMENTOS");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("ID");
        builder.Property(p => p.IngressoId).HasColumnName("INGRESSO_ID").IsRequired();
        builder.Property(p => p.Metodo).HasColumnName("METODO")
               .HasConversion<int>().IsRequired();
        builder.Property(p => p.Status).HasColumnName("STATUS")
               .HasConversion<int>().IsRequired();
        builder.Property(p => p.DataPagamento).HasColumnName("DATA_PAGAMENTO").IsRequired();
        builder.Property(p => p.Valor).HasColumnName("VALOR")
               .HasColumnType("NUMBER(10,2)").IsRequired();
    }
}