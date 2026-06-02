using OrbitPass.Domain.Enums;

namespace OrbitPass.Application.DTOs;

public record IngressoDto(
    Guid Id,
    Guid UsuarioId,
    Guid DataTourId,
    string CodigoUnico,
    StatusIngresso Status,
    DateTime DataCompra,
    decimal ValorPago
);