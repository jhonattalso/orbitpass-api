using OrbitPass.Domain.Entities;
using OrbitPass.Domain.Interfaces;

namespace OrbitPass.Application.UseCases.Ingressos.ComprarIngresso;

public class ComprarIngressoHandler {
    private readonly IIngressoRepository _ingressoRepository;
    private readonly IPagamentoRepository _pagamentoRepository;

    public ComprarIngressoHandler(
        IIngressoRepository ingressoRepository,
        IPagamentoRepository pagamentoRepository) {
        _ingressoRepository = ingressoRepository;
        _pagamentoRepository = pagamentoRepository;
    }

    public async Task<ComprarIngressoResponse> Handle(
        ComprarIngressoCommand command, CancellationToken ct = default) {
        var ingresso = Ingresso.Criar(
            command.UsuarioId,
            command.DataTourId,
            command.ValorPago);

        await _ingressoRepository.AdicionarAsync(ingresso, ct);

        var pagamento = Pagamento.Criar(ingresso.Id, command.Metodo, command.ValorPago);
        pagamento.Aprovar();
        ingresso.Confirmar();

        await _pagamentoRepository.AdicionarAsync(pagamento, ct);
        await _ingressoRepository.AtualizarAsync(ingresso, ct);

        return new ComprarIngressoResponse(
            ingresso.Id,
            ingresso.CodigoUnico,
            ingresso.Status,
            pagamento.Id,
            pagamento.Status);
    }
}