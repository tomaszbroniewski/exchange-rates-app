using Microsoft.EntityFrameworkCore;
using Polly.CircuitBreaker;
using System;
using System.Data.Common;
using System.Linq;
using ExchangeRatesApp.Api.StartupConfigs.Middlewares;
using ExchangeRatesApp.Domain;
using ExchangeRatesApp.Domain.Exceptions;
using ExchangeRatesApp.Dto;

namespace ExchangeRatesApp.Api.Helpers
{
    public static class ErrorResultDtoFactory
    {
        public static ErrorResultDto Create(Exception exception, string errorLogId)
        {
            if (exception is UserException ue)
            {
                return new ErrorResultDto(ue, errorLogId);
            }
            else if (exception is DbUpdateConcurrencyException)
            {
                return new ErrorResultDto(new UserException("The functionality has encountered temporary problem. Please try again.", ExchangeRatesAppErrorCodes.Other), errorLogId);
            }
            else if ((exception is DbException && PollyMiddleware.DbExceptionCodes.Any(dbErrorCode => exception.Message.Contains(dbErrorCode)))
                        || exception is BrokenCircuitException)
            {
                return new ErrorResultDto(new UserException("The functionality is busy. Please try again in a while.", ExchangeRatesAppErrorCodes.DbTimeoutError), errorLogId);
            }

            return new ErrorResultDto(new UserException($"The functionality has encountered problem. Please contact the support. Error Id: {errorLogId}", ExchangeRatesAppErrorCodes.ServerError), errorLogId);
        }
    }
}
