using OrbitPass.Api.Extensions;
using OrbitPass.Api.Middlewares;
using OrbitPass.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("ORACLE_CONNECTION_STRING")
    ?? throw new InvalidOperationException("ORACLE_CONNECTION_STRING não definida.");

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
    ?? throw new InvalidOperationException("JWT_SECRET_KEY não definida.");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();
builder.Services.AddJwtAuthentication(builder.Configuration, jwtSecret);
builder.Services.AddHealthChecksConfig(connectionString);
builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrbitPass v1"));
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();