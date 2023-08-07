


using System.Threading;

namespace Best_web_application_ever.Services
{
    public class CringeCountService : BackgroundService
    {
        public CringeCountService(ICountService countService) 
        { 
            this.someService = countService;
        }

        protected ICountService someService;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Выполняем задачу пока не будет запрошена остановка приложения
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await someService.Dowork();
                }
                catch (Exception ex)
                {
                    // обработка ошибки однократного неуспешного выполнения фоновой задачи
                }

                await Task.Delay(500);
            }

            // Если нужно дождаться завершения очистки, но контролировать время, то стоит предусмотреть в контракте использование CancellationToken
            // await someService.DoSomeCleanupAsync(cancellationToken);
        }
    }
}
