using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace FissaBissa.Discounts
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        Dictionary<string, int> GetDiscount(DataModel model);
    }

    [DataContract]
    public class DataModel
    {
        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public List<AnimalModel> Animals { get; set; }
    }

    [DataContract]
    public class AnimalModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}
