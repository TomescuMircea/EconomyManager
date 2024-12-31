using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.InkML;
using ErasmusEconomy.Library.Repositories;
using System.Xml.Linq;
using Windows.UI.Xaml;
using EconomyManager.Domain.Repositories;
using EconomyManager.Domain.SeedWork;
using EconomyManager.Domain.Models;

namespace EconomyManager.UWP__Universal_Windows_.ViewModel
{
    public class SettingsViewModel : BindableBase
    {
        private IUserRepository _userRepository;
        private User _currentUser;

        public SettingsViewModel()
        {
            _currentUser = new User(); 
            LoadCurrentUser();
        }

        private void LoadCurrentUser()
        {
            _currentUser = _userRepository.FindByIdAsync(CurrentUserId).Result;
        }

        public string CurrentUserId { get; set; } // You need to set this when the user logs in

        public User CurrentUser => _currentUser;

        public void UpdateUser(User updatedUser)
        {
            _userRepository.Update(updatedUser);
        }
    }
}
