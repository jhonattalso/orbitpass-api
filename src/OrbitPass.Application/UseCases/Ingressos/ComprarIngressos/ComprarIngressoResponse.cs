using OrbitPass.Domain.Enums;

namespace OrbitPass.Application.UseCases.Ingressos.ComprarIngresso;

public record ComprarIngressoResponse(
    Guid IngressoId,
    string CodigoUnico,
    StatusIngresso StatusIngresso,
    Guid PagamentoId,
    StatusPagamento StatusPagamento
);