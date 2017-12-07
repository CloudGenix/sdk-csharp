using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;

namespace CloudGenix.Classes
{
    public class SecurityPolicyRule
    {
        #region Public-Members

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("application_ids")]
        public List<string> ApplicationIds { get; set; }

        [JsonProperty("disabled_flag")]
        public bool Disabled { get; set; }

        [JsonProperty("destination_filter_ids")]
        public List<string> DestinationFilterIds { get; set; }

        [JsonProperty("destination_zone_ids")]
        public List<string> DestinationZoneIds { get; set; }

        [JsonProperty("source_filter_ids")]
        public List<string> SourceFilterIds { get; set; }

        [JsonProperty("source_zone_ids")]
        public List<string> SourceZoneIds{ get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public SecurityPolicyRule()
        {
        }

        #endregion

        #region Public-Methods
             
        public override string ToString()
        {
            return string.Format("[SecurityPolicyRule: Id={0}, Name={1}, Action={2}, ApplicationIds={3}, Disabled={4}, DestinationFilterIds={5}, DestinationZoneIds={6}, SourceFilterIds={7}, SourceZoneIds={8}]", Id, Name, Action, ApplicationIds, Disabled, DestinationFilterIds, DestinationZoneIds, SourceFilterIds, SourceZoneIds);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
