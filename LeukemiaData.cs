using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace JSONWebServiceLeukemia
{
    [DataContract]
    [Serializable]
    public class wsLeukemiaData
    {
        [DataMember]
        public string patientID { get; set; }
        
        [DataMember]
        public List<wsPainData> painData { get; set; }

        [DataMember]
        public List<wsMedicineData> medicineData { get; set; }

        [DataMember]
        public List<wsDiaryData> diaryData { get; set; }

        public override string ToString()
        {
            string result = "";
            foreach (wsMedicineData pd in medicineData)
            {
                result = result + pd.ToString() + ",";
            }
            return result;
        }
    }
}