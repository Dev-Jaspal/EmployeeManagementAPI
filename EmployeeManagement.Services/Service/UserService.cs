using EmployeeManagement.BusinessModel.Dto;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using EmployeeManagement.Services.IService;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Services.Service
{
    public class UserService : IUserService
    {
        #region Private Member Variables
        /// <summary>
        /// Initialise generic data context variable.
        /// </summary>
        private readonly GenericUnitOfWork<NTT_DBContext> _unitOfWork;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the dependencies of services
        /// </summary>
        /// <param name="unitOfWork">unit of work for repository</param>
        public UserService(GenericUnitOfWork<NTT_DBContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Public Method
        public LoginModel AuthenticateUserFromDb(int employeeId, string password)
        {
            if (employeeId > 0 && !string.IsNullOrEmpty(password))
            {
                // add left outer join with table Ntt_Roles to get role on the bases of employee id.
                LoginModel userDetail = _unitOfWork.Context.NttEmployeeRecords
                     .Join(_unitOfWork.Context.NttRoles,
                                 s => s.EmployeeId,
                                 t => t.EmployeeId,
                                 (s, t) => new { EmployeeRecords = s, Roles = t })
                     .Where(x => x.EmployeeRecords.EmployeeId == employeeId && x.EmployeeRecords.Password == password)
                     .Select(x => new LoginModel
                     {
                         Role = x.Roles.Role,
                         FirstName = x.EmployeeRecords.FirstName,
                         LastName = x.EmployeeRecords.LastName,
                         EmployeeId = x.EmployeeRecords.EmployeeId.ToString(),
                         Password = x.EmployeeRecords.Password
                     }).FirstOrDefault();

                #region commented code
                //LoginModel userDetail = _unitOfWork.Context.NttEmployeeRecords
                //  .Where(x => x.EmployeeId == employeeId && x.Password == password).Select(x => new LoginModel
                //  {
                //    EmployeeId = x.EmployeeId,
                //    Password = x.Password
                //  }).FirstOrDefault();
                #endregion

                if (userDetail != null)
                    return userDetail;
            }
            return null;
        }
        #endregion
    }
}
