using EconomyManager.Domain.Models;
using EconomyManager.Domain;
using EconomyManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Wallet;

namespace EconomyManager.UWP__Universal_Windows_.ViewModel
{

    public class UserHasWalletViewModel : BindableBase
    {
        private IUnitOfWork _uow;

        public ObservableCollection<UserHasWallet> UsersHasWallets;

        public UserHasWalletViewModel()
        {
            UsersHasWallets = new ObservableCollection<UserHasWallet>();
            _uow = new UnitOfWork();
            UserHasWallet = new UserHasWallet();
        }



        private UserHasWallet _userHasWallet;

        public UserHasWallet UserHasWallet
        {
            get { return _userHasWallet; }
            set
            {
                _userHasWallet = value;
            }
        }


        public async void LoadAllUsersAsync(int walletId)
        {
            var list = await _uow.UserHasWalletRepository.FindByWalletIdAsync(walletId);
            UsersHasWallets.Clear();
            foreach (var userHasWallet in list)
            {
                UsersHasWallets.Add(userHasWallet);
            }
        }


        public async void LoadAllAsync(int userId)
        {
            var list = await _uow.UserHasWalletRepository.FindByUserIdAsync(userId);
            UsersHasWallets.Clear();
            foreach (var userHasWallet in list)
            {
                UsersHasWallets.Add(userHasWallet);
            }
        }

        public async Task<bool> CreateOrUpdateUserWasWalletAsync(User user,Wallet wallet)
        {
            // Checking for a Movement with the new name
            var existingUserHasWallet = UsersHasWallets
                .FirstOrDefault(x => x.WalletId==wallet.Id);

            if (existingUserHasWallet == null)
            {
                // We don't have a Movement with the new name

                if (UserHasWallet.Id == 0)
                {
                    // Add a new Movement
                    var userHasWallet = new UserHasWallet()
                    {
                        UserId = user.Id,
                        WalletId = wallet.Id,
                    };
                    _uow.UserHasWalletRepository.Create(userHasWallet);
                    UsersHasWallets.Add(userHasWallet);
                }
                else
                {
                    // Update Movement

                    //Wallet.Name = WalletName;
                    //_uow.WalletRepository.Update(Wallet);
                }
            }
            else
            {
                return false;
            }

            await _uow.SaveAsync();
            return true;
        }

        public async void DeleteAsync()
        {
            _uow.UserHasWalletRepository.Delete(UserHasWallet);
            UsersHasWallets.Remove(UserHasWallet);
            await _uow.SaveAsync();
        }


    }
}
