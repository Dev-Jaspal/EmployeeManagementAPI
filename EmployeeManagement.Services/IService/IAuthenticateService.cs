using EmployeeManagement.BusinessModel.Dto;

namespace EmployeeManagement.Services.IService
{
    public interface IAuthenticateService
    {
        LoginModel Authenticate(int employeeId, string password);
    }
}
