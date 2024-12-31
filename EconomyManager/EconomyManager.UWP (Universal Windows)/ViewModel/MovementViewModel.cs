using EconomyManager.Domain.Models;
using EconomyManager.Domain;
using EconomyManager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EconomyManager.Domain.Utilities;

namespace EconomyManager.UWP__Universal_Windows_.ViewModel
{
    public class MovementViewModel : BindableBase
    {
        public bool Valid
        {
            get
            {
                return Quantity != 0;
            }
        }
        public bool Invalid
        {
            get
            {
                return !Valid;
            }
        }



        private IUnitOfWork _uow;

        public ObservableCollection<Movement> Movements;

        public int _movementId;

        public float Quantity { get; set; }

        public int MovementId
        {
            get { return _movementId; }
            set
            {
                Set(ref _movementId, value);
                OnPropertyChanged(nameof(Valid));
                OnPropertyChanged(nameof(Invalid));
            }
        }

        private Movement _movement;

        public Movement Movement
        {
            get => _movement;
            set
            {
                _movement = value;
                MovementId = (_movement != null) ? _movement.Id : 0;
            }
        }

        public MovementViewModel()
        {
            Movements = new ObservableCollection<Movement>();
            Movement = new Movement();
            _uow = new UnitOfWork();
        }


        public async Task<bool> LoadAllAsync(Wallet w)
        {
            var list = await _uow.MovementRepository.FindAllByWalletIdAsync(w.Id);
            Movements.Clear();
            foreach (var movement in list)
            {
                Movements.Add(movement);
            }
            Movements = new ObservableCollection<Movement>(Movements.OrderBy(m => m.MovementDate).Reverse());
            return true;
        }

        public async Task<bool> CreateOrUpdateMovementAsync()
        {
            // Checking for a Movement with the new name
            var existingMovement = Movements
                .FirstOrDefault(x => x.Id == MovementId);

            if (existingMovement == null)
            {
                // We don't have a Movement with the new name

                if (Movement.Id == 0)
                {
                    // Add a new Movement
                    var movement = new Movement()
                    {
                        Quantity = Movement.Quantity,
                        MovementDate = Movement.MovementDate,
                        IsIncome = Movement.IsIncome,
                        CategoryId = Movement.CategoryId,
                        WalletId = Movement.WalletId,
                    };
                    _uow.MovementRepository.Create(movement);
                    Movements.Add(movement);
                }
                else
                {
                    // Update Movement

                    Movement.Quantity = Quantity;
                    _uow.MovementRepository.Update(Movement);
                }
            }
            else
            {
                if (existingMovement.Id != 0)
                {
                    Movement.Quantity = Quantity;
                    _uow.MovementRepository.Update(Movement);
                }
                else
                    return false;
            }

            await _uow.SaveAsync();
            return true;
        }

        public async void DeleteAsync()
        {
            _uow.MovementRepository.Delete(Movement);
            Movements.Remove(Movement);
            await _uow.SaveAsync();
        }
    }
}
