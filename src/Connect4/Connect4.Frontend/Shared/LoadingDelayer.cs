using Microsoft.Extensions.Configuration;

namespace Connect4.Frontend.Shared
{
    internal class LoadingDelayer(IConfiguration configuration)
    {
        private readonly int globalDelay = configuration.GetValue("FRONTEND_LOADING_DELAY", 0);

        public async Task Delay(int? delay = null)
        {
            delay ??= this.globalDelay;

            await Task.Delay(delay ?? 0);
        }
    }
}
