using EconomyManager.Domain;
using EconomyManager.Domain.Models;
using EconomyManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager.UWP__Universal_Windows_.ViewModel
{
    public class WalletViewModel : BindableBase
    {

        public bool Valid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(WalletName);
            }
        }
        public bool Invalid
        {
            get
            {
                return !Valid;
            }
        }
        public bool ValidDescription
        {
            get
            {
                return !string.IsNullOrWhiteSpace(WalletDescription);
            }
        }
        public bool InvalidDescription
        {
            get
            {
                return !ValidDescription;
            }
        }
        public string PageTitle
        {
            get
            {
                return Wallet.Id == 0 ? "New Wallet" : $"Edit Wallet " + WalletName;
            }
        }

        private IUnitOfWork _uow;

        public ObservableCollection<Wallet> Wallets;

        public string _walletName;

        public string WalletName 
        {
            get { return _walletName; }
            set 
            {
                Set(ref _walletName, value);
                OnPropertyChanged(nameof(Valid));
                OnPropertyChanged(nameof(Invalid));
            }
        }
        public string _walletDescription;

        public string WalletDescription
        {
            get { return _walletDescription; }
            set
            {
                Set(ref _walletDescription, value);
                OnPropertyChanged(nameof(ValidDescription));
                OnPropertyChanged(nameof(InvalidDescription));
            }
        }

        private Wallet _wallet;

        public Wallet Wallet
        {
            get
            {
                return _wallet;
            }
            set
            { 
                _wallet = value;
                WalletName = _wallet?.Name;
                WalletDescription = _wallet?.Description;
            }
        }

        public WalletViewModel() 
        {
            Wallet = new Wallet();
            Wallets = new ObservableCollection<Wallet>();
            _uow = new UnitOfWork();
        }


        public async Task<bool> LoadAllAsync(User u)
        {
            var list = await _uow.WalletRepository.FindAllByUserIdAsync(u.Id);
            Wallets.Clear();
            foreach (var wallet in list)
            {
                Wallets.Add(wallet);
            }
            return true;
        }

        public async Task<bool> CreateOrUpdateWalletAsync()
        {
            // Checking for a Movement with the new name
            var existingWallet = Wallets
                .FirstOrDefault(x => x.Name == WalletName);

            if (existingWallet == null)
            {
                // We don't have a Movement with the new name

                if (Wallet.Id == 0)
                {
                    // Add a new Movement
                    var wallet = new Wallet { 
                        Name = WalletName,
                        Description= WalletDescription,
                    };
                    _uow.WalletRepository.Create(wallet);
                    Wallets.Add(wallet);
                }
                else
                {
                    // Update Movement

                    Wallet.Name = WalletName;
                    _uow.WalletRepository.Update(Wallet);
                }
            }
            else
            {
                return false;
            }

            await _uow.SaveAsync();
            Wallet= await _uow.WalletRepository.FindByNameAsync(WalletName);
            return true;
        }

        public async void DeleteAsync()
        {
            _uow.WalletRepository.Delete(Wallet);
            Wallets.Remove(Wallet);
            await _uow.SaveAsync();
        }
    }
}
