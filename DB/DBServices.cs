using Newtonsoft.Json;
using SignalRSample.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SignalRSample.DB
{
    public class DbServices : IDbServices
    {
        public List<Employees> GetEmployees()
        {
            var sr = new StreamReader("./w_Employee.json");
            var st = sr.ReadToEnd().Replace("\r\n", "");

            List<Employees> json = string.IsNullOrEmpty(st) ? new List<Employees>() : JsonConvert.DeserializeObject<List<Employees>>(st);
            sr.Close();
            return json;
        }

        public void SetEmployees(List<Employees> json)
        {
            var sw = new StreamWriter("./w_Employee.json");
            sw.WriteLine(JsonConvert.SerializeObject(json));
            sw.Close();
        }

        public List<WolfType> GetWolfTypes()
        {
            var sr = new StreamReader("./w_type.json");
            var st = sr.ReadToEnd().Replace("\r\n", "");
            List<WolfType> w_jsons = JsonConvert.DeserializeObject<List<WolfType>>(st);
            sr.Close();
            return w_jsons;
        }

        public List<Employees> CreateEmployees(Employees employee)
        {
            var employees = GetEmployees();
            employees.Add(employee);
            SetEmployees(employees);
            return employees;
        }

        public WolfType GetWolfTypesById(string wolfTypeId)
        {
            return GetWolfTypes().FirstOrDefault(e => e.Id == wolfTypeId);
        }
    }
}