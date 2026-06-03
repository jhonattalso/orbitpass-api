using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrbitPass.Application.UseCases.Pagamentos.ProcessarPagamento;

namespace OrbitPass.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PagamentosController : ControllerBase {
    private readonly ProcessarPagamentoHandler _handler;

    public PagamentosController(ProcessarPagamentoHandler handler)
        => _handler = handler;

    /// <summary>Processa o pagamento de um ingresso.</summary>
    [HttpPost]
    public async Task<IActionResult> Processar(
        [FromBody] ProcessarPagamentoCommand command, CancellationToken ct) {
        var resultado = await _handler.Handle(command, ct);
        return Ok(resultado);
    }
}