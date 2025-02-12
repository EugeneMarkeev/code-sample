using Domain.PlayerResources;

namespace Domain.Planets
{
    public class PlanetLevel
    {
        public Resource Price { get; }
        public int IncomeAmount { get; }
        public int Population { get; }
        
        public PlanetLevel(Resource price, int incomeAmount, int population)
        {
            Price = price;
            IncomeAmount = incomeAmount;
            Population = population;
        }
    }
}