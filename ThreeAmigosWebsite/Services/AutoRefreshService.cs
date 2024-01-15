using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using ThreeAmigosWebsite.Services;

namespace ThreeAmigosWebsite.Services{
    public class RefreshService : BackgroundService{
        private readonly ILogger<RefreshService> _logger;
        private readonly IProductService _productService;
        private const double RefreshIntervalInMinutes = 5;

        public RefreshService(IProductService productService, ILogger<RefreshService> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken){
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Refreshing page and product data");
                    await _productService.GetProductDataAsync();
                    _logger.LogInformation("Completed refresh and products refetch");
                    await Task.Delay(TimeSpan.FromMinutes(RefreshIntervalInMinutes), stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}