using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Bemobi.Consumer.Consumers.Base
{
    public abstract class BaseEventConsumer
    {
        protected static async Task ProcessAsync<TConsumer, TProcess>(
                ConsumeContext<TConsumer> context,
                string consumeType,
                Func<TProcess, Task> process,
                Func<TProcess, Exception, Task>? exceptionHandler = null,
                Func<TProcess, Task>? finallyHandler = null)
            where TConsumer : class
        {
            var serviceProvider = context.GetPayload<IServiceProvider>();
            var eventProcess = serviceProvider.GetService<TProcess>();
            
            if (eventProcess == null)
                return;

            try
            {
                Debug.WriteLine($"Executing the resource {consumeType}");
                await process(eventProcess);
                Debug.WriteLine($"The resource {consumeType} execution completed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while executing the resource {consumeType}");
                Debug.WriteLine($"{ex.Message}");
                Debug.WriteLine($"{ex.StackTrace}");

                if (exceptionHandler != null)
                    await exceptionHandler(eventProcess, ex);
            }
            finally
            {
                if (finallyHandler != null)
                    await finallyHandler(eventProcess);
            }
        }
    }
}
