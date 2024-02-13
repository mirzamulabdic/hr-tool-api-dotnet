
using API.Interfaces;
using API.Repositories;

namespace API.Services
{
    public class LeavesBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LeavesBackgroundService> _logger;

        public LeavesBackgroundService(IServiceProvider serviceProvider, ILogger<LeavesBackgroundService> logger)
        {
            this._serviceProvider = serviceProvider;
            this._logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    _logger.LogInformation("_Updating leave requests to Taken");
                    var leaveRequestRepository = scope.ServiceProvider.GetRequiredService<ILeaveRequestRepository>();
                    await leaveRequestRepository.UpdateTakenLeaveRequests();
                }
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }
    }
}
