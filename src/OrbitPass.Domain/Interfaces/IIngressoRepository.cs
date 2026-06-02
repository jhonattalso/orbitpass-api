using OrbitPass.Domain.Entities;

namespace OrbitPass.Domain.Interfaces;

public interface IIngressoRepository {
    Task<Ingresso?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Ingresso>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken ct = default);
    Task AdicionarAsync(Ingresso ingresso, CancellationToken ct = default);
    Task AtualizarAsync(Ingresso ingresso, CancellationToken ct = default);
}