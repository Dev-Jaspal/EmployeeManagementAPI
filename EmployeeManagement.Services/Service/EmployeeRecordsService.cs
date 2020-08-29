using EmployeeManagement.DataAccess;
using EmployeeManagement.Model.Dto;
using EmployeeManagement.Models;
using EmployeeManagement.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Services.Service
{
    public class EmployeeRecordsService : IEmployeeRecordsService
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
        public EmployeeRecordsService(GenericUnitOfWork<NTT_DBContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Public Method
        public bool AddRecord(EmployeeRecords employeeRecords)
        {
            bool isSuccess = false;
            try
            {
                if (employeeRecords != null)
                {
                    NttEmployeeRecords nttEmployeeRecords = new NttEmployeeRecords
                    {
                        Email = employeeRecords.Email,
                        EmployeeId = Convert.ToInt32(employeeRecords.EmployeeId),
                        FirstName = employeeRecords.FirstName,
                        LastName = employeeRecords.LastName,
                        IsActive = employeeRecords.IsActive,
                        Password = employeeRecords.Password
                    };
                    NttRoles nttRoles = new NttRoles
                    {
                        EmployeeId = Convert.ToInt32(employeeRecords.EmployeeId),
                        Role = employeeRecords.Roles
                    };
                    _unitOfWork.Repository<NttEmployeeRecords>().Insert(nttEmployeeRecords);
                    _unitOfWork.Repository<NttRoles>().Insert(nttRoles);
                    _unitOfWork.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public bool DeleteRecord(int EmployeeId)
        {
            bool isSuccess = false;
            try
            {
                if (EmployeeId > 0)
                {
                    NttEmployeeRecords nttEmployeeRecords = _unitOfWork.Context.NttEmployeeRecords
                                                            .Where(x => x.EmployeeId == EmployeeId)
                                                            .FirstOrDefault();
                    if (nttEmployeeRecords != null)
                    {
                        _unitOfWork.Repository<NttEmployeeRecords>().Delete(nttEmployeeRecords);
                        _unitOfWork.SaveChanges();
                        isSuccess = true;
                    }
                }
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public List<EmployeeRecords> GetRecords(int EmployeeId)
        {
            if (EmployeeId > 0)
            {
                return _unitOfWork.Context.NttEmployeeRecords
                      .Join(_unitOfWork.Context.NttRoles,
                                  s => s.EmployeeId,
                                  t => t.EmployeeId,
                                  (s, t) => new { EmployeeRecords = s, Roles = t })
                      .Where(x => x.EmployeeRecords.EmployeeId == EmployeeId)
                   .Select(x => new EmployeeRecords
                   {
                       Email = x.EmployeeRecords.Email,
                       EmployeeId = Convert.ToString(x.EmployeeRecords.EmployeeId),
                       FirstName = x.EmployeeRecords.FirstName,
                       LastName = x.EmployeeRecords.LastName,
                       Roles = x.Roles.Role
                   }).ToList();
            }
            return null;
        }

        public List<DataTableEmployeeRecords> GetAllRecords()
        {
            return _unitOfWork.Context.NttEmployeeRecords
                   .Join(_unitOfWork.Context.NttRoles,
                               s => s.EmployeeId,
                               t => t.EmployeeId,
                               (s, t) => new { EmployeeRecords = s, Roles = t })
                .Select(x => new DataTableEmployeeRecords
                {
                    Email = x.EmployeeRecords.Email,
                    EmployeeId = Convert.ToString(x.EmployeeRecords.EmployeeId),
                    Name = x.EmployeeRecords.FirstName + " " + x.EmployeeRecords.LastName,
                    Roles = x.Roles.Role
                }).ToList();
        }

        public bool UpdateRecord(string id, EmployeeRecords employeeRecords)
        {
            bool isSuccess = false;
            try
            {
                if (employeeRecords != null)
                {
                    NttEmployeeRecords nttEmployeeRecords = _unitOfWork.Context.NttEmployeeRecords
                                                            .Where(x => x.EmployeeId == Convert.ToInt32(id)).FirstOrDefault();
                    if (nttEmployeeRecords != null)
                    {
                        if (employeeRecords.Email != nttEmployeeRecords.Email)
                            nttEmployeeRecords.Email = employeeRecords.Email ?? nttEmployeeRecords.Email;
                        if (employeeRecords.EmployeeId != nttEmployeeRecords.EmployeeId.ToString())
                            nttEmployeeRecords.EmployeeId = Convert.ToInt32(id);
                        if (employeeRecords.FirstName != nttEmployeeRecords.FirstName)
                            nttEmployeeRecords.FirstName = employeeRecords.FirstName ?? nttEmployeeRecords.FirstName;
                        if (employeeRecords.LastName != nttEmployeeRecords.LastName)
                            nttEmployeeRecords.LastName = employeeRecords.LastName ?? nttEmployeeRecords.LastName;
                        if (employeeRecords.Password != nttEmployeeRecords.Password && !String.IsNullOrWhiteSpace(employeeRecords.Password))
                            nttEmployeeRecords.Password = employeeRecords.Password;
                        nttEmployeeRecords.IsActive = employeeRecords.IsActive;

                        _unitOfWork.Repository<NttEmployeeRecords>().Update(nttEmployeeRecords);
                        _unitOfWork.SaveChanges();
                        isSuccess = true;
                    }
                }
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }



        #endregion
    }
}
