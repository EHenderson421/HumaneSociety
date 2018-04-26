using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
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
                    performCrudDelegate = CreateNewEmployee;
                    performCrudDelegate(employee, crud);
                    break;

                case "delete":
                    performCrudDelegate = DeleteOldEmployee;
                    performCrudDelegate(employee, crud);
                    break;

                case "read":
                    performCrudDelegate = ReadEmployeeInfo;
                    performCrudDelegate(employee, crud);
                    break;

                case "update":
                    performCrudDelegate = UpdateEmployeeInfo;
                    performCrudDelegate(employee, crud);
                    break;

                default:
                    break;
            }
        }


        public static void CreateNewEmployee(Employee employee, string create)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            db.Employees.InsertOnSubmit(employee);

            try
            {
                db.SubmitChanges();
            }

            catch (Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }


        public static void DeleteOldEmployee(Employee employee, string delete)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var oldEmployee = (from o in db.Employees where o.employeeNumber == employee.employeeNumber && o.lastName == employee.lastName select o).FirstOrDefault();
            db.Employees.DeleteOnSubmit(oldEmployee);

            try
            {
                db.SubmitChanges();
            }

            catch (Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }


        public static void ReadEmployeeInfo(Employee employee, string read)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var employeeInfo = (from i in db.Employees where i.ID == employee.ID select i).FirstOrDefault();
            UserInterface.DisplayEmployeeInfo(employeeInfo);
        }


        public static void UpdateEmployeeInfo(Employee employee, string update)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            int employeeNumber = int.Parse(UserInterface.GetStringData("employee number", "the employee's original"));
            Employee employeetoUpdate = GetEmployeeByEmployeeNumber(employeeNumber);
            var employeeInContext = (from u in db.Employees where employeetoUpdate.ID == u.ID select u).FirstOrDefault();
            employeeInContext.email = employee.email;
            employeeInContext.lastName = employee.lastName;
            employeeInContext.employeeNumber = employee.employeeNumber;
            employeeInContext.firsttName = employee.firsttName;

            try
            {
                db.SubmitChanges();
            }

            catch (Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }

        public static Employee GetEmployeeByEmployeeNumber(int employeeNumber)
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            var employee = (from e in context.Employees where e.employeeNumber == employeeNumber select e).FirstOrDefault();
            return employee;
        }

        public static List<ClientAnimalJunction> GetUserAdoptionStatus(Client client)
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            var pendingAdoptions = (from p in context.ClientAnimalJunctions where p.client == client.ID select p).ToList();

            return pendingAdoptions;
        }

        public static Animal GetAnimalByID(int id)
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            var animal = (from a in context.Animals where a.ID == id select a).FirstOrDefault();
            return animal;
        }

        public static Table<Client> RetrieveClients()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext("c:/Documents/HumaneSociety/HumaneSociety/HumaneSociety.dbml");
            Table<Client> clients = db.GetTable<Client>();
            return clients;
        }

        public static Client GetClient(string userName, string password)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext("c:/Documents/HumaneSociety/HumaneSociety/HumaneSociety.dbml");
            Table<Client> clients = db.GetTable<Client>();
            var getClient = (from c in db.Clients where c.userName == userName select c).FirstOrDefault();

            if (password != getClient.pass)
            {
                getClient = null;
            }
            return getClient;
        }

        public static void Adopt(Animal animal, Client client)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var pendingAdopt = (from p in db.ClientAnimalJunctions where p.animal == animal.ID && p.client == client.ID select p).FirstOrDefault();
            pendingAdopt.approvalStatus = "pending";
            var animalWanted = (from a in db.Animals where a.ID == animal.ID select a).FirstOrDefault();
            animalWanted.adoptionStatus = "pending";
            db.ClientAnimalJunctions.InsertOnSubmit(pendingAdopt);

            try
            {
                db.SubmitChanges();
            }

            catch (Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }

            db.Animals.InsertOnSubmit(animalWanted);

            try
            {
                db.SubmitChanges();
            }

            catch (Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }

        public static List<ClientAnimalJunction> GetPendingAdoptions()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var pendingAdoption = (from p in db.ClientAnimalJunctions select p).ToList();

            return pendingAdoption;
        }

        public static void UpdateAdoption(bool approve, ClientAnimalJunction clientAnimalJunction)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal animalAdopted = (from a in db.Animals where clientAnimalJunction.animal == a.ID select a).FirstOrDefault();

            if (approve == true)
            {
                clientAnimalJunction.approvalStatus = "approved";
                animalAdopted.adoptionStatus = "adopted";
            }

            else
            {
                clientAnimalJunction.approvalStatus = "rejected";
            }

            try
            {
                db.SubmitChanges();
            }

            catch (Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }

        public static IEnumerable<AnimalShotJunction> GetShots(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            var shots = (from s in db.AnimalShotJunctions where s.Animal_ID == animal.ID select s);
            return shots;
        }

        public static void UpdateShot(string typeOfShot, Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            AnimalShotJunction animalShot = new AnimalShotJunction();
            animalShot.Animal_ID = animal.ID;
            animalShot.Shot_ID = GetShotID(typeOfShot);
            animalShot.dateRecieved = DateTime.Now;
            db.AnimalShotJunctions.InsertOnSubmit(animalShotJunction);

            try
            {
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }
    }


        public static int GetShotID(string name)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            bool shotExist = db.Shots.Any(s => s.name == name);
            if (shotExist == false)
            {
                Shot shotToAdd = new Shot();
                shotToAdd.name = name;
                db.Shots.InsertOnSubmit(shotToAdd);
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    UserInterface.DisplayExceptionMessage(e);
                }
                var shotID = (from s in db.Shots where s.name == name select s.ID).FirstOrDefault();
                return shotID;
            }
        }
    }
}
