using OrbitPass.Domain.Enums;
using OrbitPass.Domain.Exceptions;

namespace OrbitPass.Domain.Entities;

public class Ingresso {
    public Guid Id { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Guid DataTourId { get; private set; }
    public string CodigoUnico { get; private set; } = string.Empty;
    public StatusIngresso Status { get; private set; }
    public DateTime DataCompra { get; private set; }
    public decimal ValorPago { get; private set; }

    public Pagamento? Pagamento { get; private set; }

    protected Ingresso() { }

    public static Ingresso Criar(Guid usuarioId, Guid dataTourId, decimal valor) {
        if (valor <= 0)
            throw new DomainException("O valor do ingresso deve ser positivo.");

        return new Ingresso {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            DataTourId = dataTourId,
            CodigoUnico = GerarCodigo(),
            Status = StatusIngresso.PendentePagamento,
            DataCompra = DateTime.UtcNow,
            ValorPago = valor
        };
    }

    public void Confirmar() {
        if (Status != StatusIngresso.PendentePagamento)
            throw new DomainException("Apenas ingressos pendentes podem ser confirmados.");

        Status = StatusIngresso.Confirmado;
    }

    public void Cancelar() {
        if (Status == StatusIngresso.Cancelado)
            throw new DomainException("Ingresso já está cancelado.");

        Status = StatusIngresso.Cancelado;
    }

    private static string GerarCodigo()
        => $"OP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
}