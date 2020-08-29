using EmployeeManagement.Model.Dto;
using System.Collections.Generic;

namespace EmployeeManagement.Services.IService
{
    public interface IEmployeeEntryRecordsService
    {
        List<EmployeeEntryRecords> GetRecords(int EmployeeId);
        bool AddRecord(EmployeeEntryRecords employeeRecords);
        bool Update(string id, EmployeeEntryRecords employeeEntryRecords);
    }
}
