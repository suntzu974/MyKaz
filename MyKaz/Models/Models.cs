using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyKaz.Models
{
    
    [DataContract]
    public class ForecastResponse 
    {

        [DataMember(Name = "status")]
        public string status { get; set; }
        [DataMember(Name = "success")]
        public bool success { get; set; }
        [DataMember(Name = "info")]
        public string info { get; set; }
        [DataMember(Name = "forecasts")]
        public Forecast forecast { get; set; }
    }
    [DataContract]
    public class Forecast 
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public DateTime created_at { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public float temperature { get; set; }
        [DataMember]
        public float pressure { get; set; }
        [DataMember]
        public float light { get; set; }
        [DataMember]
        public float altitude { get; set; }
        [DataMember]
        public float humidity { get; set; }
    }
}
