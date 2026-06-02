using OrbitPass.Domain.Enums;

namespace OrbitPass.Domain.Entities;

public class Pagamento {
    public Guid Id { get; private set; }
    public Guid IngressoId { get; private set; }
    public MetodoPagamento Metodo { get; private set; }
    public StatusPagamento Status { get; private set; }
    public DateTime DataPagamento { get; private set; }
    public decimal Valor { get; private set; }

    public Ingresso? Ingresso { get; private set; }

    protected Pagamento() { }

    public static Pagamento Criar(Guid ingressoId, MetodoPagamento metodo, decimal valor) {
        return new Pagamento {
            Id = Guid.NewGuid(),
            IngressoId = ingressoId,
            Metodo = metodo,
            Status = StatusPagamento.Processando,
            DataPagamento = DateTime.UtcNow,
            Valor = valor
        };
    }

    public void Aprovar() => Status = StatusPagamento.Aprovado;
    public void Recusar() => Status = StatusPagamento.Recusado;
}