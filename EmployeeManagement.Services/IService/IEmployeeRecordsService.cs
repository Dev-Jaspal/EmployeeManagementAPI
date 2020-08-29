using EmployeeManagement.Model.Dto;
using System.Collections.Generic;

namespace EmployeeManagement.Services.IService
{
    public interface IEmployeeRecordsService
    {
        List<DataTableEmployeeRecords> GetAllRecords();
        List<EmployeeRecords> GetRecords(int EmployeeId);
        bool DeleteRecord(int EmployeeId);
        bool AddRecord(EmployeeRecords employeeRecords);
        bool UpdateRecord(string id, EmployeeRecords employeeRecords);
    }
}
