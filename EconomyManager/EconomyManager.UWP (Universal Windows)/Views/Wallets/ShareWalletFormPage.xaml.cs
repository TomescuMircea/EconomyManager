using EconomyManager.Domain;
using EconomyManager.Domain.Models;
using EconomyManager.Infrastructure;
using EconomyManager.UWP__Universal_Windows_.ViewModel;
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

namespace EconomyManager.UWP__Universal_Windows_.Views.Wallets
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShareWalletFormPage : Page
    {
        public UserViewModel UserViewModel { get; set; }
        public WalletViewModel WalletViewModel { get; set; }
        public UserHasWalletViewModel UserHasWalletViewModel { get; set; }
        public ShareWalletFormPage()
        {
            this.InitializeComponent();
            UserHasWalletViewModel = new UserHasWalletViewModel();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Function modified to support the cancel feature
            base.OnNavigatedTo(e);

            if (e.Parameter is List<BindableBase> L)
            {
                UserViewModel = (UserViewModel)L[0];
                WalletViewModel = (WalletViewModel)L[1];
                UserHasWalletViewModel.LoadAllAsync(UserViewModel.LoggedUser.Id);
            }
        }
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            IUnitOfWork uow = new UnitOfWork();

            User tempUser = await uow.UserRepository.FindByEmailAsync(UserViewModel.UserEmail);

            if (tempUser != null)
            {
                UserHasWalletViewModel.LoadAllAsync(tempUser.Id);

                if (await UserHasWalletViewModel.CreateOrUpdateUserWasWalletAsync(tempUser, WalletViewModel.Wallet))
                {
                    UserHasWalletViewModel.LoadAllAsync(UserViewModel.LoggedUser.Id);
                    Frame.Navigate(typeof(ListOfTheWallets), new List<BindableBase> { UserViewModel, WalletViewModel });
                }
                else
                {
                    FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
                }
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
