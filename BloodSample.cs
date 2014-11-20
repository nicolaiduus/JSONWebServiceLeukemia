using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace JSONWebServiceLeukemia
{
    [DataContract]
    [Serializable]
    public class wsBloodSample
    {
        [DataMember]
        public int alat { get; set; }

        [DataMember]
        public int thrombocytes { get; set; }

        [DataMember]
        public int hemoglobin { get; set; }

        [DataMember]
        public int neutrofile { get; set; }

        [DataMember]
        public int crp { get; set; }

        [DataMember]
        public int other { get; set; }

        [DataMember]
        public int leukocytes { get; set; }
    }
}