using ExchangeRatesApp.Application.TechnicalInterfaces;

namespace ExchangeRatesApp.Infrastructure
{
    public class HandlerContext : IHandlerContext
    {
        private string? _lockKey;
        public string? LockId => _lockKey;
        public void ConfigureLock(string lockKey)
        {
            _lockKey = lockKey;
        }

        private bool _dontUseCommandValidator;
        public bool IsDontUseCommandValidator => _dontUseCommandValidator;

        public void DontUseCommandValidator()
        {
            _dontUseCommandValidator = true;
        }
    }
}
