using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace JSONWebServiceLeukemia
{
    [DataContract]
    public class wsUploadResult
    {
        [DataMember]
        public int WasSuccessful { get; set; }

        [DataMember]
        public string Exception { get; set; }
    }
}