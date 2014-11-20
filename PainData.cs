using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace JSONWebServiceLeukemia
{
    [DataContract]
    [Serializable]
    public class wsPainData
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public int painlevel { get; set; }

        [DataMember]
        public string drawingpath { get; set; }

        [DataMember]
        public string photopath { get; set; }

        [DataMember]
        public int morphinelevel { get; set; }

        [DataMember]
        public string morphineunit { get; set; }

        [DataMember]
        public string date { get; set; }

        [DataMember]
        public string paintype { get; set; }

        [DataMember]
        public bool paracetamol { get; set; }

        public override string ToString()
        {
            string strId = id.ToString();
            return "Id: " + strId + " Pain level:" + painlevel.ToString();
        }
    }
}