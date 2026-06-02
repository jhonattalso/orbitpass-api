using Microsoft.EntityFrameworkCore;
using OrbitPass.Domain.Entities;
using OrbitPass.Domain.Interfaces;
using OrbitPass.Infrastructure.Persistence.Context;

namespace OrbitPass.Infrastructure.Persistence.Repositories;

public class IngressoRepository : IIngressoRepository {
    private readonly AppDbContext _context;

    public IngressoRepository(AppDbContext context) => _context = context;

    public async Task<Ingresso?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Ingressos
            .Include(i => i.Pagamento)
            .FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<IEnumerable<Ingresso>> ListarPorUsuarioAsync(
        Guid usuarioId, CancellationToken ct = default)
        => await _context.Ingressos
            .Include(i => i.Pagamento)
            .Where(i => i.UsuarioId == usuarioId)
            .ToListAsync(ct);

    public async Task AdicionarAsync(Ingresso ingresso, CancellationToken ct = default) {
        await _context.Ingressos.AddAsync(ingresso, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AtualizarAsync(Ingresso ingresso, CancellationToken ct = default) {
        _context.Ingressos.Update(ingresso);
        await _context.SaveChangesAsync(ct);
    }
}