using System;
using System.Collections.Generic;
using System.Linq;

namespace FissaBissa.Discounts
{
    public class Service : IService
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        public Dictionary<string, int> GetDiscount(DataModel model)
        {
            var discounts = model.Animals
                .Aggregate(new Dictionary<string, int>(), (result, current) =>
                {
                    result[current.Type] = result.TryGetValue(current.Type, out var amount) ? amount + 1 : 1;

                    return result;
                })
                .Where(p => p.Value >= 3)
                .ToDictionary(p => $"{p.Value}x {p.Key}", p => 10);

            if (model.Animals.Any(a => a.Name == "Eend") && new Random().Next(0, 5) == 0)
            {
                discounts["Eend"] = 50;
            }

            if (model.Date.DayOfWeek == DayOfWeek.Monday || model.Date.DayOfWeek == DayOfWeek.Tuesday)
            {
                discounts["Dag"] = 15;
            }

            var discount = model.Animals.Select(a => Enumerable.Range(0, Alphabet.Length).Aggregate(0, (result, current) => result == current && a.Name.Contains(Alphabet[current]) ? result + 1  : result) * 2).Sum();

            if (discount > 0)
            {
                discounts["Letter"] = discount;
            }

            return discounts;
        }
    }
}
