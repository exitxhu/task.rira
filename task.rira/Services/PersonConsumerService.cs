using task.rira.Abstractions;
using task.rira.sdk;

namespace task.rira.Services;

public class PersonConsumerService(ILogger<PersonConsumerService> logger, IServiceScopeFactory scopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {

            try
            {
                while (true)
                {
                    using var scope = scopeFactory.CreateScope();

                    var queue = scope.ServiceProvider.GetRequiredService<IQueue>();
                    var repo = scope.ServiceProvider.GetRequiredService<IPersonRepo>();

                    if (queue.TryDequeue("person_insert", out var val) && val is PersonDto dto)
                    {
                        await repo.Insert(dto);
                    }
                    else
                        await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Something happend in person consumer");// not an informant message:)
                throw;
            }
        });
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {

    }
}