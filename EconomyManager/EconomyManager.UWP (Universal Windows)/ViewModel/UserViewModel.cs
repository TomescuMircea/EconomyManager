using EconomyManager.Domain;
using EconomyManager.Domain.Models;
using EconomyManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager.UWP__Universal_Windows_.ViewModel
{
    public class UserViewModel : BindableBase
    {
        private IUnitOfWork _uow;
        private User _user;
        private List<User> Users {  get; set; }
        public User User {
            get 
            {
            return _user;
            }
            set
            {
                _user = value;
                UserEmail = _user?.Email;
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                Set(ref _userName, value);
                OnPropertyChanged(nameof(Valid));
                OnPropertyChanged(nameof(Invalid));
            }
        }
        private string _userEmail;

        public string UserEmail
        {
            get { return _userEmail; }
            set
            {
                Set(ref _userEmail, value);
                OnPropertyChanged(nameof(Valid));
                OnPropertyChanged(nameof(Invalid));
            }
        }

        private string _userPassword;

        public string UserPasssword
        {
            get { return _userPassword; }
            set
            {
                Set(ref _userPassword, value);
                OnPropertyChanged(nameof(Valid));
                OnPropertyChanged(nameof(Invalid));
            }
        }
        private string _userRepeatPassword;

        public string UserRepeatPassword
        {
            get { return _userRepeatPassword; }
            set
            {
                Set(ref _userRepeatPassword, value);
                OnPropertyChanged(nameof(Valid));
                OnPropertyChanged(nameof(Invalid));
            }
        }

        public bool Valid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(UserEmail);
            }
        }
        public bool Invalid
        {
            get
            {
                return !Valid;
            }
        }




        private User _loggedUser;

        public UserViewModel()
        {
            _uow = new UnitOfWork();
            User = new User();
        }
        public User LoggedUser
        {
            get { return _loggedUser; }
            set
            {
                _loggedUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsLogged));
                OnPropertyChanged(nameof(IsNotLogged));
                //OnPropertyChanged(nameof(IsAdmin));
            }
        }
        public bool IsLogged
        {
            get { return _loggedUser != null; }
        }
        public bool IsNotLogged
        {
            get => !IsLogged;
        }

        private bool _showError;

        public bool ShowError
        {
            get { return _showError; }
            set
            {
                _showError = value;
                OnPropertyChanged();
            }
        }

        internal async Task<bool> DoLoginAsync()
        {
            var user = await _uow.UserRepository
                .FindByEmailAndPasswordAsync(User.Email, User.Password);

            LoggedUser = user;
            ShowError = user == null || !user.Active ;
            return !ShowError;
        }

        internal async Task<bool> DoRegisterAsync()
        {
            ShowError = true;

            var user = await _uow.UserRepository.FindByEmailAsync(User.Email);
            if (user == null)
            {
                User.Active = true;
                User.Categories = new List<Category>();
                User.Categories.Add(new Category{
                    Name = "Base Outcome",
                    IsIncome = false
                });
                User.Categories.Add(new Category{
                    Name = "Base Income",
                    IsIncome = true
                });
                _uow.UserRepository.Create(User);
                await _uow.SaveAsync();
                LoggedUser = User;
                ShowError = false;
            }
            return !ShowError;
        }

        internal void DoLogout()
        {
            LoggedUser = null;
            User = new User();
        }

        public async void LoadAllAsync()
        {
            Users = await _uow.UserRepository.FindAllAsync();
        }

        public async Task<bool> UpdateUserDataAsync()
        {
            LoggedUser.Name = User.Name;
            LoggedUser.Email = User.Email;

            _uow.UserRepository.Update(LoggedUser);
            await _uow.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateUserPasswordAsync(string newPassword)
        {
            LoggedUser.Password = newPassword;

            _uow.UserRepository.Update(LoggedUser);
            await _uow.SaveAsync();
            return true;
        }


    }
}
