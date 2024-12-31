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

namespace EconomyManager.UWP__Universal_Windows_.Views.Users
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistrationForm : Page
    {

        UserViewModel UserViewModelLogin { get; set; }
        UserViewModel UserViewModelRegistration { get; set; }

        public bool ErrorLogin { get; set; }
        public bool ErrorRegister { get; set; }


        public RegistrationForm()
        {
            this.InitializeComponent();
            UserViewModelLogin = new UserViewModel();
            UserViewModelLogin.User.Email = "user1@alunos.ipb.pt";
            UserViewModelLogin.User.Password = "Password1";
            UserViewModelRegistration = new UserViewModel();
            ErrorLogin = false;
            ErrorRegister = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var userViewModel = e.Parameter as UserViewModel;
            if (userViewModel != null) 
            { 
                UserViewModelLogin = userViewModel;
                UserViewModelLogin.DoLogout();
            }
            base.OnNavigatedTo(e);
        }

        private async void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            ErrorRegister = false; //This is to see visually that a new operation is taking place
            ErrorRegister = !await UserViewModelRegistration.DoRegisterAsync();
            if(!ErrorRegister)
            {
                Frame.Navigate(typeof(MainPage), UserViewModelRegistration);
            }
            else
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            ErrorLogin = false; //This is to see visually that a new operation is taking place
            ErrorLogin = !await UserViewModelLogin.DoLoginAsync();
            if (!ErrorLogin)
            {
                Frame.Navigate(typeof(MainPage),UserViewModelLogin);
            } else
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }
    }
}
