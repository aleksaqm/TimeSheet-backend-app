using System.Net.Mime;
using System.Net;
using System.Text.Json;
using Shared;
using System;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;

namespace TimeSheet.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleCustomExceptionResponseAsync(context, ex);
            }
        }

        private async Task HandleCustomExceptionResponseAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            ProblemDetails response = ex switch
            {
                CategoryNotFoundException _ => new ProblemDetails { Status = StatusCodes.Status400BadRequest , Title = "Category not found", Detail = ex.Message},
                ClientNotFoundException _ => new ProblemDetails { Status = StatusCodes.Status400BadRequest, Title = "Client not found", Detail = ex.Message },
                ProjectNotFoundException _ => new ProblemDetails { Status = StatusCodes.Status400BadRequest, Title = "Project not found", Detail = ex.Message },
                TeamMemberNotFoundException _ => new ProblemDetails { Status = StatusCodes.Status400BadRequest, Title = "Team member not found", Detail = ex.Message },
                InvalidDatesException _ => new ProblemDetails { Status = StatusCodes.Status400BadRequest, Title = "Invalid dates", Detail = ex.Message },
                ActivityNotFoundException _ => new ProblemDetails { Status = StatusCodes.Status400BadRequest, Title = "Activity not found", Detail = ex.Message },
                UsernameAlreadyTakenException _ => new ProblemDetails { Status = StatusCodes.Status400BadRequest, Title = "Username already taken", Detail = ex.Message },
                EmailAlreadyExistsException _ => new ProblemDetails { Status = StatusCodes.Status400BadRequest, Title = "Email already exists", Detail = ex.Message },
                InvalidLoginCredentialsException _ => new ProblemDetails { Status = StatusCodes.Status400BadRequest, Title = "Invalid login credentials", Detail = ex.Message },

                //_ => new ProblemDetails((int)HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.Status;
            await context.Response.WriteAsJsonAsync(response);
        }

    }
}
