using GGroupp.Infra;

namespace ConsolDebug
{
    class ConfigProvider : IFunc<DataverseApiClientConfiguration>
    {
        public DataverseApiClientConfiguration Invoke()
        => new(
                    serviceUrl: "https://ggstage.crm4.dynamics.com/",
                    authTenantId: "73bfb34f-ff97-47b8-af7b-1720168c1ef0",
                    authClientId: "5a3c53e6-3198-43f2-b7ba-dc56a5545007",
                    authClientSecret: "uqG7Q~N-yD4ZrTI5bi5udJxSdN_j5Lu2reEYE"
                );
        //=> new (
        //            serviceUrl: "https://ggstage.crm4.dynamics.com/",
        //            authTenantId: "73bfb34f-ff97-47b8-af7b-1720168c1ef0",
        //            authClientId: "5a3c53e6-3198-43f2-b7ba-dc56a5545007",
        //            authClientSecret: "0m5_d-G6QJkG07K9TLA6JOUF55uaT_r1.Q"
        //        );
    }
}
