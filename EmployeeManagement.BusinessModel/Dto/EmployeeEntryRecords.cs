using System;

namespace EmployeeManagement.Model.Dto
{
    public class EmployeeEntryRecords
    {
        public int EmployeeId { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public bool IsActive { get; set; }
    }
}
