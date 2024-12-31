using EconomyManager.Domain.Models;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace EconomyManager.UWP__Universal_Windows_.Views.Users
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {

        public UserViewModel UserViewModel { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set; }
        public Settings()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is UserViewModel userViewModel)
            {
                UserViewModel = userViewModel;
                UserViewModel.User = new User
                {
                    Name = UserViewModel.LoggedUser.Name,
                    Email = UserViewModel.LoggedUser.Email,
                };
            }
            else
            {
                Frame.GoBack();
            }
        }

        private void BtnSaveUserData_Click(object sender, RoutedEventArgs e)
        {
            UserViewModel.UpdateUserDataAsync();
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if(User.EncodeString(OldPassword).Equals(UserViewModel.LoggedUser.Password) &&
                NewPassword.Equals(RepeatPassword))
            {
                UserViewModel.UpdateUserPasswordAsync(NewPassword);
            }
            else
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }
    }
}
