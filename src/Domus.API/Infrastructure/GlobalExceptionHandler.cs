using Domus.Core.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Domus.Api.Infrastructure;

/// <summary>
/// Tratador global de exceções que mapeia erros de domínio para respostas HTTP com <see cref="ProblemDetails"/>.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    /// <summary>Logger para registro das exceções capturadas.</summary>
    private readonly ILogger<GlobalExceptionHandler> _logger;

    /// <summary>
    /// Inicializa o handler com o logger injetado.
    /// </summary>
    /// <param name="logger">Logger utilizado para registrar as exceções.</param>
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Tenta tratar a exceção e escreve uma resposta <see cref="ProblemDetails"/> adequada no contexto HTTP.
    /// </summary>
    /// <param name="httpContext">Contexto HTTP da requisição atual.</param>
    /// <param name="exception">A exceção capturada.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns><c>true</c> se a exceção foi tratada; caso contrário, <c>false</c>.</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        ProblemDetails problemDetails = new ProblemDetails { Instance = httpContext.Request.Path };

        switch (exception)
        {
            case FormException formException:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation Error";
                problemDetails.Detail = formException.Message;
                problemDetails.Extensions["errors"] = formException.Errors;
                break;

            case NotFoundException notFoundException:
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                problemDetails.Title = "Resource Not Found";
                problemDetails.Detail = notFoundException.Message;
                break;

            case BusinessRuleException businessRuleException:
                httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                problemDetails.Title = "Business Rule Violation";
                problemDetails.Detail = businessRuleException.Message;
                break;

            case UnauthorizedAccessException unauthorizedAccessException:
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                problemDetails.Title = "Forbidden";
                problemDetails.Detail = unauthorizedAccessException.Message;
                break;

            default:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Internal Server Error";
                problemDetails.Detail = "An unexpected error occurred. Please try again later.";
                break;
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
