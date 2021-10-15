using System.Collections;
using System.Collections.Generic;
using SignalRSample.Models;

namespace SignalRSample.DB
{
    public interface IDbServices
    {
        WolfType GetWolfTypesById(string wolfTypeId);

        void SetEmployees(List<Employees> json);

        List<Employees> GetEmployees();

        List<WolfType> GetWolfTypes();

        List<Employees> CreateEmployees(Employees employees);
    }
}