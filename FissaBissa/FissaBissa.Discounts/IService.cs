using System.Runtime.Serialization;
using System.ServiceModel;

namespace FissaBissa.Discounts
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        int GetDiscount(DataModel model);
    }

    [DataContract]
    public class DataModel
    {
        [DataMember]
        public int Price { get; set; }
    }
}
