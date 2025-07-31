using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Base;
using System;
using System.Runtime.Serialization;

[DataContract]
public class SummaryDTO
{
    [DataMember]
    public PersonBaseDTO Holder { get; set; }

    [DataMember]
    public BaseCurrencyDTO BaseCurrencyDTO { get; set; }

    [DataMember]
    public DateTime IssuanceDate { get; set; }

    [DataMember]
    public DateTime FromDate { get; set; }

    [DataMember]
    public DateTime ToDate { get; set; }

    [DataMember]
    public string ConduitName { get; set; }

    [DataMember]
    public PersonBaseDTO MainAgent { get; set; }

    [DataMember]
    public string CurrentPlan { get; set; }
}