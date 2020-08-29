using System;

namespace EmployeeManagement.Models
{
    public partial class NttEmployeeEntryRecords
    {
        public int EmployeeId { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public bool IsActive { get; set; }
        public int NttEmployeeId { get; set; }

        public virtual NttEmployeeRecords NttEmployee { get; set; }
    }
}
