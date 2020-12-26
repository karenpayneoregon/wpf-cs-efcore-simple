using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using WpfApp1.Models;

namespace WpfApp1.Classes
{
    public class EmployeesOperations
    {
        public static string ConnectionString =
            "Data Source=.\\SQLEXPRESS;database=HumanResources;Integrated Security=True";

        public static List<Employees> List()
        {
            var list = new List<Employees>() ;
            
            var selectStatement =
                @"
            SELECT 
                employee_id, first_name, last_name, email, phone_number, hire_date, job_id, salary, manager_id, department_id 
            FROM 
                dbo.employees;
";

            using (var cn = new SqlConnection() { ConnectionString = ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement})
                {
                    cn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                       
                        var employee = new Employees()
                        {
                            EmployeeId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Email = reader.GetString(3),
                            PhoneNumber = reader.SafeGetString(4),
                            HireDate = reader.GetDateTime(5),
                            JobId = reader.GetInt32(6),
                            Salary = reader.GetDecimal(7),
                            ManagerId = reader["manager_id"].DbCast<int>(),
                            DepartmentId = reader.GetInt32(9),
                            Manager = new Employees(), 
                            InverseManager = new List<Employees>()
                        };

                        list.Add(employee);
                    }
                }
            }

            return list;
        }
    }

    public static class DbExtensions
    {
        public static T? DbCast<T>(this object dbValue) where T : struct
        {
            switch (dbValue)
            {
                case null:
                case DBNull _:
                    return null;
            }

            T? value = dbValue as T?;
            if (value != null)
            {
                return value;
            }

            if (dbValue is IConvertible conv)
            {
                value = (T)conv.ToType(typeof(T), CultureInfo.InvariantCulture);
            }
            
            return value;
        }
        public static string SafeGetString(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
            {
                return reader.GetString(colIndex);
            }

            return string.Empty;
        }
    }
}
