using DocumentFormat.OpenXml.Drawing.Charts;
using EconomyManager.UWP__Universal_Windows_.Views.Movements;
using EconomyManager.UWP__Universal_Windows_.Views.Plans;
using EconomyManager.UWP__Universal_Windows_.Views.Wallets;
using EconomyManager.UWP__Universal_Windows_.Views.Users;
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
using EconomyManager.Domain.Models;
using EconomyManager.Domain.Utilities;
using static System.Net.Mime.MediaTypeNames;
using EconomyManager.UWP__Universal_Windows_.Views.Categories;
using System.Threading.Tasks;
using EconomyManager.Domain;
using EconomyManager.Infrastructure;
using System.Collections.ObjectModel;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace EconomyManager.UWP__Universal_Windows_.Views.Wallets
{
    public class Month
    {
        public string Name
        { get; set; }
        public double Sum
        {
            get;
            set;
        }
    }
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Home : Page
    {

        UserViewModel UserViewModel { get; set; }
        WalletViewModel WalletViewModel { get; set; }

        MovementViewModel MovementViewModel { get; set; }
        CategoryViewModel CategoryViewModel { get; set; }
        public Home()
        {
            this.InitializeComponent();
            MovementViewModel = new MovementViewModel();
            CategoryViewModel = new CategoryViewModel();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is List<BindableBase> l)
            {
                WalletViewModel = l[0] as WalletViewModel;
                UserViewModel = l[1] as UserViewModel;
                CategoryViewModel.LoadAllAsync(UserViewModel.LoggedUser.Id);

                GetChartData();
            }
            else
            {
                Frame.GoBack();
            }
        }
        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private string getMonthName(int month)
        {
            switch (month)
            {
                case 1: return "Jan";
                case 2: return "Feb";
                case 3: return "Mar";
                case 4: return "Apr";
                case 5: return "May";
                case 6: return "Jun";
                case 7: return "Jul";
                case 8: return "Aug";
                case 9: return "Sep";
                case 10: return "Oct";
                case 11: return "Nov";
                case 12: return "Dec";
                default: return "";
            }
        }

        private List<int> getMonthsList()
        {
            List<int> months = new List<int>();
            int month = DateTime.Now.Month;
            do
            {
                months.Add(month);
                month = (month == 1) ? 12 : month - 1;
            } while (month != DateTime.Now.Month);
            months.Reverse();
            return months;
        }

        private async void GetChartData()
        {
            await MovementViewModel.LoadAllAsync(WalletViewModel.Wallet);
            List<Month> months = new List<Month>();
            List<Movement> movementsToDisplay = WalletViewModel.Wallet.Movements.FindAll(
                m => m.MovementDate >= new DateTime(DateTime.Now.Year - 1,
                (DateTime.Now.Month == 12 ? 1 : DateTime.Now.Month + 1),
                1)
                && m.MovementDate <= DateTime.Now); //We only display the evolution within the last year
            List<Movement> previousMovements = WalletViewModel.Wallet.Movements.FindAll(
                m => m.MovementDate < new DateTime(
                    DateTime.Now.Year - 1,
                    (DateTime.Now.Month == 12 ? 1 : DateTime.Now.Month + 1),
                    1)
                );
            float startingBalance = MovementUtilities.getTotalOfType(previousMovements, true) - MovementUtilities.getTotalOfType(previousMovements, false);

            List<int> monthsNumbers = getMonthsList();
            int year = DateTime.Now.Year;
            if (monthsNumbers[monthsNumbers.Count - 1] != 12)
                --year;
            foreach (int month in monthsNumbers)
            {
                if (monthsNumbers[monthsNumbers.Count - 1] != 12 && month == 1)
                    ++year;
                float balance = MovementUtilities.getTotalOfMonth(movementsToDisplay, month) + startingBalance;
                months.Add(new Month { Name = $"{getMonthName(month)}/{year % 100:D2}", Sum = Math.Round(balance, 2) });
                startingBalance = balance;
            }
            /*months.Add(new Month { Name = "January", Sum = 150 });
            months.Add(new Month { Name = "February", Sum = 100 });
            months.Add(new Month { Name = "March", Sum = 90 });
            months.Add(new Month { Name = "April", Sum = 180 });
            months.Add(new Month { Name = "May", Sum = 125 });
            months.Add(new Month { Name = "June", Sum = 100 });
            months.Add(new Month { Name = "July", Sum = 179 });
            months.Add(new Month { Name = "August", Sum = 156 });
            months.Add(new Month { Name = "September", Sum = 147 });
            months.Add(new Month { Name = "October", Sum = 132 });
            months.Add(new Month { Name = "November", Sum = 124 });
            months.Add(new Month { Name = "December", Sum = 164 });*/

            (LineChart.Series[0] as ColumnSeries).ItemsSource = months;
        }

        private void BtnToday_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(Outcomes));
        }

        private void BtnWeekly_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(Incomes));
        }

        private void BtnMonthly_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(RegistrationForm));
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ShareWalletFormPage), new List<BindableBase> { UserViewModel, WalletViewModel });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void ReloadScreen()
        {
            int walletId = WalletViewModel.Wallet.Id;
            await WalletViewModel.LoadAllAsync(UserViewModel.LoggedUser);
            WalletViewModel.Wallet = WalletViewModel.Wallets.First(Wallet => Wallet.Id == walletId);
            await MovementViewModel.LoadAllAsync(WalletViewModel.Wallet);
            Frame.Navigate(typeof(Home), new List<BindableBase> { WalletViewModel, UserViewModel });

        }
        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddMovementDialog(UserViewModel, MovementViewModel, CategoryViewModel, WalletViewModel);
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ReloadScreen();
            }
        }

        private async void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {

            if (sender is FrameworkElement fe && fe.DataContext is Movement m)
            {
                MovementViewModel.Movement = m;
                var dialog = new AddMovementDialog(UserViewModel, MovementViewModel, CategoryViewModel, WalletViewModel);
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Frame.GoBack();
                }
            }
        }

        private void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {

            if (sender is FrameworkElement fe && fe.DataContext is Movement m)
            {
                MovementViewModel.Movement = m;
                MovementViewModel.DeleteAsync();
                Frame.GoBack();

            }
        }

        private void Plans_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ListOfPlans), new List<BindableBase> { UserViewModel, WalletViewModel});
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MovementViewModel.Movements.Count > 0)
            {
                MovementViewModel movementViewModel = new MovementViewModel();

                foreach (Movement movement in MovementViewModel.Movements)
                {
                    movementViewModel.Movement = movement;
                    movementViewModel.DeleteAsync();

                }

                await MovementViewModel.LoadAllAsync(WalletViewModel.Wallet);
                Frame.Navigate(typeof(ListOfTheWallets), UserViewModel);
            }
            else
            {
                //IUnitOfWork uow = new UnitOfWork();
                //uow.WalletRepository.Delete(WalletViewModel.Wallet);
                //await uow.SaveAsync();
                WalletViewModel.DeleteAsync();
                Frame.Navigate(typeof(ListOfTheWallets), UserViewModel);

            }

        }
    }
}
