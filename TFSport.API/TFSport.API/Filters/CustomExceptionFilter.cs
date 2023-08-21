﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using TFSport.Models;

namespace TFSport.API.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An exception occurred in the application.");

            if (context.Exception is CustomException customException)
            {
                HandleCustomException(context, customException);
            }
            else
            {
                HandleOtherExceptions(context);
            }
        }

        private void HandleCustomException(ExceptionContext context, CustomException customException)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = customException.DisplayMessage;

            _logger.LogInformation("Custom exception occurred in {actionName}: {exceptionMessage}. Stack Trace: {exceptionStack}", actionName, exceptionMessage, exceptionStack);

            var response = new
            {
                Message = exceptionMessage
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            context.ExceptionHandled = true;
        }

        private void HandleOtherExceptions(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;

            _logger.LogError("Unhandled exception occurred in {actionName}: {exceptionMessage}. Stack Trace: {exceptionStack}", actionName, exceptionMessage, exceptionStack);

            var response = new
            {
                Message = exceptionMessage
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}