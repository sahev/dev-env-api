using Domain.Constants;
using Domain.Enums;
using Domain.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Extensions;
public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {

        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            AllowStatusCode404Response = true,
            ExceptionHandler = async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorId = Guid.NewGuid();

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    string errorMessage = string.Empty;
                    string errorCode = string.Empty;

                    if (contextFeature.Error is UserFriendlyException userFriendlyException)
                    {
                        switch (userFriendlyException.ErrorCode)
                        {
                            case ErrorCode.NotFound:
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = ErrorRespondCode.NOT_FOUND;
                                break;
                            case ErrorCode.VersionConflict:
                                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = ErrorRespondCode.VERSION_CONFLICT;
                                break;
                            case ErrorCode.ItemAlreadyExists:
                                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                                errorMessage = userFriendlyException.UserFriendlyMessage;

                                errorCode = ErrorRespondCode.ITEM_ALREADY_EXISTS;
                                break;
                            case ErrorCode.Conflict:
                                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                                errorMessage = userFriendlyException.UserFriendlyMessage;

                                errorCode = ErrorRespondCode.CONFLICT;
                                break;
                            case ErrorCode.BadRequest:
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = ErrorRespondCode.BAD_REQUEST;
                                break;
                            case ErrorCode.Unauthorized:
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = ErrorRespondCode.UNAUTHORIZED;
                                break;
                            case ErrorCode.Internal:
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = ErrorRespondCode.INTERNAL_ERROR;
                                break;
                            case ErrorCode.UnprocessableEntity:
                                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = ErrorRespondCode.UNPROCESSABLE_ENTITY;
                                break;
                            default:
                                context.Response.StatusCode = 500;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = ErrorRespondCode.GENERAL_ERROR;
                                break;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 500;
                        errorCode = ErrorRespondCode.GENERAL_ERROR;
                        errorMessage = contextFeature.Error.Message;
                    }
                    await context.Response.WriteAsync(new Error(errorCode, errorMessage, errorId));
                    logger.LogError("ErrorId:{errorId} Exception:{contextFeature.Error}", errorId, contextFeature.Error);
                }
            }
        });
    }
}
