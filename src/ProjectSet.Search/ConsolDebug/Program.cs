using ConsolDebug;
using GGroupp.Infra;
using GGroupp.Internal.Timesheet;
using PrimeFuncPack;


class Program
{
    public static async Task Main()
    {
        using var messageHandler = new DebugDelefationHandler();

        var apiClient = CreateProdApiClient(messageHandler);
        var searchClient = apiClient.UseProjectSetSearchApi().Resolve(null!);
        while (true)
        {
            var result = await searchClient.InvokeAsync(new("тес", 1000));
            var res = result.SuccessOrThrow();
            Console.WriteLine("Project Search Result");
            foreach (var proj in res.Projects)
            {
                Console.WriteLine(proj.Type.ToString());
                Console.WriteLine($"\t\"{proj.Name}\"");
                Console.WriteLine($"\t{proj.Id}");
            }
        }
    }

    //static Dependency<IDataverseApiClient> CreateApiClient(HttpMessageHandler socketsHttpHandler)
    //        =>
    //        Dependency.Of<HttpMessageHandler, IFunc<DataverseApiClientConfiguration>>(
    //            socketsHttpHandler,
    //            new ConfigProvider())
    //        .UseDataverseApiClient();
    static Dependency<IDataverseApiClient> CreateProdApiClient(HttpMessageHandler socketsHttpHandler)
            =>
            Dependency.Of<HttpMessageHandler, DataverseApiClientConfiguration>(
                socketsHttpHandler,
                new DataverseApiClientConfiguration(
            serviceUrl: "https://ggrouppru.crm4.dynamics.com/",
            authTenantId: "73bfb34f-ff97-47b8-af7b-1720168c1ef0",
            authClientId: "98768d52-2635-40e7-adfb-8a008c3293f8", 
            authClientSecret: "VYx7Q~0YithyFZYm5pHheoBt2ydF~FoRP8dFe")) 
            .UseDataverseApiClient();
}


