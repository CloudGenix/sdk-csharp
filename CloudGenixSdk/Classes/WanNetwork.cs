using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;

namespace CloudGenix.Classes
{
    public class WanNetwork
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("provider_as_numbers")]
        public object ProviderAsNumbers { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public WanNetwork()
        {
        }

        #endregion

        #region Public-Methods
         
        public override string ToString()
        {
            return string.Format("[WanNetwork: Id={0}, Name={1}, Type={2}, ProviderAsNumbers={3}]", Id, Name, Type, ProviderAsNumbers);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
