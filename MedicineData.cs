using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace JSONWebServiceLeukemia
{
    [DataContract]
    [Serializable]
    public class wsMedicineData
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string kemoTreatment { get; set; }

        [DataMember]
        public int mercaptopurin { get; set; }

        [DataMember]
        public int mtx { get; set; }

        [DataMember]
        public string date { get; set; }

        [DataMember]
        public wsBloodSample bloodSample { get; set; }

        public override string ToString()
        {
            string strId = id.ToString();
            return "Id: " + strId + " Date:" + date;
        }
    }
}