/*

    CloudGenix Controller SDK
    (c) 2017 CloudGenix, Inc.
    All Rights Reserved

    https://www.cloudgenix.com

    This SDK is released under the MIT license.
    For support, please contact us on:

        NetworkToCode Slack channel #cloudgenix: http://slack.networktocode.com
        Email: developers@cloudgenix.com

 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 

namespace CloudGenix.Classes
{
    public class ResourceResponse
    {
        #region Public-Members

        [JsonProperty("_etag")]
        public long Etag { get; set; }

        [JsonProperty("_content_length")]
        public long ContentLength { get; set; }

        [JsonProperty("_schema")]
        public long Schema { get; set; }

        [JsonProperty("_created_on_utc")]
        public long CreatedOnUtc { get; set; }

        [JsonProperty("_updated_on_utc")]
        public long UpdatedOnUtc { get; set; }

        [JsonProperty("_status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("_request_id")]
        public string RequestId { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("items")]
        public object Items { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public ResourceResponse()
        {
        }

        #endregion

        #region Public-Methods

        public T GetItems<T>()
        {
            if (Items == null) return default(T);

            if (Items is JArray)
            {
                if (((JArray)Items).Type == JTokenType.Null)
                {
                    return default(T);
                }
                return ((JArray)Items).ToObject<T>();
            }
            else if (Items is JObject)
            {
                if (((JObject)Items).Type == JTokenType.Null)
                {
                    return default(T);
                }
                return ((JObject)Items).ToObject<T>();
            }
            else
            {
                throw new Exception("Unable to discern items data type.");
            }
        }
         
        #endregion

        #region Private-Methods

        #endregion
    }
}
