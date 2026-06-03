using System.Net;
using System.Text.Json;
using OrbitPass.Domain.Exceptions;

namespace OrbitPass.Api.Middlewares;

public class ExceptionHandlingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        }
        catch (DomainException ex) {
            _logger.LogWarning("Regra de negócio violada: {Message}", ex.Message);
            await EscreverRespostaAsync(context, HttpStatusCode.UnprocessableEntity,
                "Regra de negócio", ex.Message);
        }
        catch (KeyNotFoundException ex) {
            await EscreverRespostaAsync(context, HttpStatusCode.NotFound,
                "Recurso não encontrado", ex.Message);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Erro inesperado");
            await EscreverRespostaAsync(context, HttpStatusCode.InternalServerError,
                "Erro interno", "Ocorreu um erro inesperado. Tente novamente.");
        }
    }

    private static Task EscreverRespostaAsync(
        HttpContext ctx, HttpStatusCode code, string tipo, string mensagem) {
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)code;

        var body = JsonSerializer.Serialize(new {
            status = (int)code,
            tipo,
            mensagem,
            timestamp = DateTime.UtcNow
        });

        return ctx.Response.WriteAsync(body);
    }
}