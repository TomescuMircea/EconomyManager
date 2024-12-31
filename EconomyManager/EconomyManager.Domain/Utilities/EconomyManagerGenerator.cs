using EconomyManager.Domain.Models;
using EconomyManager.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EconomyManager.Domain.Utilities
{
    public class EconomyManagerGenerator
    {
        private static readonly Random R = new Random();
        private static readonly int MAX_ITEMS = 20;
        static int RandInt
        {
            get
            {
                return R.Next();
            }
        }
        static float RandFloat
        {
            get
            {
                return Convert.ToSingle(R.NextDouble());
            }
        }

        static bool RandBool
        {
            get
            {
                return (R.Next() % 2 == 0);
            }
        }

        public static List<Movement> generateMovementsForWallet(Wallet w)
        {
            List<Movement> movements = new List<Movement>();
            int itemsNumber = (RandInt % MAX_ITEMS) +1;
            for (int i = 0; i < itemsNumber; ++i)
            {
                Movement mov = new Movement
                {
                    IsIncome = RandBool,
                    MovementDate = DateTime.Now.AddDays((RandInt)%100 * (RandBool ? 1 : -1)),
                    Quantity = RandFloat,
                    Wallet = w,
                    Category = w.UserHasWallets[0].User.Categories[0]
                };
                movements.Add(mov);
                w.Movements.Add(mov);
                mov.Category.Movements.Add(mov);
            }
            return movements;
        }

        public static List<AutomaticMovement> generateAutomaticMovementsForWallet(Wallet w)
        {
            List<AutomaticMovement> autoMovements = new List<AutomaticMovement>();
            int itemsNumber = (RandInt % MAX_ITEMS) + 1;
            for (int i = 0; i < itemsNumber; ++i)
            {
                AutomaticMovement autoMov = new AutomaticMovement
                {
                    IsIncome = RandBool,
                    Frequency = RandInt % 30,
                    Quantity = RandFloat,
                    Wallet = w,
                    Category = w.UserHasWallets[0].User.Categories
                    [0]
                };
                autoMovements.Add(autoMov);
                autoMov.Category.AutomaticMovements.Add(autoMov);
                w.AutomaticMovements.Add(autoMov);
            }
            return autoMovements;
        }

        public static List<Category> generateCategoriesForUser(User u)
        {
            List<Category> categories = new List<Category>();
            int itemsNumber = (RandInt % MAX_ITEMS) + 1;
            for (int i = 0; i < itemsNumber; ++i)
            {
                Category cat = new Category
                {
                    Name = $"{u.Name}'s Category {i+1}",
                    IsIncome = RandBool,
                    User = u
                };
                categories.Add(cat);
                u.Categories.Add(cat);
            }
            return categories;
        }

        public static List<Wallet> generateWalletsForUser(User u)
        {
            List<Wallet> wallets = new List<Wallet>();
            int itemsNumber = (RandInt % MAX_ITEMS) + 1;
            for (int i = 0; i < itemsNumber; ++i)
            {
                Wallet w = new Wallet
                {
                    Description = $"Wallet description -{i+1}",
                    Name = $"{u.Name}'s Wallet ${i+1}",
                    
                };
                UserHasWallet userHasWallet = new UserHasWallet
                {
                    User = u,
                    Wallet = w
                };
                u.UserHasWallets.Add(userHasWallet);
                w.UserHasWallets.Add(userHasWallet);
                wallets.Add(w);

            }
            return wallets;
        }

        public static List<Plan> generatePlansForWallet(Wallet w)
        {
            List<Plan> plans = new List<Plan>();
            int itemsNumber = (RandInt % MAX_ITEMS) + 1;
            for (int i = 0; i < itemsNumber; ++i)
            {
                Plan p = new Plan
                {
                    Deadline = DateTime.Now.AddDays(RandInt%100),
                    Name = $"My Plan Number {i + 1}",
                    Objective = RandFloat,
                    Wallet = w,
                    Quantity = w.Balance()
                };
                w.Plans.Add(p);
                plans.Add(p);
            }
            return plans;
        }

        public static List<Attachment> generateAttachmentsForMovement(Movement m)
        {
            List<Attachment> attachments = new List<Attachment>();
            int itemsNumber = (RandInt % MAX_ITEMS) + 1;
            for (int i = 0; i < itemsNumber; ++i)
            {
                Attachment att = new Attachment
                {
                    Movement = m,
                    Url = $"Not Found"
                };
                m.Attachments.Add(att);
                attachments.Add(att);
            }
            return attachments;
        }

        public static List<User> generateUsers()
        {
            List <User> users = new List<User>();
            int itemsNumber = (RandInt % MAX_ITEMS) + 5;
            for (int i = 0; i < itemsNumber; ++i)
            {
                User u = new User
                {
                    Email = $"user{i+1}@alunos.ipb.pt",
                    Password = $"Password{i+1}" ,
                    Active = true ,
                    Name = $"User {i+1}"
                };
                users.Add(u);
            }
            return users;
        }

        //Multiple lists

        public static List<Movement> generateMovementsForWalletList(List<Wallet> wallets)
        {
            List<Movement> movements = new List<Movement>();
            foreach(Wallet w in wallets)
            {
                foreach(Movement mov in generateMovementsForWallet(w))
                {
                    movements.Add(mov);
                }
            }
            return movements;
        }

        public static List<AutomaticMovement> generateAutomaticMovementsForWalletList(List<Wallet> wallets)
        {
            List<AutomaticMovement> autoMovements = new List<AutomaticMovement>();
            foreach (Wallet w in wallets)
            {
                foreach (AutomaticMovement autoMov in generateAutomaticMovementsForWallet(w))
                {
                    autoMovements.Add(autoMov);
                }
            }
            return autoMovements;
        }

        public static List<Category> generateCategoriesForUserList(List<User> users)
        {
            List<Category> categories = new List<Category>();
            foreach (User u in users)
            {
                foreach (Category cat in generateCategoriesForUser(u))
                {
                    categories.Add(cat);
                }
            }
            return categories;
        }

        public static List<Wallet> generateWalletsForUserList(List<User> users)
        {
            List<Wallet> wallets = new List<Wallet>();
            foreach (User u in users)
            {
                foreach (Wallet wal in generateWalletsForUser(u))
                {
                    wallets.Add(wal);
                }
            }
            return wallets;
        }

        public static List<Plan> generatePlansForWalletList(List<Wallet> wallets)
        {
            List<Plan> plans = new List<Plan>();
            foreach (Wallet wal in wallets)
            {
                foreach (Plan p in generatePlansForWallet(wal))
                {
                    plans.Add(p);
                }
            }
            return plans;
        }

        public static List<Attachment> generateAttachmentsForMovementList(List<Movement> movements)
        {
            List<Attachment> attachments = new List<Attachment>();
            foreach (Movement mov in movements)
            {
                foreach (Attachment att in generateAttachmentsForMovement(mov))
                {
                    attachments.Add(att);
                }
            }
            return attachments;
        }

    }
}