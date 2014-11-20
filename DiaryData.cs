using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace JSONWebServiceLeukemia
{
    [DataContract]
    [Serializable]
    public class wsDiaryData
    {
        [DataMember]
        public int protocolTreatmentDay { get; set; }
       
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public float weight { get; set; }

        [DataMember]
        public string notes { get; set; }

        [DataMember]
        public string date { get; set; }

        public override string ToString()
        {
            string strId = id.ToString();
            return "Id: " + strId + " Weight:" + weight.ToString();
        }
    }
}