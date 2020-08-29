using System;

namespace EmployeeManagement.Model.Dto
{
    public class EmployeeRecords
    {
        public string EmployeeId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public bool IsActive { get; set; }
        public String Password { get; set; }
        public String ConfirmPassword { get; set; }
        public string Roles { get; set; }
    }

    public class DataTableEmployeeRecords
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public String Email { get; set; }
        public string Roles { get; set; }
    }

}
