using EmployeeManagement.DataAccess;
using EmployeeManagement.Model.Dto;
using EmployeeManagement.Models;
using EmployeeManagement.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Services.Service
{
    public class EmployeeEntryRecordsService : IEmployeeEntryRecordsService
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
        public EmployeeEntryRecordsService(GenericUnitOfWork<NTT_DBContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Public Method
        public bool AddRecord(EmployeeEntryRecords employeeRecords)
        {
            bool isSuccess = false;
            try
            {
                if (employeeRecords != null)
                {
                    var records = _unitOfWork.Context.NttEmployeeRecords.Where(x => x.EmployeeId == employeeRecords.EmployeeId).FirstOrDefault();
                    if (records != null)
                    {
                        NttEmployeeTimeDetails nttEmployeeTimeDetails = new NttEmployeeTimeDetails
                        {
                            NttEmployeeId = records.NttEmployeeId,
                            EmployeeId = employeeRecords.EmployeeId,
                            InTime = employeeRecords.InTime ?? DateTime.MaxValue,
                            OutTime = employeeRecords.OutTime ?? DateTime.MaxValue,
                            IsActive = employeeRecords.IsActive
                        };
                        _unitOfWork.Repository<NttEmployeeTimeDetails>().Insert(nttEmployeeTimeDetails);
                        _unitOfWork.SaveChanges();
                        isSuccess = true;
                    }

                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public List<EmployeeEntryRecords> GetRecords(int EmployeeId)
        {
            try
            {
                if (EmployeeId > 0)
                {
                    return _unitOfWork.Context.NttEmployeeTimeDetails.Where(x => x.EmployeeId == EmployeeId)
                       .Select(x => new EmployeeEntryRecords
                       {
                           EmployeeId = x.EmployeeId,
                           InTime = x.InTime,
                           OutTime = x.OutTime,
                       }).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public bool Update(string id, EmployeeEntryRecords employeeEntryRecords)
        {
            bool isSuccess = false;
            try
            {
                if (employeeEntryRecords != null)
                {
                    var records = _unitOfWork.Context.NttEmployeeTimeDetails.Where(x => x.EmployeeId == Convert.ToInt32(id)).OrderByDescending(x => x.EmployeeTimeDetailId).FirstOrDefault();
                    if (records != null)
                    {
                        records.OutTime = employeeEntryRecords.OutTime ?? DateTime.MaxValue;
                        _unitOfWork.Repository<NttEmployeeTimeDetails>().Update(records);
                        _unitOfWork.SaveChanges();
                        isSuccess = true;
                    }

                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        #endregion
    }
}
