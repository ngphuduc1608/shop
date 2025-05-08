using Abp.Domain.Repositories;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proj_tt.Controllers;
using proj_tt.Orders;
using proj_tt.Web.Models.Statistics;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Web.Mvc.Controllers
{
    public class OrderStatisticsController : proj_ttControllerBase
    {
        private readonly IOrderAppService _orderAppService;
        private readonly IRepository<Order, int> _orderRepository;
        private readonly ILogger _logger;

        public OrderStatisticsController(
            IOrderAppService orderAppService,
            IRepository<Order, int> orderRepository,
            ILogger logger)
        {
            _orderAppService = orderAppService;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderCountsByPeriod(string period)
        {
            try
            {
                var today = DateTime.Today;
                var query = _orderRepository.GetAll();

                switch (period?.ToLower())
                {
                    case "day":
                        query = query.Where(o => o.CreationTime.Date == today);
                        break;
                    case "week":
                        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
                        query = query.Where(o => o.CreationTime.Date >= startOfWeek && o.CreationTime.Date <= today);
                        break;
                    case "month":
                        query = query.Where(o => o.CreationTime.Year == today.Year && o.CreationTime.Month == today.Month);
                        break;
                    case "year":
                        query = query.Where(o => o.CreationTime.Year == today.Year);
                        break;
                    default:
                        return BadRequest("Invalid period");
                }

                var totalOrders = await query.CountAsync();
                var pendingOrders = await query.CountAsync(o => o.Status == OrderStatus.Pending);
                var completedOrders = await query.CountAsync(o => o.Status == OrderStatus.Processing);
                var cancelledOrders = await query.CountAsync(o => o.Status == OrderStatus.Cancelled);

                _logger.Info($"Retrieved order statistics for period {period}: Total={totalOrders}, Pending={pendingOrders}, Completed={completedOrders}, Cancelled={cancelledOrders}");

                var data = new OrderCountStatisticsViewModel
                {
                    TotalOrders = totalOrders,
                    PendingOrders = pendingOrders,
                    CompletedOrders = completedOrders,
                    CancelledOrders = cancelledOrders,
                    SuccessRate = totalOrders > 0 ? (double)completedOrders / totalOrders * 100 : 0
                };

                return Json(data);
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving order statistics", ex);
                return StatusCode(500, "An error occurred while retrieving order statistics");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderStatusDistribution()
        {
            try
            {
                var query = _orderRepository.GetAll();
                var totalOrders = await query.CountAsync();

                var statusDistribution = await query
                    .GroupBy(o => o.Status)
                    .Select(g => new OrderStatusDistributionViewModel
                    {
                        Status = g.Key.ToString(),
                        Count = g.Count(),
                        Percentage = totalOrders > 0 ? (double)g.Count() / totalOrders * 100 : 0
                    })
                    .ToListAsync();

                _logger.Info($"Retrieved order status distribution: Total={totalOrders}, Statuses={string.Join(", ", statusDistribution.Select(s => $"{s.Status}:{s.Count}"))}");

                return Json(statusDistribution);
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving order status distribution", ex);
                return StatusCode(500, "An error occurred while retrieving order status distribution");
            }
        }
    }
}