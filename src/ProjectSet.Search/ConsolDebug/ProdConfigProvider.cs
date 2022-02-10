using GGroupp.Infra;

namespace ConsolDebug;

internal class ProdConfigProvider : IFunc<DataverseApiClientConfiguration>
{
    public DataverseApiClientConfiguration Invoke()
        =>
        new(
            serviceUrl: "https://ggrouppru.crm4.dynamics.com/",
            authTenantId: "73bfb34f-ff97-47b8-af7b-1720168c1ef0",
            authClientId: "98768d52-2635-40e7-adfb-8a008c3293f8", // 98768d52-2635-40e7-adfb-8a008c3293f8
            authClientSecret: "VYx7Q~0YithyFZYm5pHheoBt2ydF~FoRP8dFe"); // VYx7Q~0YithyFZYm5pHheoBt2ydF~FoRP8dFe
}
