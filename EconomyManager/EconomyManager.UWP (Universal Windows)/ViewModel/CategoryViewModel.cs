using EconomyManager.Domain;
using EconomyManager.Domain.Models;
using EconomyManager.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager.UWP__Universal_Windows_.ViewModel
{
    public class CategoryViewModel : BindableBase
    {
        private IUnitOfWork _uow;

        public CategoryViewModel()
        {
            Category = new Category();
            Categories = new ObservableCollection<Category>();
            _uow = new UnitOfWork();
            ListCategories = new ObservableCollection<string>();
            Dictionary = new Dictionary<string, Category>();
        }
        public Dictionary<string, Category> Dictionary { get; set; }


        public ObservableCollection<Category> Categories { get; set; }
        private ObservableCollection<string> _listCategories { get; set; }

        public ObservableCollection<string> ListCategories
        {
            get
            {
                return _listCategories;
            }
            set
            {
                if (_listCategories != value)
                {
                    _listCategories = value;
                    OnPropertyChanged(nameof(ListCategories));
                }
            }
        }

        private string _categoryName;

        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                //_categoryName = value; 
                Set(ref _categoryName, value);
                OnPropertyChanged(nameof(Valid));
                OnPropertyChanged(nameof(Invalid));
                OnPropertyChanged(nameof(ValidNameAndType));
                OnPropertyChanged(nameof(InvalidNameAndType));
            }
        }

        private int _typeOfCategory;
        public int TypeOfCategory
        {
            get { return _typeOfCategory; }
            set
            {
                Set(ref _typeOfCategory, value);
                OnPropertyChanged(nameof(InvalidType));
                OnPropertyChanged(nameof(ValidType));
                OnPropertyChanged(nameof(ValidNameAndType));
                OnPropertyChanged(nameof(InvalidNameAndType));
            }
        }

        private Category _category;

        public Category Category
        {
            get { return _category; }
            set
            {
                _category = value;
                CategoryName = _category?.Name;

            }
        }

        public int UserId;
        public string PageTitle
        {
            get
            {
                return Category.Id == 0 ? "New Category" : $"Edit category " + CategoryName;
            }
        }

        public bool ValidType
        {
            get
            {
                return _typeOfCategory != 0;
            }
        }
        public bool InvalidType
        {
            get
            {
                return !ValidType;
            }
        }
        public bool Valid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(CategoryName);
            }
        }
        public bool Invalid
        {
            get
            {
                return !Valid;
            }
        }
        public bool ValidNameAndType
        {
            get
            {
                return (Valid && ValidType);
            }
        }
        public bool InvalidNameAndType
        {
            get
            {
                return !ValidNameAndType;
            }
        }


        public async void LoadAllAsync(int userID)
        {
            var lis = await _uow.CategoryRepository.CategoriesFromUserId(userID);
            Categories.Clear();

            foreach (var category in lis)
            {
                Categories.Add(category);
                ListCategories.Add(category.Name);
                if (Dictionary.ContainsKey(category.Name) == false)
                    Dictionary.Add(category.Name, category);
            }

        }
        public async Task<bool> CreateOrUpdateCategoryAsync(User user)
        {
            // Checking for a Category with the new name
            var existingCategory = Categories
                .FirstOrDefault(x => x.Name == CategoryName);

            if (existingCategory == null)
            {
                // We don't have a Category with the new name

                if (Category.Id == 0)
                {
                    // Add a new category
                    var category = new Category
                    {
                        Name = CategoryName,
                        IsIncome = (TypeOfCategory == 1),
                        UserId = user.Id,
                    };
                    _uow.CategoryRepository.Create(category);
                    Categories.Add(category);
                }
                else
                {
                    // Update Category

                    Category.Name = CategoryName;
                    _uow.CategoryRepository.Update(Category);
                }
            }
            else
            {
                return false;
            }

            await _uow.SaveAsync();
            return true;
        }

        internal async void DeleteAsync()
        {
            _uow.CategoryRepository.Delete(Category);
            Categories.Remove(Category);
            await _uow.SaveAsync();
        }
    }
}
