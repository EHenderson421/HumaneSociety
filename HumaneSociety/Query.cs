using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {
        public static void RunEmployeeQueries(Employee employee, string crud)
        {
            PerformCrudOperationOnEmployee performCrudDelegate;
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
        
      
    }
}
