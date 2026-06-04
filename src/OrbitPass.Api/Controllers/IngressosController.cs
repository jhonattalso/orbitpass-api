using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrbitPass.Application.UseCases.Ingressos.CancelarIngresso;
using OrbitPass.Application.UseCases.Ingressos.ComprarIngresso;
using OrbitPass.Application.UseCases.Ingressos.ListarIngressos;

namespace OrbitPass.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IngressosController : ControllerBase {
    private readonly ComprarIngressoHandler _comprarHandler;
    private readonly CancelarIngressoHandler _cancelarHandler;
    private readonly ListarIngressosHandler _listarHandler;

    public IngressosController(
        ComprarIngressoHandler comprarHandler,
        CancelarIngressoHandler cancelarHandler,
        ListarIngressosHandler listarHandler) {
        _comprarHandler = comprarHandler;
        _cancelarHandler = cancelarHandler;
        _listarHandler = listarHandler;
    }

    /// <summary>Lista todos os ingressos do usuário informado.</summary>
    [HttpGet("usuario/{usuarioId:guid}")]
    public async Task<IActionResult> Listar(Guid usuarioId, CancellationToken ct) {
        var resultado = await _listarHandler.Handle(
            new ListarIngressosQuery(usuarioId), ct);
        return Ok(resultado);
    }

    /// <summary>Compra um ingresso e processa o pagamento.</summary>
    [HttpPost("comprar")]
    public async Task<IActionResult> Comprar(
        [FromBody] ComprarIngressoCommand command, CancellationToken ct) {
        var resultado = await _comprarHandler.Handle(command, ct);
        return CreatedAtAction(nameof(Listar),
            new { usuarioId = command.UsuarioId }, resultado);
    }

    /// <summary>Cancela um ingresso existente.</summary>
    [HttpDelete("{ingressoId:guid}/usuario/{usuarioId:guid}")]
    public async Task<IActionResult> Cancelar(
        Guid ingressoId, Guid usuarioId, CancellationToken ct) {
        await _cancelarHandler.Handle(
            new CancelarIngressoCommand(ingressoId, usuarioId), ct);
        return NoContent();
    }
}