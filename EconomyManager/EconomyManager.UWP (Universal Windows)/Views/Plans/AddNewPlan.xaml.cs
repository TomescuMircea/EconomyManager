using EconomyManager.UWP__Universal_Windows_.ViewModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EconomyManager.UWP__Universal_Windows_.Views.Plans
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddNewPlan : Page
    {
        public PlanViewModel PlanViewModel { get; set; }

        public UserViewModel UserViewModel { get; set; }
        public WalletViewModel WalletViewModel { get; set; }

        public DateTimeOffset Deadline {  get; set; }

        public AddNewPlan()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is List<BindableBase> l)
            {
                UserViewModel = (UserViewModel)l[0];
                WalletViewModel = (WalletViewModel)l[1];
                PlanViewModel = (PlanViewModel)l[2];
                Deadline = (PlanViewModel.Plan.Deadline == null) ? 
                    Deadline = new DateTimeOffset(DateTime.Now) : 
                    PlanViewModel.Plan.Deadline;
            }
            base.OnNavigatedTo(e);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            PlanViewModel.Plan.Deadline = new DateTime(Deadline.Date.Ticks);
            var updatedOrCreatedPlan = await PlanViewModel.CreateOrUpdatePlanAsync(WalletViewModel.Wallet.Id);
            if (updatedOrCreatedPlan != null)
            {
                WalletViewModel.Wallet.Plans.Add(updatedOrCreatedPlan);
                Frame.Navigate(typeof(ListOfPlans), new List<BindableBase> {UserViewModel, WalletViewModel, PlanViewModel });
            }
            else
            {
                FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
