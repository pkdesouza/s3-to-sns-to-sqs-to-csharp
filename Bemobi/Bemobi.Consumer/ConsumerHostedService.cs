using MassTransit;
using Microsoft.Extensions.Hosting;

namespace Bemobi.Consumer
{
    public class ConsumerHostedService : IHostedService
    {
        readonly IBusControl _bus;

        public ConsumerHostedService(IBusControl bus)
        {
            _bus = bus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bus.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _bus.StopAsync(cancellationToken);
        }
    }
}
