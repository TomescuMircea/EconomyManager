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
    public class PlanViewModel : BindableBase
    {
       public ObservableCollection<Plan> Plans { get; set; }
       public Plan Plan;

       private IUnitOfWork _uow {  get; set; }

        public PlanViewModel() 
        { 
            Plans = new ObservableCollection<Plan>();
            _uow = new UnitOfWork();
            Plan = new Plan();
        }

        public async Task<bool> LoadAllAsync(Wallet w)
        {
            var list = await _uow.PlanRepository.FindAllByWalletIdAsync(w.Id);
            Plans.Clear();
            foreach (var plan in list)
            {
                Plans.Add(plan);
            }
            Plans = new ObservableCollection<Plan>(Plans.OrderBy(p => p.Deadline));
            return true;
        }

        public async Task<Plan> CreateOrUpdatePlanAsync(int WalletId)
        {
            // Checking for a Category with the new name
            var existingPlan = Plans
                .FirstOrDefault(x => x.Id == Plan.Id);

            if (existingPlan == null)
            {
                // We don't have a Category with the new name

                if (Plan.Id == 0)
                {
                    // Add a new category
                    var plan = new Plan {
                        Name = Plan.Name,
                        Deadline = Plan.Deadline,
                        Objective = Plan.Objective,
                        WalletId = WalletId
                    };
                    _uow.PlanRepository.Create(plan);
                    Plans.Add(plan);
                    Plan = plan;
                }
                else
                {
                    // Update Category

                    _uow.PlanRepository.Update(Plan);
                }
            }
            else
            {
                return null;
            }

            await _uow.SaveAsync();
            return Plan;
        }
    }
}
