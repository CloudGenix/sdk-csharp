using System;
using System.Collections;
using System.Collections.Generic;
using CloudGenix;
using CloudGenix.Classes;

namespace SdkCli
{
    public class SdkCli
    {
        #region Public-Members

        #endregion

        #region Private-Members

        private static bool _RunForever = true;
        private static CgnxController _Cgnx;

        private static List<Context> _Contexts = null;
        private static List<Site> _Sites = null;
        private static List<Element> _Elements = null;
        private static List<ElementInterface> _ElementInterfaces = null;
        private static ElementInterfaceStatus _ElementInterfaceStatus = null;
        private static List<WanNetwork> _WanNetworks = null;
        private static List<LanNetwork> _LanNetworks = null;
        private static List<SiteWanInterface> _SiteWanInterfaces = null;
        private static Topology _SiteTopology = null;
        private static List<SnmpAgent> _SnmpAgents = null;

        private static List<ApplicationDefinition> _ApplicationDefinitions = null;
        private static List<PolicySet> _PolicySets = null;
        private static List<PolicyRule> _PolicyRules = null;

        private static List<SecurityZone> _SecurityZones = null;
        private static List<SiteSecurityZone> _SiteSecurityZones = null;
        private static List<SecurityPolicySet> _SecurityPolicySets = null;
        private static List<SecurityPolicyRule> _SecurityPolicyRules = null;

        private static MetricsQuery _MetricsQuery = null;
        private static MetricsResponse _MetricsResponse = null;

        private static TopNQuery _TopNQuery = null;
        private static TopNResponse _TopNResponse = null;

        private static FlowQuery _FlowQuery = null;
        private static FlowResponse _FlowResponse = null;

        private static EventQuery _EventQuery = null;
        private static EventResponse _EventResponse = null; 

        #endregion

        #region Constructors-and-Factories

        #endregion

        #region Public-Methods

        public static void Main(string[] args)
        {
            Welcome();

            #region Initialize-and-Login

            _Cgnx = new CgnxController(
                Common.InputString("Email address :", "demo@cloudgenix.com", false),
                Common.InputString("Password      :", "demo@cloudgenix.com", false));

            if (!_Cgnx.Login())
            {
                Console.WriteLine("Login failed.");
                return;
            }
            else
            {
                Console.WriteLine("Login succeeded.");
            }

            #endregion

            #region Menu

            while (_RunForever)
            {
                string cmd = Common.InputString("Command [? for help] >", null, false);
                switch (cmd)
                {
                    case "?":
                        Menu();
                        break;

                    case "q":
                        _RunForever = false;
                        break;

                    case "c":
                    case "cls":
                        Console.Clear();
                        break;

                    case "logout":
                        Console.WriteLine(_Cgnx.Logout());
                        break;

                    #region Show-Commands

                    case "show tenant_id":
                        Console.WriteLine(_Cgnx.TenantId);
                        break;

                    case "show token":
                        Console.WriteLine(_Cgnx.AuthToken);
                        break;

                    case "show versions":
                        Console.WriteLine(Common.SerializeJson(_Cgnx.GetAllVersions(), true));
                        break;

                    case "show endpoints":
                        Console.WriteLine(Common.SerializeJson(_Cgnx.GetAllEndpoints(), true));
                        break;

                    #endregion

                    #region Get-Commands

                    case "get contexts":
                        GetContexts();
                        break;

                    case "get sites":
                        GetSites();
                        break;

                    case "get elements":
                        GetElements();
                        break;

                    case "get interfaces":
                        GetElementInterfaces();
                        break;

                    case "get ifstatus":
                        GetElementInterfaceStatus();
                        break;

                    case "get wans":
                        GetWanNetworks();
                        break;

                    case "get lans":
                        GetLanNetworks();
                        break;

                    case "get appdefs":
                        GetApplicationDefinitions();
                        break;

                    case "get policysets":
                        GetPolicySets();
                        break;

                    case "get policyrules":
                        GetPolicyRules();
                        break;

                    case "get seczones":
                        GetSecurityZones();
                        break;

                    case "get siteseczones":
                        GetSiteSecurityZones();
                        break;

                    case "get secpolsets":
                        GetSecurityPolicySets();
                        break;

                    case "get secpolrules":
                        GetSecurityPolicyRules();
                        break;

                    case "get sitewanifs":
                        GetSiteWanInterfaces();
                        break;

                    case "get topology":
                        GetSiteTopology();
                        break;

                    case "get snmpagents":
                        GetSnmpAgents();
                        break;

                    #endregion

                    #region Metrics-Commands

                    case "metrics clear":
                        ClearMetricsQuery();
                        break;

                    case "metrics show":
                        ShowMetricsQuery();
                        break;

                    case "metrics build":
                        BuildMetricsQuery();
                        break;

                    case "metrics addmetric":
                        AddMetric();
                        break;

                    case "metrics addfilter":
                        AddMetricsFilter();
                        break;

                    case "metrics submit":
                        SendMetricsQuery();
                        break;

                    #endregion

                    #region TopN-Commands

                    case "topn clear":
                        ClearTopNQuery();
                        break;

                    case "topn show":
                        ShowTopNQuery();
                        break;

                    case "topn build":
                        BuildTopNQuery();
                        break;

                    case "topn addfilter":
                        AddTopNFilter();
                        break;

                    case "topn submit":
                        SendTopNQuery();
                        break;

                    #endregion

                    #region Flows-Commands

                    case "flows clear":
                        ClearFlowsQuery();
                        break;

                    case "flows show":
                        ShowFlowsQuery();
                        break;

                    case "flows build":
                        BuildFlowsQuery();
                        break;

                    case "flows addfilter":
                        AddFlowsFilter();
                        break;

                    case "flows submit":
                        SendFlowsQuery();
                        break;

                    #endregion

                    #region Events-Commands
 
                    case "events clear":
                        ClearEventsQuery();
                        break;

                    case "events show":
                        ShowEventsQuery();
                        break;

                    case "events build":
                        BuildEventsQuery();
                        break;

                    case "events submit":
                        SendEventsQuery();
                        break;
                         
                    #endregion

                    default:
                        break;
                }
            }

            #endregion
        }

        #endregion

        #region Private-Methods

        private static void Welcome()
        {
            Console.WriteLine("CloudGenix Controller SDK");
            Console.WriteLine("");
        }

        private static void Menu()
        {
            //                          1         2         3         4         5         6         7         
            //                 1234567890123456789012345678901234567890123456789012345678901234567890123456789
            Console.WriteLine("CloudGenix Controller -- Available Commands");
            Console.WriteLine("  ?              help, this menu");
            Console.WriteLine("  q              quit");
            Console.WriteLine("  cls            clear console");
            Console.WriteLine("  logout         logout of the controller session");
            Console.WriteLine("  show <cmd>     show commands");
            Console.WriteLine("                 | token   tenant_id   versions   endpoints");
            Console.WriteLine("  get <cmd>      retrieve objects");
            Console.WriteLine("                 | contexts   sites   elements   interfaces   ifstatus");
            Console.WriteLine("                 | wans   lans   appdefs   policysets   policyrules");
            Console.WriteLine("                 | seczones   siteseczones   secpolsets   secpolrules");
            Console.WriteLine("                 | sitewanifs   topology   snmpagents");
            Console.WriteLine("  metrics <cmd>  retrieve metrics");
            Console.WriteLine("                 | clear   show   build   addmetric   addfilter   submit");
            Console.WriteLine("  topn <cmd>     retrieve top N statistics");
            Console.WriteLine("                 | clear   show   build   addfilter   submit");
            Console.WriteLine("  flows <cmd>    retrieve flows");
            Console.WriteLine("                 | clear   show   build   addfilter   submit");
            Console.WriteLine("  events <cmd>   retrieve events");
            Console.WriteLine("                 | clear   show   build   submit");
            Console.WriteLine("");
        }

        #endregion

        #region Private-API-Wrappers

        #region Get-APIs

        private static void GetContexts()
        {
            if (_Cgnx.GetContexts(out _Contexts))
            {
                Console.WriteLine("Success");
                if (_Contexts != null)
                {
                    foreach (Context curr in _Contexts)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetSites()
        {
            if (_Cgnx.GetSites(out _Sites))
            {
                Console.WriteLine("Success");
                if (_Sites != null)
                {
                    foreach (Site curr in _Sites)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetElements()
        {
            if (_Cgnx.GetElements(out _Elements))
            {
                Console.WriteLine("Success");
                if (_Elements != null)
                {
                    foreach (Element curr in _Elements)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetElementInterfaces()
        {
            if (_Cgnx.GetElementInterfaces(
                Common.InputString("Site ID    :", null, false),
                Common.InputString("Element ID :", null, false),
                out _ElementInterfaces))
            {
                Console.WriteLine("Success");
                if (_ElementInterfaces != null)
                {
                    foreach (ElementInterface curr in _ElementInterfaces)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetElementInterfaceStatus()
        {
            if (_Cgnx.GetElementInterfaceStatus(
                Common.InputString("Site ID      :", null, false),
                Common.InputString("Element ID   :", null, false),
                Common.InputString("Interface ID :", null, false),
                out _ElementInterfaceStatus))
            {
                Console.WriteLine("Success");
                if (_ElementInterfaceStatus != null)
                {
                    Console.WriteLine(_ElementInterfaceStatus.ToString());
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetWanNetworks()
        {
            if (_Cgnx.GetWanNetworks(out _WanNetworks))
            {
                Console.WriteLine("Success");
                if (_WanNetworks != null)
                {
                    foreach (WanNetwork curr in _WanNetworks)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetLanNetworks()
        {
            if (_Cgnx.GetLanNetworks(
                Common.InputString("Site ID :", null, false),
                out _LanNetworks))
            {
                Console.WriteLine("Success");
                if (_LanNetworks != null)
                {
                    foreach (LanNetwork curr in _LanNetworks)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetApplicationDefinitions()
        {
            if (_Cgnx.GetApplicationDefinitions(out _ApplicationDefinitions))
            {
                Console.WriteLine("Success");
                if (_ApplicationDefinitions != null)
                {
                    foreach (ApplicationDefinition curr in _ApplicationDefinitions)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetPolicySets()
        {
            if (_Cgnx.GetPolicySets(out _PolicySets))
            {
                Console.WriteLine("Success");
                if (_PolicySets != null)
                {
                    foreach (PolicySet curr in _PolicySets)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetPolicyRules()
        {
            if (_Cgnx.GetPolicyRules(
                Common.InputString("Policy Set ID :", null, false),
                out _PolicyRules))
            {
                Console.WriteLine("Success");
                if (_PolicyRules != null)
                {
                    foreach (PolicyRule curr in _PolicyRules)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetSecurityZones()
        {
            if (_Cgnx.GetSecurityZones(out _SecurityZones))
            {
                Console.WriteLine("Success");
                if (_SecurityZones != null)
                {
                    foreach (SecurityZone curr in _SecurityZones)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetSiteSecurityZones()
        {
            if (_Cgnx.GetSiteSecurityZones(
                Common.InputString("Site ID :", null, false),
                out _SiteSecurityZones))
            {
                Console.WriteLine("Success");
                if (_SiteSecurityZones != null)
                {
                    foreach (SiteSecurityZone curr in _SiteSecurityZones)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetSecurityPolicySets()
        {
            if (_Cgnx.GetSecurityPolicySet(out _SecurityPolicySets))
            {
                Console.WriteLine("Success");
                if (_SecurityPolicySets != null)
                {
                    foreach (SecurityPolicySet curr in _SecurityPolicySets)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetSecurityPolicyRules()
        {
            if (_Cgnx.GetSecurityPolicyRules(
                Common.InputString("Security Policy ID :", null, false),
                out _SecurityPolicyRules))
            {
                Console.WriteLine("Success");
                if (_SecurityPolicyRules != null)
                {
                    foreach (SecurityPolicyRule curr in _SecurityPolicyRules)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetSiteWanInterfaces()
        {
            if (_Cgnx.GetSiteWanInterfaces(
                Common.InputString("Site ID :", null, false),
                out _SiteWanInterfaces))
            {
                Console.WriteLine("Success");
                if (_SiteWanInterfaces != null)
                {
                    foreach (SiteWanInterface curr in _SiteWanInterfaces)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetSiteTopology()
        {
            if (_Cgnx.GetSiteTopology(
                Common.InputString("Site ID :", null, false),
                out _SiteTopology))
            {
                Console.WriteLine("Success");
                if (_SiteTopology != null)
                {
                    Console.WriteLine(Common.SerializeJson(_SiteTopology, true));
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        private static void GetSnmpAgents()
        {
            if (_Cgnx.GetSnmpAgents(
                Common.InputString("Site ID    :", null, false),
                Common.InputString("Element ID :", null, false),
                out _SnmpAgents))
            {
                Console.WriteLine("Success");
                if (_SnmpAgents != null)
                {
                    foreach (SnmpAgent curr in _SnmpAgents)
                    {
                        Console.WriteLine(curr.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("(null)");
                }
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        #endregion

        #region Metrics-APIs

        private static void ClearMetricsQuery()
        {
            _MetricsQuery = null;
            Console.WriteLine("Cleared");
        }

        private static void ShowMetricsQuery()
        {
            if (_MetricsQuery == null)
            {
                Console.WriteLine("Please build a metrics query first");
                return;
            }

            Console.WriteLine(Common.SerializeJson(_MetricsQuery, true));
        }

        private static void BuildMetricsQuery()
        {
            Console.WriteLine("Supply timestamps in the form of yyyy-MM-ddTHH:mm:ss.zzzZ");

            string startTime = Common.InputString("Start time :", null, false);
            string endTime = Common.InputString("End time   :", null, false);
            string interval = Common.InputString("Interval   :", "5min", false);
            string view = Common.InputString("View       :", null, true);

            try
            {
                _MetricsQuery = new MetricsQuery(startTime, endTime, interval, view);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return;
            }
        }

        private static void AddMetric()
        {
            if (_MetricsQuery == null)
            {
                Console.WriteLine("Please build a metrics query first");
                return;
            }

            try
            {
                string name = Common.InputString("Name       :", null, false);
                string stat = Common.InputString("Statistics :", null, false);
                string unit = Common.InputString("Unit       :", null, false);

                MetricsQuery.MetricsSettings metric = new MetricsQuery.MetricsSettings();
                metric.Name = name;
                metric.Unit = unit;
                metric.Statistics = new List<string>();
                metric.Statistics.Add(stat);

                _MetricsQuery.Metrics.Add(metric);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return;
            }
        }

        private static void AddMetricsFilter()
        {
            if (_MetricsQuery == null)
            {
                Console.WriteLine("Please build a metrics query first");
                return;
            }

            try
            {
                string appId = Common.InputString("App ID    :", null, true);
                string dir = Common.InputString("Direction :", null, true);
                string pType = Common.InputString("Path type :", null, true);
                string siteId = Common.InputString("Site ID   :", null, true);

                MetricsQuery.FilterSettings filter = new MetricsQuery.FilterSettings();

                filter.Direction = dir;

                filter.AppIds = new List<string>();
                if (!String.IsNullOrEmpty(appId)) filter.AppIds.Add(appId);

                filter.PathTypes = new List<string>();
                if (!String.IsNullOrEmpty(pType)) filter.PathTypes.Add(pType);

                filter.SiteIds = new List<string>();
                if (!String.IsNullOrEmpty(siteId)) filter.SiteIds.Add(siteId);

                _MetricsQuery.Filter = filter;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return;
            }
        }

        private static void SendMetricsQuery()
        {
            if (_MetricsQuery == null)
            {
                Console.WriteLine("Please build a metrics query first");
                return;
            }

            if (_Cgnx.GetMetrics(_MetricsQuery, out _MetricsResponse))
            {
                Console.WriteLine("Success");
                if (_MetricsResponse != null) Console.WriteLine(Common.SerializeJson(_MetricsResponse, true));
                else Console.WriteLine("(null)");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        #endregion

        #region TopN-APIs

        private static void ClearTopNQuery()
        {
            _TopNQuery = null;
            Console.WriteLine("Cleared");
        }

        private static void ShowTopNQuery()
        {
            if (_TopNQuery == null)
            {
                Console.WriteLine("Please build a topn query first");
                return;
            }

            Console.WriteLine(Common.SerializeJson(_TopNQuery, true));
        }

        private static void BuildTopNQuery()
        {
            Console.WriteLine("Supply timestamps in the form of yyyy-MM-ddTHH:mm:ss.zzzZ");

            string startTime = Common.InputString("Start time :", null, false);
            string endTime = Common.InputString("End time   :", null, false);
            string basis = Common.InputString("Basis      :", null, false);
            string topNType = Common.InputString("Type       :", "app", false);
            int limit = Common.InputInteger("Limit      :", 10, true, false);

            try
            {
                _TopNQuery = new TopNQuery(startTime, endTime, basis, topNType, limit);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return;
            }
        }

        private static void AddTopNFilter()
        {
            if (_TopNQuery == null)
            {
                Console.WriteLine("Please build a topn query first");
                return;
            }

            try
            {
                string appId = Common.InputString("App ID    :", null, true);
                string dir = Common.InputString("Direction :", null, true);
                string pType = Common.InputString("Path type :", null, true);
                string siteId = Common.InputString("Site ID   :", null, true);

                TopNQuery.FilterSettings filter = new TopNQuery.FilterSettings();

                filter.Direction = dir;

                filter.AppIds = new List<string>();
                if (!String.IsNullOrEmpty(appId)) filter.AppIds.Add(appId);

                filter.PathTypes = new List<string>();
                if (!String.IsNullOrEmpty(pType)) filter.PathTypes.Add(pType);

                filter.SiteIds = new List<string>();
                if (!String.IsNullOrEmpty(siteId)) filter.SiteIds.Add(siteId);

                _TopNQuery.Filter = filter;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return;
            }
        }

        private static void SendTopNQuery()
        {
            if (_TopNQuery == null)
            {
                Console.WriteLine("Please build a topn query first");
                return;
            }

            if (_Cgnx.GetTopN(_TopNQuery, out _TopNResponse))
            {
                Console.WriteLine("Success");
                if (_TopNResponse != null) Console.WriteLine(Common.SerializeJson(_TopNResponse, true));
                else Console.WriteLine("(null)");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        #endregion

        #region Flows-APIs

        private static void ClearFlowsQuery()
        {
            _FlowQuery = null;
            Console.WriteLine("Cleared");
        }

        private static void ShowFlowsQuery()
        {
            if (_FlowQuery == null)
            {
                Console.WriteLine("Please build a flows query first");
                return;
            }

            Console.WriteLine(Common.SerializeJson(_FlowQuery, true));
        }

        private static void BuildFlowsQuery()
        {
            Console.WriteLine("Supply timestamps in the form of yyyy-MM-ddTHH:mm:ss.zzzZ");

            string startTime = Common.InputString("Start time  :", null, false);
            string endTime = Common.InputString("End time    :", null, false);
            string debugLevel = Common.InputString("Debug level :", "all", false);

            try
            {
                _FlowQuery = new FlowQuery(startTime, endTime, debugLevel);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return;
            }
        }

        private static void AddFlowsFilter()
        {
            if (_FlowQuery == null)
            {
                Console.WriteLine("Please build a flows query first");
                return;
            }

            try
            {
                string appId = Common.InputString("App ID    :", null, true);
                string dir = Common.InputString("Direction :", null, true);
                string pType = Common.InputString("Path type :", null, true);
                string siteId = Common.InputString("Site ID   :", null, true);

                FlowQuery.FilterSettings filter = new FlowQuery.FilterSettings();

                if (!String.IsNullOrEmpty(dir)) filter.Direction = dir;

                if (!String.IsNullOrEmpty(appId))
                {
                    filter.AppIds = new List<string>();
                    filter.AppIds.Add(appId);
                }

                if (!String.IsNullOrEmpty(pType))
                {
                    filter.PathTypes = new List<string>();
                    filter.PathTypes.Add(pType);
                }

                if (!String.IsNullOrEmpty(siteId))
                {
                    filter.SiteIds = new List<string>();
                    filter.SiteIds.Add(siteId);
                }

                _FlowQuery.Filter = filter;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return;
            }
        }

        private static void SendFlowsQuery()
        {
            if (_FlowQuery == null)
            {
                Console.WriteLine("Please build a flows query first");
                return;
            }

            if (_Cgnx.GetFlows(_FlowQuery, out _FlowResponse))
            {
                Console.WriteLine("Success");
                if (_FlowResponse != null) Console.WriteLine(Common.SerializeJson(_FlowResponse, true));
                else Console.WriteLine("(null)");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        #endregion

        #region Events-APIs
         
        private static void ClearEventsQuery()
        {
            _EventQuery = null;
            Console.WriteLine("Cleared");
        }

        private static void ShowEventsQuery()
        {
            if (_EventQuery == null)
            {
                Console.WriteLine("Please build an events query first");
                return;
            }

            Console.WriteLine(Common.SerializeJson(_EventQuery, true));
        }

        private static void BuildEventsQuery()
        {
            Console.WriteLine("Supply timestamps in the form of yyyy-MM-ddTHH:mm:ss.zzzZ");

            string startTime = Common.InputString("Start time  :", null, false);
            string endTime = Common.InputString("End time    :", null, false);
            string offset = Common.InputString("Offset      :", null, true);
            string queryType = Common.InputString("Query type  :", null, true);
            bool summary = Common.InputBoolean("Summary     :", true);

            try
            {
                _EventQuery = new EventQuery(startTime, endTime, offset, queryType, summary);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return;
            }
        }

        private static void SendEventsQuery()
        {
            if (_EventQuery == null)
            {
                Console.WriteLine("Please build an events query first");
                return;
            }

            if (_Cgnx.GetEvents(_EventQuery, out _EventResponse))
            {
                Console.WriteLine("Success");
                if (_EventResponse != null) Console.WriteLine(Common.SerializeJson(_EventResponse, true));
                else Console.WriteLine("(null)");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }

        #endregion

        #endregion
    }
}
