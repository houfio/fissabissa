using System;
using System.Collections.Generic;
using System.Linq;
using FissaBissa.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FissaBissa.Utilities
{
    public static class OrderValidator
    {
        public static void ValidateOrder(string key, ICollection<AnimalEntity> animals, DateTime date,
            ModelStateDictionary state)
        {
            if (animals.Count == 0)
            {
                state.AddModelError(key, "Must select at least one");
            }

            if (HasAnimalType(animals, "Boerderij") && HasAnimal(animals, "Leeuw", "Ijsbeer"))
            {
                state.AddModelError(key, "Invalid animal combination");
            }

            if ((date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) &&
                HasAnimal(animals, "Pinguïn"))
            {
                state.AddModelError(key, "Invalid order day");
            }

            if ((!InRange(date.Month, 2, 10) && HasAnimalType(animals, "Woestijn"))
                || (InRange(date.Month, 5, 8) && HasAnimalType(animals, "Sneeuw")))
            {
                state.AddModelError(key, "Invalid order month");
            }
        }

        private static bool HasAnimal(ICollection<AnimalEntity> animals, params string[] names)
        {
            return animals.Any(a => names.Contains(a.Name));
        }

        private static bool HasAnimalType(ICollection<AnimalEntity> animals, params string[] types)
        {
            return animals.Any(a => types.Contains(a.Type.Name));
        }

        private static bool InRange(int actual, int min, int max)
        {
            return actual > min && actual < max;
        }
    }
}
