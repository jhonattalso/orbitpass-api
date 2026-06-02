using OrbitPass.Application.DTOs;
using OrbitPass.Domain.Entities;
using OrbitPass.Domain.Exceptions;
using OrbitPass.Domain.Interfaces;

namespace OrbitPass.Application.UseCases.Pagamentos.ProcessarPagamento;

public class ProcessarPagamentoHandler {
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IIngressoRepository _ingressoRepository;

    public ProcessarPagamentoHandler(
        IPagamentoRepository pagamentoRepository,
        IIngressoRepository ingressoRepository) {
        _pagamentoRepository = pagamentoRepository;
        _ingressoRepository = ingressoRepository;
    }

    public async Task<PagamentoDto> Handle(
        ProcessarPagamentoCommand command, CancellationToken ct = default) {
        var ingresso = await _ingressoRepository.ObterPorIdAsync(command.IngressoId, ct)
            ?? throw new KeyNotFoundException($"Ingresso {command.IngressoId} não encontrado.");

        var pagamentoExistente = await _pagamentoRepository
            .ObterPorIngressoIdAsync(command.IngressoId, ct);

        if (pagamentoExistente is not null)
            throw new DomainException("Este ingresso já possui um pagamento registrado.");

        var pagamento = Pagamento.Criar(ingresso.Id, command.Metodo, command.Valor);
        pagamento.Aprovar();
        ingresso.Confirmar();

        await _pagamentoRepository.AdicionarAsync(pagamento, ct);
        await _ingressoRepository.AtualizarAsync(ingresso, ct);

        return new PagamentoDto(
            pagamento.Id,
            pagamento.IngressoId,
            pagamento.Metodo,
            pagamento.Status,
            pagamento.DataPagamento,
            pagamento.Valor);
    }
}