using Microsoft.EntityFrameworkCore;
using ProductWebApi.Models;

namespace ProductWebApi.Services;

public class QueuedHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BackgroundService> _logger;

    public QueuedHostedService(IServiceScopeFactory scopeFactory, ILogger<BackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Việc check hàng đang được tiến hành");
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await CheckProduct(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi khi đang check hàng");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }

    private async Task CheckProduct(CancellationToken cancellationToken)
    {
        using IServiceScope scope =
            _scopeFactory.CreateScope();

        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        List<Product> products = await context.Products
            .Where(p => p.Quantity < 10)
            .ToListAsync(cancellationToken);

        if (products.Count == 0) return;
        foreach (Product p in products)
        {
            _logger.LogInformation($"Hiện tại những mặt hàng này đã hết hàng:{p.Id}, {p.Name}");
        }
    }
}