namespace proj_tt.Web.Models.Statistics
{
    public class OrderCountStatisticsViewModel
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public double SuccessRate { get; set; }
    }
} 