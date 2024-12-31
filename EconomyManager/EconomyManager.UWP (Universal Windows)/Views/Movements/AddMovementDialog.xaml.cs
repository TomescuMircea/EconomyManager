using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using EconomyManager.Domain;
using EconomyManager.Domain.Models;
using EconomyManager.Infrastructure;
using EconomyManager.UWP__Universal_Windows_.ViewModel;
using EconomyManager.UWP__Universal_Windows_.Views.Wallets;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using Category = EconomyManager.Domain.Models.Category;
// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EconomyManager.UWP__Universal_Windows_.Views.Movements
{
    public sealed partial class AddMovementDialog : ContentDialog
    {
        public ComboBoxItem ComboBoxItemSelected { get; set; }
        public UserViewModel UserViewModel { get; set; }
        public MovementViewModel MovementViewModel { get; set; }
        public CategoryViewModel CategoryViewModel { get; set; }
        public Dictionary<string, Category> Dictionary { get; set; }
        public WalletViewModel WalletViewModel { get; set; }

        public UserHasWalletViewModel UserHasWalletViewModel { get; set; }

        private List<char> _characters;
        public List<string> Categories { get; set; }
        public string QuantityText { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public AddMovementDialog( UserViewModel userViewModel, MovementViewModel movementViewModel, CategoryViewModel categoryViewModel, WalletViewModel walletViewModel)
        {
            this.InitializeComponent();

            Dictionary = new Dictionary<string, Category>();
            List<string> tempList = new List<string>();

            UserViewModel = userViewModel;
            CategoryViewModel = categoryViewModel;
            MovementViewModel = movementViewModel;
            WalletViewModel = walletViewModel;

            UserHasWalletViewModel = new UserHasWalletViewModel();
            UserHasWalletViewModel.LoadAllUsersAsync(WalletViewModel.Wallet.Id);

            foreach (var userHasWallet in UserHasWalletViewModel.UsersHasWallets)
            {
                CategoryViewModel.LoadAllAsync(userHasWallet.UserId);

                foreach (var category in CategoryViewModel.Categories)
                {
                    if (Dictionary.ContainsKey(category.Name) == false)
                    {
                        Dictionary.Add(category.Name, category);

                        tempList.Add(category.Name);
                    }
                }
            }

            Categories = new List<string>(tempList);


            _characters = new List<char> { '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' };
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Quantity and IsIncome
            MovementViewModel.Movement.Quantity = float.Parse(QuantityText, CultureInfo.InvariantCulture.NumberFormat);
            if (MovementViewModel.Movement.Quantity > 0)
                MovementViewModel.Movement.IsIncome = true;
            else
            {
                MovementViewModel.Movement.IsIncome = false;
                MovementViewModel.Movement.Quantity *= -1;
            }

            // Category & CategoryID
            if (CategoryViewModel.Categories.Count > 0)
            {
                MovementViewModel.Movement.Category = CategoryViewModel.Dictionary[CategoriesComboBox.SelectedItem.ToString()];
                MovementViewModel.Movement.CategoryId = CategoryViewModel.Dictionary[CategoriesComboBox.SelectedItem.ToString()].Id;
            }

            //WalletID
            MovementViewModel.Movement.WalletId = WalletViewModel.Wallet.Id;

            //Date&Time
            MovementViewModel.Movement.MovementDate = DateTime.DateTime;

            var deferral = args.GetDeferral();
            args.Cancel = !await MovementViewModel.CreateOrUpdateMovementAsync();

            deferral.Complete();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
        private void TextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {

            args.Cancel = args.NewText.Any(c => !_characters.Contains(c));
        }
    }
}
