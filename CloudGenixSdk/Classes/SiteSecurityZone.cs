using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;

namespace CloudGenix.Classes
{
    public class SiteSecurityZone
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("zone_id")]
        public string ZoneId { get; set; }

        [JsonProperty("networks")]
        public object Networks { get; set; }
         
        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public SiteSecurityZone()
        {
        }

        #endregion

        #region Public-Methods
           
        public override string ToString()
        {
            return string.Format("[SiteSecurityZone: Id={0}, ZoneId={1}, Networks={2}]", Id, ZoneId, Networks);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
