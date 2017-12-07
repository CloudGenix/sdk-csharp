using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;

namespace CloudGenix.Classes
{
    public class SecurityZone
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
         
        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public SecurityZone()
        {
        }

        #endregion

        #region Public-Methods
          
        public override string ToString()
        {
            return string.Format("[SecurityZone: Id={0}, Name={1}, Description={2}]", Id, Name, Description);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
