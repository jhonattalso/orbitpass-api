using OrbitPass.Domain.Enums;

namespace OrbitPass.Application.DTOs;

public record PagamentoDto(
    Guid Id,
    Guid IngressoId,
    MetodoPagamento Metodo,
    StatusPagamento Status,
    DateTime DataPagamento,
    decimal Valor
);