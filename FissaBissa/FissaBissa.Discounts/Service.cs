namespace FissaBissa.Discounts
{
    public class Service : IService
    {
        public int GetDiscount(DataModel model)
        {
            return model.Price;
        }
    }
}
