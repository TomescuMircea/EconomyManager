using DocumentFormat.OpenXml.Drawing.Charts;
using EconomyManager.Domain.Models;
using EconomyManager.UWP__Universal_Windows_.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class ListOfTheWallets : Page
    {
        WalletViewModel WalletViewModel { get; set; }
        UserViewModel UserViewModel { get; set; }


        public ListOfTheWallets()
        {
            this.InitializeComponent();
            WalletViewModel = new WalletViewModel();
            UserViewModel = new UserViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is UserViewModel userViewModel)
            {
                UserViewModel = userViewModel;
                WalletViewModel.LoadAllAsync(UserViewModel.LoggedUser);
            }
            else if (e.Parameter is List<BindableBase> L)
            {
                UserViewModel = (UserViewModel)L[0];
                WalletViewModel = (WalletViewModel)L[1];
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WalletFormPage), new List<BindableBase> { UserViewModel, WalletViewModel });
        }

        private void mainListView_ItemClick(object sender, ItemClickEventArgs e)
            {
            if(sender is FrameworkElement fe && fe.DataContext is Wallet w)
                {
                WalletViewModel.Wallet = w;
                Frame.Navigate(typeof(Home),new {WalletViewModel,UserViewModel});
            }
        }

        private void mainListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Wallet w)
            {
                WalletViewModel.Wallet = w;
                Frame.Navigate(typeof(Home), new List<BindableBase>{ WalletViewModel, UserViewModel });
            }
        }
    }
}
