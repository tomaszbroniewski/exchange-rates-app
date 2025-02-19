﻿namespace ExchangeRatesApp.Application.TechnicalInterfaces
{
    public interface IHandlerContext
    {
        /// <summary>
        /// Can be set with the ConfigureLock method (default value: null, which means the locking mechanism is turned off).
        /// </summary>
        string? LockId { get; }

        /// <summary>
        /// Validation can be turned off with DontUseCommandValidator mthod. (Default value: false).
        /// </summary>
        bool IsDontUseCommandValidator { get; }
        void DontUseCommandValidator();

        /// <summary>
        /// Pass the lockKey (from ApplicationContext) to lock the execution of a handler.
        /// Be careful not to use embedded locks within the handler's logic!
        /// </summary>
        /// <param name="lockObject"></param>
        /// <param name="lockTimeout"></param>
        void ConfigureLock(string lockKey);
    }
}
