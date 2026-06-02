using OrbitPass.Application.DTOs;
using OrbitPass.Domain.Interfaces;

namespace OrbitPass.Application.UseCases.Ingressos.ListarIngressos;

public class ListarIngressosHandler {
    private readonly IIngressoRepository _ingressoRepository;

    public ListarIngressosHandler(IIngressoRepository ingressoRepository)
        => _ingressoRepository = ingressoRepository;

    public async Task<IEnumerable<IngressoDto>> Handle(
        ListarIngressosQuery query, CancellationToken ct = default) {
        var ingressos = await _ingressoRepository.ListarPorUsuarioAsync(query.UsuarioId, ct);

        return ingressos.Select(i => new IngressoDto(
            i.Id,
            i.UsuarioId,
            i.DataTourId,
            i.CodigoUnico,
            i.Status,
            i.DataCompra,
            i.ValorPago));
    }
}