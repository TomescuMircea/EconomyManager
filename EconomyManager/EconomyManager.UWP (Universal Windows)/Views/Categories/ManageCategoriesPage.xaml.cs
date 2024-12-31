using EconomyManager.UWP__Universal_Windows_.ViewModel;
using EconomyManager.Domain.Models;
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

namespace EconomyManager.UWP__Universal_Windows_.Views.Categories
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManageCategoriesPage : Page
    {
        public CategoryViewModel CategoryViewModel { get; set; }
        public UserViewModel UserViewModel { get; set; }
        public ManageCategoriesPage()
        {
            this.InitializeComponent();
            CategoryViewModel = new CategoryViewModel();
            UserViewModel = new UserViewModel();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if ( e.Parameter is UserViewModel userViewModel)
            {
                UserViewModel = userViewModel;
                CategoryViewModel.LoadAllAsync(UserViewModel.LoggedUser.Id);
            }
            else if(e.Parameter is List<BindableBase> L)
            {
                CategoryViewModel= (CategoryViewModel)L[0];
                UserViewModel = (UserViewModel)L[1];
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var deleteDialog = new ContentDialog
            {
                Title = "Delete Category",
                Content = "Are you sure you want to delete?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            var result = await deleteDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (sender is FrameworkElement fe && fe.DataContext is Category c)
                {
                    if (!c.Movements.Any())
                    {
                        CategoryViewModel.Category = c;
                        CategoryViewModel.DeleteAsync();
                    }
                    else
                    {
                        FlyoutBase.ShowAttachedFlyout(fe);
                    }
                }

            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is Category c)
            {
                CategoryViewModel.Category = c;
                Frame.Navigate(typeof(CategoryFormPage), new List<BindableBase>{ UserViewModel,CategoryViewModel});
            }
        }

        private void gridCategories_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Fix category textbox filled after inserting a new category
            CategoryViewModel.CategoryName = string.Empty;
            Frame.Navigate(typeof(CategoryFormPage), new List<BindableBase> { UserViewModel, CategoryViewModel });
        }
    }
}
