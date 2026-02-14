using Lemax.Application.Common.Exceptions;
using Lemax.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;
using System.Text;

namespace Lemax.Infrastructure.Middleware;

internal class ExceptionMiddleware : IMiddleware
{
    private readonly ISerializerService _jsonSerializer;

    public ExceptionMiddleware(
        ISerializerService jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            string errorId = Guid.NewGuid().ToString();
            ErrorResult? errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim(),
                ErrorId = errorId,
                SupportMessage = $"Provide the ErrorId {errorId} to the support team for further analysis."
            };
            errorResult.Messages.Add(exception.Message);
            if (exception is not CustomException && exception.InnerException != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
            }

            switch (exception)
            {
                case CustomException e:
                    errorResult.StatusCode = (int)e.StatusCode;
                    if (e.ErrorMessages is not null)
                    {
                        errorResult.Messages = e.ErrorMessages;
                    }

                    break;

                case KeyNotFoundException:
                    errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error($"{errorResult.Exception} Request failed with Status Code {context.Response.StatusCode} and Error Id {errorId}.");
            HttpRequest request = context.Request;

            string requestData = null;

            if (request.ContentLength != null && request.ContentLength != 0)
            {
                request.EnableBuffering();

                byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                requestData = Encoding.UTF8.GetString(buffer);

                request.Body.Position = 0;
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }

            HttpResponse? response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;
                await response.WriteAsync(_jsonSerializer.Serialize(errorResult));
            }
        }
    }
}