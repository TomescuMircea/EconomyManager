using EconomyManager.Domain.Models;
using EconomyManager.Domain.Utilities;
using EconomyManager.UWP__Universal_Windows_.ViewModel;
using EconomyManager.UWP__Universal_Windows_.Views.Wallets;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace EconomyManager.UWP__Universal_Windows_.Views.Plans
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ListOfPlans : Page
    {

        public PlanViewModel PlanViewModel { get; set; }
        public WalletViewModel WalletViewModel { get; set; }

        public UserViewModel UserViewModel { get; set; }
        public ListOfPlans()
        {
            this.InitializeComponent();
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Home), new List<BindableBase> { WalletViewModel, UserViewModel });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is List<BindableBase> l)
            {
                UserViewModel = (UserViewModel)l[0];
                WalletViewModel = (WalletViewModel)l[1];
                PlanViewModel = (l.Count==3) ? (PlanViewModel)l[2] : new PlanViewModel();
                PlanViewModel.LoadAllAsync(WalletViewModel.Wallet);
                GetGraphData();
            } else
            {
                Frame.GoBack();
            }
        }

        private void GetGraphData()
        {
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
            (LineChart.Series[0] as LineSeries).ItemsSource = months;
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

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddNewPlan), new List<BindableBase> { UserViewModel,WalletViewModel,PlanViewModel});
        }

        private void planListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is ListView le && le.SelectedItem is Plan p)
            {
                GetGraphLine(p.Objective);
            }
        }

        public void GetGraphLine(float threshold)
        {
            List<Month> months = new List<Month>();
            List<int> monthsNumbers = getMonthsList();
            int year = DateTime.Now.Year;
            if (monthsNumbers[monthsNumbers.Count - 1] != 12)
                --year;
            foreach (int month in monthsNumbers)
            {
                if (monthsNumbers[monthsNumbers.Count - 1] != 12 && month == 1)
                    ++year;
                months.Add(new Month { Name = $"{getMonthName(month)}/{year % 100:D2}", Sum = Math.Round(threshold, 2) });
            }
            (LineChart.Series[1] as LineSeries).ItemsSource = months;
        }
    }
}
