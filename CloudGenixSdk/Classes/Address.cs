using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;

namespace CloudGenix.Classes
{
    public class Address
    {
        #region Public-Members

        [JsonProperty("street")]
        public string Street1 { get; set; }

        [JsonProperty("street2")]
        public string Street2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("post_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public Address()
        {
        }

        #endregion

        #region Public-Methods

        public override string ToString()
        {
            return string.Format("[Address: Street1={0}, Street2={1}, City={2}, State={3}, PostalCode={4}, Country={5}]", Street1, Street2, City, State, PostalCode, Country);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
