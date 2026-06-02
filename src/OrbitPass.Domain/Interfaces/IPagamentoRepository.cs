using OrbitPass.Domain.Entities;

namespace OrbitPass.Domain.Interfaces;

public interface IPagamentoRepository {
    Task<Pagamento?> ObterPorIngressoIdAsync(Guid ingressoId, CancellationToken ct = default);
    Task AdicionarAsync(Pagamento pagamento, CancellationToken ct = default);
    Task AtualizarAsync(Pagamento pagamento, CancellationToken ct = default);
}