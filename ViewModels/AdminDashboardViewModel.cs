using System.Collections.Generic;

namespace KeyRent.ViewModels
{
    public class AdminDashboardViewModel
    {
        public decimal MonthlyRevenue { get; set; }
        public decimal AnnualRevenue { get; set; }
        public int TasksProgressPercentage { get; set; }
        public int PendingRequests { get; set; }

        // Dodajemy listę użytkowników do wyświetlenia na dashboardzie
        public List<UserViewModel> Users { get; set; }
    }
}
