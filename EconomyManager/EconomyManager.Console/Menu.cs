using EconomyManager.Domain.Models;
using EconomyManager.Domain.SeedWork;
using EconomyManager.Domain.Utilities;
using EconomyManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager
{
    public class Menu
    {
        private const int INITIAL_MENU_INDEX = 0;
        private const int USER_MENU_INDEX = 1;
        private const int CATEGORY_MENU_INDEX = 2;
        private const int WALLET_MENU_INDEX = 3;
        private const int AUTOMATATION_MENU_INDEX = 4;
        private static string[] menus = 
            {
                //0
                "1.Login\n" +
                "2.Register\n" +
                "3.End Program",
                
                //1
                "1.Wallets\n" +
                "2.Categories\n" +
                "3.Settings\n" +
                "4.Exit",
                
                //2
                "1.Add Categories\n" +
                "2.Modify Category\n" +
                "3.Delete Category\n" +
                "4.Exit" ,
                
                //3
                "1.See Movements\n" +
                "2.Add Movement\n" +
                "3.Outcome/Income ratio\n" +
                "4.Outcome category/total ratio\n" +
                "5.Automation Tools\n" +
                "6.Add user\n" +
                "7.Exit",
                
                //4
                "1.Add new Automatic Movement\n" +
                "2.Activate an Automatic Movement\n" +
                "3.Deactivate an Automatic Movement\n" +
                "4.Run Automatation\n"+
                "5.Exit"
            };


        int _currentUserId;
        int _currentWalletId;
        int _currentMenuIndex;
        UnitOfWork _unitOfWork;

        public bool Error { get; private set; } 

        private bool End { get; set; }


        public string Options {  get { return Menu.menus[this._currentMenuIndex]; } }

        public void Run()
        {
            do
            {
                if (this.Error)
                    Console.WriteLine("Not a valid option");
                Console.WriteLine(Options);
                int option;
                while(!Int32.TryParse(Console.ReadLine(),out option))
                    Console.Write("Please, enter a number: ");
                this.ProcessOption(option);
            } while (this.Error||!this.End);
        }

        public Menu()
        {
            this._currentUserId = -1;
            this._currentWalletId = -1;
            this._currentMenuIndex = Menu.INITIAL_MENU_INDEX;
            this._unitOfWork = new UnitOfWork();
            End = false;
        }

        public void ProcessOption(int option)
        {
            this.Error = false; //We reset error to false. If the option given is incorrect, it will be reseted to true
            switch (this._currentMenuIndex)
            {
                case Menu.INITIAL_MENU_INDEX:
                    this.ProcessInitial(option);
                    break;
                case Menu.USER_MENU_INDEX :
                    this.ProcessUser(option);
                    break;
                case Menu.WALLET_MENU_INDEX:
                    this.ProcessWallet(option);
                    break;
                case Menu.CATEGORY_MENU_INDEX:
                    this.ProcessCategory(option);
                    break;
                case Menu.AUTOMATATION_MENU_INDEX:
                    this.ProcessAutomation(option);
                    break;
                default:
                    break;
            }
            if (!this.Error)
            {
                Console.WriteLine("Saving...");
                // here should not be put an await?
                _unitOfWork.SaveAsync();
            }
            Thread.Sleep(2000);
            Console.Clear();
        }

        private async void ProcessInitial(int option)
        {
            switch (option)
            {
                case 1: //Login
                    {
                        string email, password;
                        Console.Write("Email: ");
                        email = Console.ReadLine();
                        Console.Write("Password: ");
                        password = Menu.GetHiddenConsoleInput();
                        User u = await _unitOfWork.UserRepository.FindByEmailAsync(email);
                        if (u.Password.Equals(User.EncodeString(password)))
                        {
                            this._currentMenuIndex = Menu.USER_MENU_INDEX;
                            this._currentUserId = u.Id;
                            Console.WriteLine($"Welcome back, {u.Name}!");
                        }
                        else
                        {
                            Console.WriteLine("Email or password incorrect");
                        }
                    }
                    break;
                case 2: //Register
                    {
                        string name, email, password, repeatPassword;

                        Console.Write("Full Name: ");
                        name = Console.ReadLine();
                        Console.Write("Email: ");
                        email = Console.ReadLine();
                        Console.Write("Password: ");
                        password = Menu.GetHiddenConsoleInput();
                        Console.Write("Repeat Password: ");
                        repeatPassword = Menu.GetHiddenConsoleInput();
                        if (password.Equals(repeatPassword))
                        {
                            User u = await _unitOfWork.UserRepository.Register(name, email, password);
                            if (u != null)
                            {
                                u.Active = true;
                                _unitOfWork.UserRepository.Update(u);
                                Console.WriteLine($"User {u.Email} registered sucessfully");
                            }
                            else
                            {
                                Console.WriteLine("Email alredy registered");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Passwords don't match");
                        }
                    }
                    break;
                case 3: //End program
                    this.Exit();
                    break;
                default:
                    { this.Error = true; }
                    break;
            }
        }

        private async void ProcessUser(int option)
        {
            switch (option)
            {
                case 1: //Wallets
                    var wallets = await _unitOfWork.WalletRepository.FindAllByUserIdAsync(this._currentUserId);
                    Console.WriteLine("-1- Back");
                    Console.WriteLine("0- Create a new Wallet");
                    for (int i = 0; i < wallets.Count; ++i)
                        Console.WriteLine($"{i+1}- {wallets[i].Name}");
                    int selectedIndex = Int32.Parse(Console.ReadLine());
                    if (selectedIndex == -1)//Create a Wallet
                    {
                        break;
                    }
                    else if(selectedIndex == 0)//Create a Wallet
                    {
                        await createWalletAsync();
                    }
                    else if(selectedIndex > 0 || selectedIndex <= wallets.Count)
                    {
                        this._currentMenuIndex = Menu.WALLET_MENU_INDEX;
                        this._currentWalletId = wallets[selectedIndex-1].Id;
                    } else
                    {
                        Console.WriteLine("No such wallet available");
                    }
                    break;
                case 2: //Category
                    this._currentMenuIndex = Menu.CATEGORY_MENU_INDEX;
                    break;
                case 3: //Settings
                    User u = await _unitOfWork.UserRepository.FindByIdAsync(this._currentUserId);
                    if (u != null)
                    {
                        bool save = false;
                        do
                        {
                            Console.Write($"Name: {u.Name}\n" +
                                $"Email: {u.Email}\n\n" +
                                $"1- Change name\n" +
                                $"2- Change email\n" +
                                $"3- Change password\n" +
                                $"4- Deactivate account\n" +
                                $"5- Save Changes\n" +
                                $"Choose an option: ");

                            int opt;
                            while (!Int32.TryParse(Console.ReadLine(), out opt))
                                Console.Write("Enter a number: ");

                            switch (opt)
                            {
                                case 1:
                                    {
                                        Console.Write("Enter new name: ");
                                        u.Name = Console.ReadLine();
                                    }
                                    break;
                                case 2:
                                    {
                                        Console.Write("Enter new email: ");
                                        u.Name = Console.ReadLine();
                                    }
                                    break;
                                case 3:
                                    {
                                        string old_password,new_password,repeat_password;
                                        Console.Write("Enter old password: ");
                                        old_password = Menu.GetHiddenConsoleInput();
                                        Console.Write("Enter new password: ");
                                        new_password = Menu.GetHiddenConsoleInput();
                                        Console.Write("Repeat new password: ");
                                        repeat_password = Menu.GetHiddenConsoleInput();
                                        if (u.Password.Equals(User.EncodeString(old_password)))
                                        {
                                            if (new_password.Equals(repeat_password))
                                            {
                                                u.Password = new_password;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Passwords don't match");
                                            }
                                        } else
                                        {
                                            Console.WriteLine("Old password incorrect");
                                        }
                                    }
                                    break;
                                case 4:
                                    {
                                        u.Active = false;
                                    }
                                    break;
                                case 5:
                                    {
                                        save = true;
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Not a valid option\n");
                                    break;
                            }
                        } while (!save);
                        _unitOfWork.UserRepository.Update(u);
                    }
                    break;
                case 4: //Exit
                    this.Exit();
                    break;
                default:
                    break;
            }
        }

        private async Task createWalletAsync()
        {
            string name, description;
            Console.Write("Name of the wallet: ");
            name = Console.ReadLine();
            Console.Write("Description of the wallet: ");
            description = Console.ReadLine();
            Wallet wallet = new Wallet
            {
                Name = name,
                Description = description
            };
            this._unitOfWork.WalletRepository.Create(wallet);
            UserHasWallet userHasWallet = new UserHasWallet
            {
                UserId = this._currentUserId,
                Wallet = wallet,
            };
            wallet.UserHasWallets.Add(userHasWallet);
            await this._unitOfWork.SaveAsync();
        }

        private async void ProcessWallet(int option)
        {
            switch (option)
            {
                case 1: //See Movements
                    {
                        List<Movement> movements = await _unitOfWork.MovementRepository.FindAllByWalletIdAsync(this._currentWalletId);
                        foreach (var mov in movements)
                        {
                            Console.WriteLine($"{mov.MovementDate}- {mov.Category.Name}......{mov.Quantity}");
                        }
                    }
                    break;
                case 2: // Add Movements
                    {
                        List<Category> categories = await _unitOfWork.CategoryRepository.CategoriesFromWalletId(this._currentWalletId);
                        int selectedIndex;
                        float quantity;

                        Console.Write("Enter the quantity of the movement " +
                            "(negative quantities will be automatically set as outcomes): ");
                        while (!float.TryParse(Console.ReadLine(), out quantity))
                            Console.Write("Enter a valid quantity: ");
                        var mov = new Movement
                        {
                            IsIncome = quantity > 0,
                            Quantity = float.Abs(quantity),
                            WalletId = this._currentWalletId,
                            MovementDate = DateTime.Now
                        };
                        if (categories.Any())
                        {
                            for (int i = 0; i < categories.Count(); ++i)
                            {
                                Console.WriteLine($"{i + 1}- {categories[i].Name}");
                            }
                            Console.Write("Select a category: ");
                            while (!Int32.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > categories.Count())
                                Console.Write("Enter a valid category: ");
                            mov.Category = categories[selectedIndex-1];
                        }
                        _unitOfWork.MovementRepository.Create(mov);
                    }
                    break;
                case 3: // Outcome/Income Ratio
                    {
                        var movements = await _unitOfWork.MovementRepository.FindAllByWalletIdAsync(this._currentWalletId);
                        Console.WriteLine($"You have {MovementUtilities.getTotalOfType(movements, Movement.INCOME)} as incomes" +
                            $"and {MovementUtilities.getTotalOfType(movements, Movement.OUTCOME)} as outcomes; this means your " +
                            $"outcomes represent a {MovementUtilities.getRatioOfType(movements, Movement.OUTCOME) *100}% of your incomes");
                        MovementUtilities.getRatioOfType(movements, Movement.OUTCOME);
                    }
                    break;
                case 4: // Outcome Category/Total Ratio
                    {
                        var movements = (await _unitOfWork.MovementRepository.FindAllByWalletIdAsync(this._currentWalletId))
                            .FindAll(x => x.IsIncome == Movement.OUTCOME);
                        var categories = (await _unitOfWork.CategoryRepository.CategoriesFromWalletId(this._currentWalletId))
                            .FindAll(x => x.IsIncome == Movement.OUTCOME);

                        if (categories.Any())
                        {
                            int selectedIndex;
                            Console.WriteLine("0-Back");
                            for(int i = 0; i< categories.Count;++i)
                            {
                                Console.Write($"{i+1} - {categories[i].Name}");
                            }
                            Console.Write("Choose a category: ");
                            while(!Int32.TryParse(Console.ReadLine(),out selectedIndex)
                                || selectedIndex < 0 || selectedIndex > categories.Count)
                                Console.Write("Enter a valid number: ");

                            if(selectedIndex > 0)
                                Console.WriteLine($"You have {MovementUtilities.getTotalOfType(movements, Movement.INCOME)} as outcomes" +
                                    $"which {MovementUtilities.getTotalOfCategory(movements, categories[selectedIndex-1])} are from {categories[selectedIndex-1].Name}; " +
                                    $"this means that {categories[selectedIndex - 1].Name} represents a {MovementUtilities.getRatioOfCategory(movements, categories[selectedIndex-1])} " +
                                    $"of your outcomes" +
                                    $"outcomes represent a {MovementUtilities.getRatioOfType(movements, Movement.OUTCOME) * 100}% of your incomes");
                        } else
                        {
                            Console.WriteLine("No categories to select");
                        }
                    }
                    break;
                case 5: // Automation Tools
                    this._currentMenuIndex = Menu.AUTOMATATION_MENU_INDEX;
                    break;
                case 6: // Add User
                    {
                        string email;
                        Console.Write("Enter user's email: ");
                        email = Console.ReadLine();
                        User u = await _unitOfWork.UserRepository.FindByEmailAsync(email);
                        if (u != null)
                        {
                            Wallet w = await _unitOfWork.WalletRepository.FindByIdAsync(this._currentWalletId);
                            w.UserHasWallets.Add(new UserHasWallet
                            {
                                WalletId = this._currentWalletId,
                                UserId = u.Id,
                            });
                        }
                        else
                        {
                            Console.WriteLine($"Couldn't find user with email \'{email}\'");
                        }
                    }

                    break;
                case 7: //Exit
                    this.Exit();
                    break;
                default:
                    break;

            }
        }

        private async void ProcessCategory(int option)
        {
            List<Category> categories = await _unitOfWork.CategoryRepository.CategoriesFromUserId(this._currentUserId);
            switch (option)
            {
                case 1: // Add Category
                    string name;
                    char isIncome;
                    Console.Write("Enter the name of the new category: ");
                    name = Console.ReadLine();
                    Console.Write("Is the category available for incomes?[y/n] ");
                    isIncome = Console.ReadLine().ElementAt(0);
                    Category cat = new Category
                    {
                        Name = name,
                        IsIncome = isIncome == 'y',
                        UserId = this._currentUserId,
                    };
                    _unitOfWork.CategoryRepository.Create(cat);
                    break;
                case 2: // Modify Category
                    {
                        for (int i = 0; i < categories.Count(); ++i)
                        {
                            Console.WriteLine($"{i + 1} - {categories[i].Name}");
                        }
                        Console.Write("Choose a category: ");
                        Console.Write("0-Back");
                        int selectedIndex;
                        while (!Int32.TryParse(Console.ReadLine(), out selectedIndex) ||
                            selectedIndex < 0 || selectedIndex > categories.Count())
                            Console.Write("Select a valid category: ");
                        if(selectedIndex >0)
                            this.EditCategory(categories[selectedIndex - 1].Id);
                    }
                    break;
                case 3: // Delete Category
                    {
                        for (int i = 0; i < categories.Count(); ++i)
                        {
                            Console.WriteLine($"{i + 1} - {categories[i].Name}");
                        }
                        Console.Write("Choose a category: ");
                        int selectedIndex;
                        while (!Int32.TryParse(Console.ReadLine(), out selectedIndex) ||
                            selectedIndex < 1 || selectedIndex > categories.Count())
                            Console.Write("Select a valid category: ");
                        _unitOfWork.CategoryRepository.Delete(categories[selectedIndex-1]);
                    }
                    break;
                case 4: // Exit
                    this.Exit();
                    break;
                default:
                    break;

            }
        }

        private async void ProcessAutomation(int option)
        {
            List<AutomaticMovement> automaticMovements = await _unitOfWork.AutomaticMovementRepository.FindAllByWalletIdAsync(this._currentWalletId);
            switch (option)
            {
                case 1: //Add new Automatic Movement
                    {
                        List<Category> categories = await _unitOfWork.CategoryRepository.CategoriesFromWalletId(this._currentWalletId);
                        int selectedIndex, frequency;
                        float quantity;
                        Console.Write("Enter the frequency of this movement (on days): ");
                        while (!Int32.TryParse(Console.ReadLine(), out frequency) || frequency < 1)
                            Console.Write("Enter a valid frequency: ");
                        
                        Console.Write("Enter the quantity of the movement " +
                            "(negative quantities will be automatically set as outcomes): ");
                        while (!float.TryParse(Console.ReadLine(), out quantity))
                            Console.Write("Enter a valid quantity: ");

                        var autoMov = new AutomaticMovement
                        {
                            Frequency = frequency,
                            IsIncome = quantity > 0,
                            Quantity = float.Abs(quantity),
                            WalletId = this._currentWalletId,
                            Active = true,
                        };
                        if (categories.Any())
                        {
                            for (int i = 0; i< categories.Count(); ++i)
                            {
                                Console.WriteLine($"{i+1}- {categories[i].Name}");
                            }
                            Console.Write("Select a category: ");
                            while (!Int32.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > categories.Count())
                                Console.Write("Enter a valid category: ");
                            autoMov.Category = categories[selectedIndex-1];
                        }
                        _unitOfWork.AutomaticMovementRepository.Create(autoMov);
                        automaticMovements.Add(autoMov);
                    }
                    break;
                case 2: // Activate an Automatic Movement
                    {
                        List<AutomaticMovement> deactivatedMovements = automaticMovements.FindAll(x => !x.Active);
                        if (deactivatedMovements.Any())
                        {
                            for (int i = 0; i < deactivatedMovements.Count(); ++i)
                            {
                                Console.WriteLine($"{i + 1}- [{deactivatedMovements}]");
                            }
                            Console.Write("Which active movement do you want to deactivate? ");
                            int selectedIndex;
                            while (Int32.TryParse(Console.ReadLine(), out selectedIndex))
                                Console.Write("Input a number");
                            if (selectedIndex > 0 && selectedIndex <= deactivatedMovements.Count())
                                deactivatedMovements[selectedIndex - 1].Active = true;
                            else
                                Console.WriteLine("No such Automatic Movement exists");
                        }
                        else
                        {
                            Console.WriteLine("No deactivated movements on this wallet");
                        }
                    }
                    break;
                case 3: // Deactivate an Automatic Movement
                    {
                        List<AutomaticMovement> activeMovements = automaticMovements.FindAll(x => x.Active);
                        if (activeMovements.Any())
                        {
                            for (int i = 0; i < activeMovements.Count();++i)
                            {
                                Console.WriteLine($"{i+1}- [{activeMovements}]");
                            }
                            Console.Write("Which active movement do you want to deactivate? ");
                            int selectedIndex;
                            while (Int32.TryParse(Console.ReadLine(), out selectedIndex))
                                Console.Write("Input a number");
                            if (selectedIndex > 0 && selectedIndex <= activeMovements.Count())
                                activeMovements[selectedIndex - 1].Active = false;
                            else
                                Console.WriteLine("No such Automatic Movement exists");

                        } else
                        {
                            Console.WriteLine("No active movements on this wallet");
                        }
                    }
                    break;
                case 4: // Run Automatation
                    {
                        List<Movement> movements = new List<Movement>();
                        foreach (AutomaticMovement autoMov in automaticMovements)
                        {
                            movements.Add(new Movement
                            {
                                MovementDate = DateTime.Now,
                                Quantity = autoMov.Quantity,
                                CategoryId = autoMov.CategoryId,
                                WalletId = this._currentWalletId,
                                IsIncome = autoMov.IsIncome
                                
                            });
                        }
                        _unitOfWork.MovementRepository.CreateAll(movements);
                    }
                    break;
                case 5: // Exit
                    this.Exit();
                    break;
                default:
                    break;
            }
        }

        private void Exit()
        {
            switch (this._currentMenuIndex)
            {
                case Menu.INITIAL_MENU_INDEX:
                    this._currentMenuIndex = Menu.INITIAL_MENU_INDEX;
                    this.End = true;
                    break;
                case Menu.USER_MENU_INDEX:
                    this._currentMenuIndex = Menu.INITIAL_MENU_INDEX;
                    this._currentUserId = -1;
                    break;
                case Menu.CATEGORY_MENU_INDEX:
                    this._currentMenuIndex = Menu.USER_MENU_INDEX;
                    break;
                case Menu.WALLET_MENU_INDEX:
                    this._currentMenuIndex = Menu.USER_MENU_INDEX;
                    this._currentWalletId = -1;
                    break;
                case Menu.AUTOMATATION_MENU_INDEX:
                    this._currentMenuIndex = Menu.WALLET_MENU_INDEX;
                    break;
                default:
                    this._currentMenuIndex = Menu.INITIAL_MENU_INDEX;
                    break;
            }
        }

        private async void EditCategory(int categoryId)
        {
            Category c = await _unitOfWork.CategoryRepository.FindByIdAsync(categoryId);
            if (c != null)
            {
                bool save = false;
                do
                {
                    Console.Write($"Name: {c.Name}\n" +
                        $"Available for: {c.Type}\n\n" +
                        $"1- Change name\n" +
                        $"2- Switch Income/Outcome type\n" +
                        $"3- Save Changes\n" +
                        $"Choose an option: ");

                    int opt;
                    while (!Int32.TryParse(Console.ReadLine(), out opt))
                        Console.Write("Enter a number: ");

                    switch (opt)
                    {
                        case 1:
                            {
                                Console.Write("Enter new name: ");
                                c.Name = Console.ReadLine();
                            }
                            break;
                        case 2:
                            {
                                c.IsIncome = !c.IsIncome;
                            }
                            break;
                        case 3:
                            save = true;
                            break;
                        default:
                            Console.WriteLine("Not a valid option\n");
                            break;
                    }
                } while (!save);
                _unitOfWork.CategoryRepository.Update(c);
            }
        }

        private static string GetHiddenConsoleInput()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
            }
            Console.Write('\n');
            return input.ToString();
        }

    }
}
