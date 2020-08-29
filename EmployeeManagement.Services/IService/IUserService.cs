using EmployeeManagement.BusinessModel.Dto;

namespace EmployeeManagement.Services.IService
{
    public interface IUserService
    {
        LoginModel AuthenticateUserFromDb(int employeeId, string password);
    }
}
