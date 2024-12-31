using EconomyManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyManager.Domain.Utilities
{
    public class MovementUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="movements"> List of movements. Take in consideration that the 
        /// Category and all the movements should have the same isIncome type</param>
        /// <param name="category">The category for which you want to know the objective</param>
        /// <returns></returns>
        public static float getRatioOfCategory(List<Movement> movements, Category category)
        {
            float ratio = 0f;
            float total = 0f;
            foreach (Movement movement in movements)
            {
                total += movement.Quantity;
                if (category.Name.Equals(movement.Category.Name))
                {
                    ratio += movement.Quantity;
                }
            }
            return ratio / total;
        }

        /// <summary>
        /// This method get's the objective that represents the quantity of the movements in the list 
        /// which isIncome type matches the one passed as an argument opposite the opposite quantity of
        /// the movements in the list
        /// <example>
        /// For example: If I have a list with 100€ as opposite of quantity for !isIncome and a 
        /// opposite of 40€ for isIncome, then the result is 40/100 = 0.4
        /// <code>
        /// List<Movement> = new List<Movement>();
        /// float objective = new Point(movements, Movement.OUTCOME);
        /// </code>
        /// results in <c>p</c>'s having the value (2,8).
        /// </example>
        /// </summary>
        public static float getRatioOfType(List<Movement> movements, bool isIncome)
        {
            float objective = MovementUtilities.getTotalOfType(movements, isIncome);
            float opposite = MovementUtilities.getTotalOfType(movements, !isIncome);
            
            return objective / opposite;
        }

        public static float getTotalOfType(List<Movement> movements, bool isIncome)
        {
            float total = 0f;
            foreach (Movement movement in movements)
            {
                if(movement.IsIncome == isIncome)
                {
                    total += movement.Quantity;
                }
            }
            return total;
        }

        public static float getTotalOfCategory(List<Movement>movements, Category cat)
        {
            float total = 0f;
            foreach(Movement mov in movements)
            {
                if(mov.CategoryId == cat.Id)
                {
                    total += mov.Quantity;
                }
            }
            return total;
        }

        public static float getTotalOfMonth(List<Movement> movements, int month)
        {
            float total = 0f;
            foreach (Movement mov in movements)
            {
                if (mov.MovementDate.Month == month)
                {
                    total += (mov.IsIncome) ? mov.Quantity : -mov.Quantity;
                }
            }
            return total;
        }
    }
}
