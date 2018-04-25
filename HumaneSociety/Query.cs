using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {
        public delegate void PerfromCrudOperationOnEmplyee(Employee employee, string crud);

        public static void RunEmployeeQueries(Employee employee, string crud)
        {
            PerfromCrudOperationOnEmplyee performCrudDelegate;
            switch (crud)
            {
                case "create":
                    performCrudDelegate = CreateEmployee;
                    performCrudDelegate(employee, crud);
                    break;

                case "delete":
                    performCrudDelegate = DeleteEmployee;
                    performCrudDelegate(employee, crud);
                    break;

                case "read":
                    performCrudDelegate = ReadEmployee;
                    performCrudDelegate(employee, crud);
                    break;

                case "update":
                    performCrudDelegate = UpdateEmployee;
                    performCrudDelegate(employee, crud);
                    break;

                default:
                    break;
            }
        }
        

        public static void CreateEmployee(Employee employee, string create)
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            context.Employees.InsertOnSubmit(employee);
            try
            {
                context.SubmitChanges();
            }
            catch (Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }


      
    }
}
