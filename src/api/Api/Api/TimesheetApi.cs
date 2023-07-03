using System;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class TimesheetApi : ITimesheetApi
{
    private static readonly FlatArray<string> ProjectTypeEntityNames
        =
        new(ProjectJson.EntityName, LeadJson.EntityName, OpportunityJson.EntityName, IncidentJson.EntityName);

    private readonly IDataverseApiClient dataverseApiClient;

    private readonly ITodayProvider todayProvider;

    private readonly TimesheetApiOption option;

    internal TimesheetApi(IDataverseApiClient dataverseApiClient, ITodayProvider todayProvider, TimesheetApiOption option)
    {
        this.dataverseApiClient = dataverseApiClient;
        this.todayProvider = todayProvider;
        this.option = option;
    }

    private IDataverseApiClient GetDataverseApiClient(Guid? callerId)
        =>
        callerId is null ? dataverseApiClient : dataverseApiClient.Impersonate(callerId.Value);
}