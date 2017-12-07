using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RestWrapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CloudGenix.Classes
{
    public class FlowResponse
    {
        #region Public-Members

        [JsonProperty("flows")]
        public FlowCollection Flows { get; set; }

        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        public FlowResponse()
        {
        }

        #endregion

        #region Public-Methods

        public override string ToString()
        {
            return string.Format("[FlowResponse: Flows={0}]", Flows);
        }

        #endregion

        #region Private-Methods

        #endregion

        #region Public-Embedded-Classes

        public class FlowCollection
        {
            [JsonProperty("debug_level")]
            public string DebugLevel { get; set; }

            [JsonProperty("start_time")]
            public DateTime? StartTime { get; set; }

            [JsonProperty("end_time")]
            public DateTime? EndTime { get; set; }

            [JsonProperty("items")]
            public List<Flow> Items { get; set; }

            public FlowCollection()
            {
                Items = new List<Flow>(); 
            }

            public override string ToString()
            {
                return string.Format("[FlowCollection: DebugLevel={0}, StartTime={1}, EndTime={2}, Items={3}]", DebugLevel, StartTime, EndTime, Items);
            }

            public class Flow
            {
                [JsonProperty("app_id")]
                public string AppId { get; set; }

                [JsonProperty("average_ntt")]
                public object AverageNetworkTransferTime { get; set; }

                [JsonProperty("average_pg")]
                public object AveragePathGoodput { get; set; }

                [JsonProperty("average_rtt")]
                public object AverageResponseTime { get; set; }

                [JsonProperty("average_srt")]
                public object AverageServerResponseTime { get; set; }

                [JsonProperty("average_udp_trt")]
                public object AverageUdpTransactionResponseTime { get; set; }

                [JsonProperty("avg_jitter_c2s")]
                public object AverageJitterClientToServer { get; set; }

                [JsonProperty("avg_jitter_s2c")]
                public object AverageJitterServerToClient { get; set; }

                [JsonProperty("avg_mos_c2s")]
                public object AverageMosClientToServer { get; set; }

                [JsonProperty("avg_mos_s2c")]
                public object AverageMosServerToClient { get; set; }

                [JsonProperty("avg_packet_loss_c2s")]
                public object AveragePacketLossClientToServer { get; set; }

                [JsonProperty("avg_packet_loss_s2c")]
                public object AveragePacketLossServerToClient { get; set; }

                [JsonProperty("bytes_c2s")]
                public long BytesClientToServer { get; set; }

                [JsonProperty("bytes_s2c")]
                public long BytesServerToClient { get; set; }

                [JsonProperty("codec_c2s")]
                public List<object> CodecClientToServer { get; set; }

                [JsonProperty("codec_s2c")]
                public List<object> CodecServerToClient { get; set; }

                [JsonProperty("destination_ip")]
                public string DestinationIp { get; set; }

                [JsonProperty("destination_port")]
                public int DestinationPort { get; set; }

                [JsonProperty("fc_app_id")]
                public string FlowControllerAppId { get; set; }

                [JsonProperty("fin_c2s")]
                public int TcpFinClientToServer { get; set; }

                [JsonProperty("fin_s2c")]
                public int TcpFinServerToClient { get; set; }

                [JsonProperty("flow_decision_metadata_list")]
                public List<object> FlowDecisionMetadataList { get; set; }

                [JsonProperty("flow_end_time_ms")]
                public long FlowEndTimeMs { get; set; }

                [JsonProperty("flow_start_time_ms")]
                public long FlowStartTimeMs { get; set; }

                [JsonProperty("incomplete_transactions")]
                public object IncompleteTransactions { get; set; }

                [JsonProperty("init_success")]
                public object InitSuccess { get; set; }

                [JsonProperty("is_local_traffic")]
                public bool? IsLocalTraffic { get; set; }

                [JsonProperty("is_security_policy_present")]
                public bool? SecurityPolicyPresent { get; set; }

                [JsonProperty("lan_to_wan")]
                public bool? LanToWan { get; set; }

                [JsonProperty("max_jitter_c2s")]
                public object MaxJitterClientToServer { get; set; }

                [JsonProperty("max_jitter_s2c")]
                public object MaxJitterServerToClient { get; set; }

                [JsonProperty("max_mos_c2s")]
                public object MaxMosClientToServer { get; set; }

                [JsonProperty("max_mos_s2c")]
                public object MaxMosServerToClient { get; set; }

                [JsonProperty("max_ntt")]
                public object MaxNetworkTransferTime { get; set; }

                [JsonProperty("max_packet_loss_c2s")]
                public object MaxPacketLossClientToServer { get; set; }

                [JsonProperty("max_packet_loss_s2c")]
                public object MaxPacketLossServerToClient { get; set; }

                [JsonProperty("max_pg")]
                public object MaxPacketGap { get; set; }

                [JsonProperty("max_rtt")]
                public object MaxRoundTripTime { get; set; }

                [JsonProperty("max_srt")]
                public object MaxServerResponseTime { get; set; }

                [JsonProperty("max_udp_trt")]
                public object MaxUdpTransactionResponseTime { get; set; }

                [JsonProperty("media_type")]
                public object MediaType { get; set; }

                [JsonProperty("min_mos_c2s")]
                public object MinMosClientToServer { get; set; }

                [JsonProperty("min_mos_s2c")]
                public object MinMosServerToClient { get; set; }

                [JsonProperty("min_ntt")]
                public object MinNetworkTransferTime { get; set; }

                [JsonProperty("min_pg")]
                public object MinPacketGap { get; set; }

                [JsonProperty("min_rtt")]
                public object MinRoundTripTime { get; set; }

                [JsonProperty("min_srt")]
                public object MinServerResponseTime { get; set; }

                [JsonProperty("min_udp_trt")]
                public object MinUdpTransactionResponseTime { get; set; }

                [JsonProperty("new_flow")]
                public bool? NewFlow { get; set; }

                [JsonProperty("ooo_pkts_c2s")]
                public long OutOfOrderPacketsClientToServer { get; set; }

                [JsonProperty("ooo_pkts_s2c")]
                public long OutOfOrderPacketsServerToClient { get; set; }

                [JsonProperty("packets_c2s")]
                public long PacketsClientToServer { get; set; }

                [JsonProperty("packets_s2c")]
                public long PacketsServerToClient { get; set; }

                [JsonProperty("path_id")]
                public string PathId { get; set; }

                [JsonProperty("path_type")]
                public string PathType { get; set; }

                [JsonProperty("policy_id")]
                public string PolicyId { get; set; }

                [JsonProperty("priority_class")]
                public int PriorityClass { get; set; }

                [JsonProperty("protocol")]
                public int Protocol { get; set; }

                [JsonProperty("reset_c2s")]
                public long ResetClientToServer { get; set; }

                [JsonProperty("reset_s2c")]
                public long ResetServerToClient { get; set; }

                [JsonProperty("retransmit_bytes_c2s")]
                public long RetransmitBytesClientToServer { get; set; }

                [JsonProperty("retransmit_bytes_s2c")]
                public long RetransmitBytesServerToClient { get; set; }

                [JsonProperty("retransmit_pkts_c2s")]
                public long RetransmitPacketsClientToServer { get; set; }

                [JsonProperty("retransmit_pkts_s2c")]
                public long RetransmitPacketsServerToClient { get; set; }

                [JsonProperty("sack_pkts_c2s")]
                public long SelectiveAckPacketsClientToServer { get; set; }

                [JsonProperty("sack_pkts_s2c")]
                public long SelectiveAckPacketsServerToClient { get; set; }

                [JsonProperty("source_ip")]
                public string SourceIp { get; set; }

                [JsonProperty("source_port")]
                public int SourcePort { get; set; }

                [JsonProperty("success_transactions")]
                public object SuccessTransactions { get; set; }

                [JsonProperty("syn_c2s")]
                public long SynClientToServer { get; set; }

                [JsonProperty("syn_s2c")]
                public long SynServerToClient { get; set; }

                [JsonProperty("traffic_type")]
                public string TrafficType { get; set; }

                [JsonProperty("unknown_domain")]
                public object UnknownDomain { get; set; }

                [JsonProperty("wan_path_change_reason")]
                public object WanPathChangeReason { get; set; }

                public Flow()
                {
                    
                }

                public override string ToString()
                {
                    return string.Format("[Flow: AppId={0}, AverageNetworkTransferTime={1}, AveragePathGoodput={2}, AverageResponseTime={3}, AverageServerResponseTime={4}, AverageUdpTransactionResponseTime={5}, AverageJitterClientToServer={6}, AverageJitterServerToClient={7}, AverageMosClientToServer={8}, AverageMosServerToClient={9}, AveragePacketLossClientToServer={10}, AveragePacketLossServerToClient={11}, BytesClientToServer={12}, BytesServerToClient={13}, CodecClientToServer={14}, CodecServerToClient={15}, DestinationIp={16}, DestinationPort={17}, FlowControllerAppId={18}, TcpFinClientToServer={19}, TcpFinServerToClient={20}, FlowDecisionMetadataList={21}, FlowEndTimeMs={22}, FlowStartTimeMs={23}, IncompleteTransactions={24}, InitSuccess={25}, IsLocalTraffic={26}, SecurityPolicyPresent={27}, LanToWan={28}, MaxJitterClientToServer={29}, MaxJitterServerToClient={30}, MaxMosClientToServer={31}, MaxMosServerToClient={32}, MaxNetworkTransferTime={33}, MaxPacketLossClientToServer={34}, MaxPacketLossServerToClient={35}, MaxPacketGap={36}, MaxRoundTripTime={37}, MaxServerResponseTime={38}, MaxUdpTransactionResponseTime={39}, MediaType={40}, MinMosClientToServer={41}, MinMosServerToClient={42}, MinNetworkTransferTime={43}, MinPacketGap={44}, MinRoundTripTime={45}, MinServerResponseTime={46}, MinUdpTransactionResponseTime={47}, NewFlow={48}, OutOfOrderPacketsClientToServer={49}, OutOfOrderPacketsServerToClient={50}, PacketsClientToServer={51}, PacketsServerToClient={52}, PathId={53}, PathType={54}, PolicyId={55}, PriorityClass={56}, Protocol={57}, ResetClientToServer={58}, ResetServerToClient={59}, RetransmitBytesClientToServer={60}, RetransmitBytesServerToClient={61}, RetransmitPacketsClientToServer={62}, RetransmitPacketsServerToClient={63}, SelectiveAckPacketsClientToServer={64}, SelectiveAckPacketsServerToClient={65}, SourceIp={66}, SourcePort={67}, SuccessTransactions={68}, SynClientToServer={69}, SynServerToClient={70}, TrafficType={71}, UnknownDomain={72}, WanPathChangeReason={73}]", AppId, AverageNetworkTransferTime, AveragePathGoodput, AverageResponseTime, AverageServerResponseTime, AverageUdpTransactionResponseTime, AverageJitterClientToServer, AverageJitterServerToClient, AverageMosClientToServer, AverageMosServerToClient, AveragePacketLossClientToServer, AveragePacketLossServerToClient, BytesClientToServer, BytesServerToClient, CodecClientToServer, CodecServerToClient, DestinationIp, DestinationPort, FlowControllerAppId, TcpFinClientToServer, TcpFinServerToClient, FlowDecisionMetadataList, FlowEndTimeMs, FlowStartTimeMs, IncompleteTransactions, InitSuccess, IsLocalTraffic, SecurityPolicyPresent, LanToWan, MaxJitterClientToServer, MaxJitterServerToClient, MaxMosClientToServer, MaxMosServerToClient, MaxNetworkTransferTime, MaxPacketLossClientToServer, MaxPacketLossServerToClient, MaxPacketGap, MaxRoundTripTime, MaxServerResponseTime, MaxUdpTransactionResponseTime, MediaType, MinMosClientToServer, MinMosServerToClient, MinNetworkTransferTime, MinPacketGap, MinRoundTripTime, MinServerResponseTime, MinUdpTransactionResponseTime, NewFlow, OutOfOrderPacketsClientToServer, OutOfOrderPacketsServerToClient, PacketsClientToServer, PacketsServerToClient, PathId, PathType, PolicyId, PriorityClass, Protocol, ResetClientToServer, ResetServerToClient, RetransmitBytesClientToServer, RetransmitBytesServerToClient, RetransmitPacketsClientToServer, RetransmitPacketsServerToClient, SelectiveAckPacketsClientToServer, SelectiveAckPacketsServerToClient, SourceIp, SourcePort, SuccessTransactions, SynClientToServer, SynServerToClient, TrafficType, UnknownDomain, WanPathChangeReason);
                }
            }
        }

        #endregion
    }
}
