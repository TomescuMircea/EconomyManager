using System;
using System.Collections.Generic;
using System.Text;
using EconomyManager.Domain.Models;
using EconomyManager.Infrastructure;
using EconomyManager.Domain.SeedWork;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;

namespace EconomyManager.Domain.SeedWork
{
    public class Meniu
    {
        /*
        public static string MenuDialog {
            get
            {
                return "IsIncome the number of the action you want to make:\n" +
                        // Actions on a User
                        "On a User object:\n" +
                        "\t1. Create User\n" +
                        "\t2. Delete User\n" +
                        "\t3. Update User\n" +
                        "\t4. Retrieve all the Users\n" +
                        "\t5. Retrieve a User by his email\n" +
                        "\t6. Retrieve a User by his name\n" +
                        "\n" +

                        // Actions on a Wallet
                        "On a Wallet object:\n" +
                        "\t11. Create Wallet\n" +
                        "\t12. Delete Wallet\n" +
                        "\t13. Update Wallet\n" +
                        "\t14. Retrieve all the Wallets\n" +
                        "\t15. Delete a Wallet by User and By Name\n" +
                        "\t16. Delete all the Wallets of a User\n" +
                        "\t17. Retrieve a Wallet by the name\n" +
                        "\n" +

                        // Actions on a UserHasWallet (make modification the the relathionships
                        "On a UserHasWallet object:\n" +
                        "\t21. Create UserHasWallet\n" +
                        "\t22. Delete UserHasWallet\n" +
                        "\t23. Update UserHasWallet\n" +
                        "\t24. Retrieve all the relationships (pairs: UserID, WalletID\n" +
                        "\t25. Retrieve all the Wallets based on a User ID\n" +
                        "\t26. Retrieve a UserHasWallet by the ID of the relationship\n" +
                        "\n" +

                        // Actions on a Plan
                        "On a Plan object:\n" +
                        "\t31. Create Plan\n" +
                        "\t32. Delete Plan\n" +
                        "\t33. Update Plan\n" +
                        "\t34. Retrieve all the Plans\n" +
                        "\t35. Retrieve all the Plans of a Wallet\n" +
                        "\t36. Retrieve a Plan by its ID\n" +
                        "\n" +

                        // Actions on a Category
                        "On a Category object:\n" +
                        "\t41. Create Category\n" +
                        "\t42. Delete Category\n" +
                        "\t43. Update Category\n" +
                        "\t44. Retrieve all the Categories\n" +
                        "\t45. Retrieve all the Categories from a Wallet\n" +
                        "\t46. Retrieve all the Categories from a User\n" +
                        "\t47. Retrieve a Category by its ID\n" +
                        "\n" +

                        // Actions on a Movement
                        "On a Movement object:\n" +
                        "\t51. Create Movement\n" +
                        "\t52. Delete Movement\n" +
                        "\t53. Update Movement\n" +
                        "\t54. Retrieve all the Movements\n" +
                        "\t55. Retrieve a Movement by its ID\n" +
                        "\t56. Retrieve all the Movements of a Wallet\n" +
                        "\n" +

                        // Actions on a AutomaticMovement
                        "On a AutomaticMovement object:\n" +
                        "\t61. Create AutomaticMovement\n" +
                        "\t62. Delete AutomaticMovement\n" +
                        "\t63. Update AutomaticMovement\n" +
                        "\t64. Retrieve all the AutomaticMovements\n" +
                        "\t65. Retrieve a AutomaticMovement by its ID\n" +
                        "\nOption: ";
            }
        }
        public static async Task<List<Entity>> ProcessAsync(int option, out bool success)
        {

            List<Entity> entities = null; //out variable
            string value = Console.ReadLine();

            //if the value is not a int number the function will throw an error
            int valueInteger = Int32.Parse(value);

            if (valueInteger >= 1 && valueInteger <= 6)
            {
                entities = await UserOptionsAsync(value);
                if(!entities.Any() || entities[0].Id!=-1)
                    success = true;
                else
                    success = false;


            }
            else if (valueInteger >= 11 && valueInteger <= 17)
            {
                WalletOptionsAsync(value);
            }
            else if (valueInteger >= 21 && valueInteger <= 26)
            {
                UserHasWalletOptions(value);
            }
            else if (valueInteger >= 31 && valueInteger <= 36)
            {
                PlanOptions(value);
            }
            else if (valueInteger >= 41 && valueInteger <= 47)
            {
                CategoryOptions(value);
            }
            else if (valueInteger >= 51 && valueInteger <= 56)
            {
                MovementOptionsAsync(value);
            }
            else if (valueInteger >= 61 && valueInteger <= 65)
            {
                AutomaticMovementOptions(value);
            }
            else
            {
                Console.WriteLine("Wrong value");
            }
        }
        private static async Task<List<Entity>> UserOptionsAsync(string value)
        {
            UnitOfWork uow = new UnitOfWork();
            List<Entity> entities = new List<Entity>();
            switch (value)
            {
                case "1":
                    {
                        Console.WriteLine("Insert the name of the User:");
                        string name = Console.ReadLine();
                        Console.WriteLine("Insert the email of the User:");
                        string emil = Console.ReadLine();
                        Console.WriteLine("Insert the password of the User:");
                        string password = Console.ReadLine();
                        User user = new User()
                        {
                            Name = name,
                            Email = emil,
                            Password = password
                        };
                        uow.UserRepository.Create(user);
                        uow.SaveAsync();
                        entities.Add(user);
                    }
                    break;
                case "2":
                    {
                        Console.WriteLine("Insert the name of the User:");
                        string name = Console.ReadLine();
                        User user = await uow.UserRepository.FindByNameAsync(name);
                        entities.Add((user == null) ? new User()
                        {
                            Name = "Not Found",
                            Id = -1,
                            Active = false,
                        }
                        : user);
                        if(user!=null) 
                            uow.UserRepository.Delete(user);
                    }
                    break;
                case "3":
                    break;
                case "4":
                    {
                        var users = await uow.UserRepository.FindAllAsync();
                        foreach (User user in users)
                        {
                            entities.Add(user);
                        }
                    }
                    break;
                case "5":
                    {
                        Console.WriteLine("Insert the email of the User:");
                        string email = Console.ReadLine();
                        User user = await uow.UserRepository.FindByEmailAsync(email);
                        entities.Add((user == null) ? new User()
                        {
                            Name = "Not Found",
                            Id = -1,
                            Active = false,
                        } 
                        : user);
                    }
                    break;
                case "6":
                    {
                        Console.WriteLine("Insert the name of the User:");
                        string name = Console.ReadLine();
                        User user = await uow.UserRepository.FindByNameAsync(name);
                        entities.Add((user == null) ? new User()
                        {
                            Name = "Not Found",
                            Id = -1,
                            Active = false,
                        }
                        : user);
                    }
                    break;
                default:
                    entities = null;
                    break;
            }
            await uow.SaveAsync();
            return entities;
        }
        private static async Task<Entity> WalletOptionsAsync(string value)
        {
            UnitOfWork uow = new UnitOfWork();
            switch (value)
            {
                case "1":
                    {

                        Console.WriteLine("Insert the name of the Wallet:");
                        string name = Console.ReadLine();
                        Console.WriteLine("Insert the description of the Wallet:");
                        string description = Console.ReadLine();
                        Wallet wallet = new Wallet()
                        {
                            Name = name,
                            Description = description
                        };
                        uow.WalletRepository.Create(wallet);
                    }
                    break;
                case "2":
                    {
                        Console.WriteLine("Insert the name of the User:");
                        string nameUser = Console.ReadLine();

                        Console.WriteLine("Insert the name of the Wallet:");
                        string nameWallet = Console.ReadLine();

                        User user = await uow.UserRepository.FindByNameAsync(nameUser);

                        UnitOfWork uow2 = new UnitOfWork();
                        
                        uow2.WalletRepository.DeleteByUserByNameAsync(user, nameWallet);

                        await uow2.SaveAsync();
                    }
                    break;
                case "3":
                    break;
                case "4":
                    {
                        var wallets = await uow.WalletRepository.FindAllAsync();
                        foreach(Wallet wallet in wallets)
                        {
                            Console.WriteLine(wallet);
                        }
                    }
                    break;
                case "5":
                    {
                        Console.WriteLine("Insert the name of the User:");
                        string nameUser = Console.ReadLine();

                        Console.WriteLine("Insert the name of the Wallet:");
                        string nameWallet = Console.ReadLine();

                        User user = await uow.UserRepository.FindByNameAsync(nameUser);

                        UnitOfWork uow2 = new UnitOfWork();

                        uow2.WalletRepository.DeleteByUserByNameAsync(user, nameWallet);

                        await uow2.SaveAsync();
                    }
                    break;
                case "6":
                    {
                        Console.WriteLine("Insert the name of the User:");
                        string nameUser = Console.ReadLine();

                        User user = await uow.UserRepository.FindByNameAsync(nameUser);

                        UnitOfWork uow2 = new UnitOfWork();

                        uow2.WalletRepository.DeleteAllByUserAsync(user);

                        await uow2.SaveAsync();
                    }
                    break;
                case "7":
                    {
                        Console.WriteLine("Insert the name of the Wallet:");
                        string nameWallet = Console.ReadLine();

                        Wallet wallet = await uow.WalletRepository.FindByNameAsync(nameWallet);

                        Console.WriteLine(wallet);
                    }
                    break;
                default:
                    break;
            }
            uow.SaveAsync();
        }
        private static async Task<Entity> UserHasWalletOptions(string value)
        {
            switch (value)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "6":
                    break;
                default:
                    break;
            }
        }
        private static async Task<Entity> PlanOptions(string value)
        {
            switch (value)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "6":
                    break;
                default:
                    break;
            }
        }
        private static async Task<Entity> CategoryOptions(string value)
        {
            switch (value)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "6":
                    break;
                default:
                    break;
            }
        }
        private static async Task<Entity> MovementOptionsAsync(string value)
        {
            UnitOfWork uow = new UnitOfWork();
            switch (value)
            {
                case "1":
                    {
                        Console.WriteLine("Introduce the quantity of the Movement:");
                        string tempString = Console.ReadLine();
                        float quantity = float.Parse(tempString);

                        Console.WriteLine("Introduce the type:");
                        tempString = Console.ReadLine();
                        bool type = bool.Parse(tempString);

                        Console.WriteLine("Introduce the DateTime:");
                        tempString = Console.ReadLine();
                        DateTime time = DateTime.Parse(tempString);

                        Console.WriteLine("Introduce the Category:");
                        tempString = Console.ReadLine();
                        string category = tempString;

                        Movement movement = new Movement()
                        {
                            Quantity = quantity,
                            IsIncome = type,
                            MovementDate = time,
                            Category = category,
                        };

                        uow.MovementRepository.Create(movement);
                    }
                    break;
                case "2":
                    {
                        Console.WriteLine("Introduce the Id of the Movement:");
                        string tempString = Console.ReadLine();
                        int id=int.Parse(tempString);
                        Movement movement = await uow.MovementRepository.FindByIdAsync(id);
                        Console.WriteLine(movement);
                    }
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "6":
                    break;
                default:
                    break;
            }
            uow.SaveAsync();
        }
        private static async Task<Entity> AutomaticMovementOptions(string value)
        {
            switch (value)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                default:
                    break;
            }
        }
        private static void ShowOptions()
        {
            Console.Clear();
            Console.WriteLine(MenuDialog);
        }
        */
    }
}
