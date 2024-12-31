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

namespace EconomyManager.UWP__Universal_Windows_.Views.Categories
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryFormPage : Page
    {
        public CategoryViewModel CategoryViewModel { get; set; }
        public UserViewModel UserViewModel { get; set; }
        public CategoryFormPage()
        {
            this.InitializeComponent();

            CategoryViewModel = new CategoryViewModel();
            UserViewModel = new UserViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Function modified to support the cancel feature
            base.OnNavigatedTo(e);

            if (e.Parameter is List<BindableBase> L)
            {
                UserViewModel = (UserViewModel)L[0];
                CategoryViewModel = (CategoryViewModel)L[1];

                CategoryViewModel.TypeOfCategory = CategoryViewModel.Invalid ? 0 : (CategoryViewModel.Category.IsIncome) ? 1 : 2;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // To show the error in case there is no option selected
            if (CategoryViewModel.ValidType)
            {
                if (await CategoryViewModel.CreateOrUpdateCategoryAsync(UserViewModel.LoggedUser))
                {
                    Frame.Navigate(typeof(ManageCategoriesPage), new List<BindableBase> { CategoryViewModel, UserViewModel });
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

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            CategoryViewModel.TypeOfCategory = 1;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            CategoryViewModel.TypeOfCategory = 2;
        }

        // Members to check the radiobuttons for the value of a object retrieved from the database
        public bool IsIncomeChecked => CategoryViewModel.TypeOfCategory == 1;
        public bool IsOutcomeChecked => CategoryViewModel.TypeOfCategory == 2;

    }
}
