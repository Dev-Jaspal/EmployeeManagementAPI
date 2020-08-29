using System;

namespace EmployeeManagement.BusinessModel.Dto
{
    public class LoginModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //  public string UserName { get; set; }
        public String Password { get; set; }
        public string Token { get; set; }
        public String Role { get; set; }
    }
}
