﻿using System;
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
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            context.Employees.InsertOnSubmit(employee);

            try
            {
                context.SubmitChanges();
            }

            catch(Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }


        public static void DeleteOldEmployee(Employee employee, string delete)
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            var oldEmployee = (from o in context.Employees where o.employeeNumber == employee.employeeNumber && o.lastName == employee.lastName select o).FirstOrDefault();
            context.Employees.DeleteOnSubmit(oldEmployee);

            try
            {
                context.SubmitChanges();
            }

            catch(Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }


        public static void ReadEmployeeInfo(Employee employee, string read)
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            var employeeInfo = (from i in context.Employees where i.ID == employee.ID select i).FirstOrDefault();
            UserInterface.DisplayEmployeeInfo(employeeInfo);
        }


        public static void UpdateEmployeeInfo(Employee employee, string update)
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            int employeeNumber = int.Parse(UserInterface.GetStringData("employee number", "the employee's original"));
            Employee employeetoUpdate = GetEmployeeByEmployeeNumber(employeeNumber);
            var employeeInContext = (from u in context.Employees where employeetoUpdate.ID == u.ID select u).FirstOrDefault();
            employeeInContext.email = employee.email;
            employeeInContext.lastName = employee.lastName;
            employeeInContext.employeeNumber = employee.employeeNumber;
            employeeInContext.firsttName = employee.firsttName;
            try
            {
                context.SubmitChanges();
            }
            catch(Exception e)
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

        public static List <ClientAnimalJunction> GetUserAdoptionStatus(Client client)
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
            HumaneSocietyDataContext db = new HumaneSocietyDataContext( "c:/Documents/HumaneSociety/HumaneSociety/HumaneSociety.dbml");
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
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            var pendingAdopt = (from p in context.ClientAnimalJunctions where p.animal == animal.ID && p.client == client.ID select p).FirstOrDefault();
            pendingAdopt.approvalStatus = "pending";
            var animalWanted = (from a in context.Animals where a.ID == animal.ID select a).FirstOrDefault();
            animalWanted.adoptionStatus = "pending";

            context.ClientAnimalJunctions.InsertOnSubmit(pendingAdopt);
            try
            {
                context.SubmitChanges();
            }
            catch(Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }

            context.Animals.InsertOnSubmit(animalWanted);
            try
            {
                context.SubmitChanges();
            }
            catch(Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }

        public static List<ClientAnimalJunction> GetPendingAdoptions()
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            var pendingAdoption = (from p in context.ClientAnimalJunctions select p).ToList();

            return pendingAdoption;
        }

        public static void UpdateAdoption(bool approve, ClientAnimalJunction clientAnimalJunction)
        {
            HumaneSocietyDataContext context = new HumaneSocietyDataContext();
            Animal animalAdopted = (from a in context.Animals where clientAnimalJunction.animal == a.ID select a).FirstOrDefault();

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
                context.SubmitChanges();
            }
        
            catch (Exception e)
            {
                UserInterface.DisplayExceptionMessage(e);
            }
        }
    }
}
