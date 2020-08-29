using System;

namespace EmployeeManagement.Models
{
    public partial class NttEmployeeTimeDetails
    {
        public int EmployeeTimeDetailId { get; set; }
        public int NttEmployeeId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public bool? IsActive { get; set; }
    }
}
