/*

    CloudGenix Controller SDK
    (c) 2018 CloudGenix, Inc.
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

namespace CloudGenix.Classes
{
    public class EndpointManager
    {
        #region Public-Members

        #endregion

        #region Private-Members

        private readonly object _Lock;
        private Dictionary<string, string> _Endpoints; 
        private Dictionary<string, string> _Versions;

        #endregion

        #region Constructors-and-Factories

        public EndpointManager()
        {
            _Lock = new object(); 
            _Versions = new Dictionary<string, string>();
            _Endpoints = new Dictionary<string, string>();

            // overload versions
            AddVersion("login", "v2.0");
            AddVersion("logout", "v2.0");
            AddVersion("permissions", "v2.0");
            AddVersion("profile", "v2.0");
            AddVersion("query_events", "v3.0");

            // endpoints that are needed prior to pulling permissions or require overload
            AddEndpoint("login", "/%s/api/login");
            AddEndpoint("logout", "/%s/api/logout");
            AddEndpoint("permissions", "/%s/api/permissions");
            AddEndpoint("profile", "/%s/api/profile");
            AddEndpoint("flows_monitor", "/%s/api/tenants/%s/monitor/flows");
            AddEndpoint("query_events", "/%s/api/tenants/%s/events/query");
        }

        #endregion

        #region Public-Methods

        public void AddVersion(string key, string val)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (String.IsNullOrEmpty(val)) throw new ArgumentNullException(nameof(val));

            lock (_Lock)
            {
                if (_Versions.ContainsKey(key)) return;
                _Versions.Add(key, val);

                Debug.WriteLine("AddVersion added " + key + ": " + val);
            }
        }

        public void AddEndpoint(string key, string val)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (String.IsNullOrEmpty(val)) throw new ArgumentNullException(nameof(val));

            lock (_Lock)
            {
                if (_Endpoints.ContainsKey(key)) _Endpoints.Remove(key);

                if (_Versions != null && _Versions.Count > 0)
                {
                    if (!_Versions.ContainsKey(key))
                    {
                        Debug.WriteLine("AddEndpoint unable to find version for " + key + ", skipping");
                        return;
                    }
                    else
                    {
                        val = InsertVersionString(val, _Versions[key]); 
                    }
                }

                _Endpoints.Add(key, val);

                Debug.WriteLine("AddEndpoint added " + key + ": " + val);
            }
        }

        public string GetVersion(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            lock (_Lock)
            {
                if (_Versions.ContainsKey(key))
                {
                    Debug.WriteLine("GetVersion returning " + key + ": " + _Versions[key]);
                    return _Versions[key];
                }
                else
                {
                    Debug.WriteLine("GetVersion unable to find " + key);
                    throw new KeyNotFoundException(key);
                }
            }
        }

        public Dictionary<string, string> GetAllVersions()
        {
            lock (_Lock)
            {
                Dictionary<string, string> ret = new Dictionary<string, string>(_Versions);
                return ret;
            }
        }

        public string GetEndpoint(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            lock (_Lock)
            {
                if (_Endpoints.ContainsKey(key))
                {
                    Debug.WriteLine("GetEndpoint returning " + key + ": " + _Endpoints[key]);
                    return _Endpoints[key];
                }
                else
                {
                    Debug.WriteLine("GetEndpoint unable to find " + key);
                    throw new KeyNotFoundException(key);
                }
            }
        }

        public Dictionary<string, string> GetAllEndpoints()
        {
            lock (_Lock)
            {
                Dictionary<string, string> ret = new Dictionary<string, string>(_Endpoints);
                return ret;
            }    
        }

        #endregion

        #region Private-Methods

        private string InsertVersionString(string src, string ver)
        {
            if (String.IsNullOrEmpty(src)) throw new ArgumentNullException(nameof(src));
            if (String.IsNullOrEmpty(ver)) throw new ArgumentNullException(nameof(ver));

            if (ver.Contains("|"))
            {
                #region Regex

                // "query_events":"v(2\\.0|3\\.0)"
                // "query_events":"/%s/api/tenants/%s/events/query"
                ver = ver.Replace("v(", "");
                ver = ver.Replace("(", "");
                ver = ver.Replace(")", "");

                while (ver.Contains("\\"))
                {
                    ver = ver.Replace("\\", "");
                }

                string last = "";
                string[] versions = ver.Split('|');

                foreach (string curr in versions)
                {
                    last = "v" + curr;    
                }

                src = Common.StringReplaceFirst(src, "%s", last);
                return src;

                #endregion
            }
            else
            {
                #region Normal

                src = Common.StringReplaceFirst(src, "%s", ver);
                return src;

                #endregion
            }
        }

        #endregion
    } 
}
