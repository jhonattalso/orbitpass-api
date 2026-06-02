using OrbitPass.Domain.Exceptions;
using OrbitPass.Domain.Interfaces;

namespace OrbitPass.Application.UseCases.Ingressos.CancelarIngresso;

public class CancelarIngressoHandler {
    private readonly IIngressoRepository _ingressoRepository;

    public CancelarIngressoHandler(IIngressoRepository ingressoRepository)
        => _ingressoRepository = ingressoRepository;

    public async Task Handle(CancelarIngressoCommand command, CancellationToken ct = default) {
        var ingresso = await _ingressoRepository.ObterPorIdAsync(command.IngressoId, ct)
            ?? throw new KeyNotFoundException($"Ingresso {command.IngressoId} não encontrado.");

        if (ingresso.UsuarioId != command.UsuarioId)
            throw new DomainException("Você não tem permissão para cancelar este ingresso.");

        ingresso.Cancelar();
        await _ingressoRepository.AtualizarAsync(ingresso, ct);
    }
}