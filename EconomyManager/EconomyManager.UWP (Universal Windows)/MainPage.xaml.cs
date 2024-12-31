using EconomyManager.UWP__Universal_Windows_.Views.Movements;
using EconomyManager.UWP__Universal_Windows_.Views.Plans;
using EconomyManager.UWP__Universal_Windows_.Views.Wallets;
using EconomyManager.UWP__Universal_Windows_.Views.Users;
using EconomyManager.UWP__Universal_Windows_.Views.Categories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using EconomyManager.UWP__Universal_Windows_.ViewModel;
using EconomyManager.Infrastructure;
using EconomyManager.Domain.Utilities;
using System.Threading.Tasks;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EconomyManager.UWP__Universal_Windows_
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public WalletViewModel WalletViewModel { get; set; }
        public UserViewModel UserViewModel { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            WalletViewModel = new WalletViewModel();
            //PopulateDatabaseAsync();
        }

        private async Task PopulateDatabaseAsync()
        {
            /**************************Create Data**************************************/
            //RUN ONLY ONCE IF THE DATABASE IS EMPTY
            using (var uow = new UnitOfWork())
            {
                var users = EconomyManagerGenerator.generateUsers();
                var categories = EconomyManagerGenerator.generateCategoriesForUserList(users);
                var wallets = EconomyManagerGenerator.generateWalletsForUserList(users);
                var movements = EconomyManagerGenerator.generateMovementsForWalletList(wallets);
                var plans = EconomyManagerGenerator.generatePlansForWalletList(wallets);
                var attachments = EconomyManagerGenerator.generateAttachmentsForMovementList(movements);
                var autoMovements = EconomyManagerGenerator.generateAutomaticMovementsForWalletList(wallets);


                foreach (var user in users)
                {
                    Console.WriteLine(user);
                    uow.UserRepository.Create(user);
                }
                foreach (var wallet in wallets)
                {
                    Console.WriteLine(wallet);
                    uow.WalletRepository.Create(wallet);
                }
                foreach (var movement in movements)
                {
                    Console.WriteLine(movement);
                }
                foreach (var autoMov in autoMovements)
                {
                    Console.WriteLine(autoMov);
                }
                foreach (var category in categories)
                {
                    Console.WriteLine(category);
                }
                foreach (var plan in plans)
                {
                    Console.WriteLine(plan);
                }
                foreach (var attachmemt in attachments)
                {
                    Console.WriteLine(attachmemt);
                }

                await uow.SaveAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var userViewModel = e.Parameter as UserViewModel;
            if (userViewModel == null)
            {
                appFrame.Navigate(typeof(RegistrationForm));
            }
            else
            {
                UserViewModel = userViewModel;
            }
        }




        private void mainNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;

            if (selectedItem != null)
            {
                switch (selectedItem.Tag)
                {
                    case "wallets":
                        mainFrame.Navigate(typeof(ListOfTheWallets), UserViewModel);
                        break;
                    case "categories":
                        mainFrame.Navigate(typeof(ManageCategoriesPage), UserViewModel);
                        break;
                    case "settings":
                        mainFrame.Navigate(typeof(Settings), UserViewModel);
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnLogout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UserViewModel.DoLogout();
            Frame.Navigate(typeof(RegistrationForm), UserViewModel);
        }
    }
}
