using Microsoft.EntityFrameworkCore;
using OrbitPass.Domain.Entities;
using OrbitPass.Domain.Interfaces;
using OrbitPass.Infrastructure.Persistence.Context;

namespace OrbitPass.Infrastructure.Persistence.Repositories;

public class PagamentoRepository : IPagamentoRepository {
    private readonly AppDbContext _context;

    public PagamentoRepository(AppDbContext context) => _context = context;

    public async Task<Pagamento?> ObterPorIngressoIdAsync(
        Guid ingressoId, CancellationToken ct = default)
        => await _context.Pagamentos
            .FirstOrDefaultAsync(p => p.IngressoId == ingressoId, ct);

    public async Task AdicionarAsync(Pagamento pagamento, CancellationToken ct = default) {
        await _context.Pagamentos.AddAsync(pagamento, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AtualizarAsync(Pagamento pagamento, CancellationToken ct = default) {
        _context.Pagamentos.Update(pagamento);
        await _context.SaveChangesAsync(ct);
    }
}