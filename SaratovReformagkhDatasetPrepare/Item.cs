using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SaratovReformagkhDatasetPrepare
{
    [DataContract]
    class Item
    {
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Year { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }

        public Item(string address, string year, string latitude, string longitude)
        {
            this.Address = address;
            this.Year = year;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
