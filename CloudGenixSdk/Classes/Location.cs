using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;

namespace CloudGenix.Classes
{
    public class Location
    {
        #region Public-Members

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }

        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public Location()
        {
        }

        #endregion

        #region Public-Methods

        public override string ToString()
        {
            return string.Format("[Location: Longitude={0}, Latitude={1}, Description={2}]", Longitude, Latitude, Description);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
