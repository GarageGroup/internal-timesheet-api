using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolDebug
{
    internal class DebugDelefationHandler : DelegatingHandler
    {
        public DebugDelefationHandler()
        {
            InnerHandler = new HttpClientHandler();
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var str = await request.Content.ReadAsStringAsync();

            Console.WriteLine($"Request hedeaders:\n{string.Join("\n", request.Headers.Select(kv => $"({kv.Key}, {kv.Value.FirstOrAbsent()})"))}\n");
            Console.WriteLine($"Request body:\n{str}\n\n");

            //request.Headers.Add("MSCRMCallerID", new Guid("571c0986-2873-ec11-8943-00224882fdc").ToString("D", CultureInfo.InvariantCulture));
            var response = await base.SendAsync(request, cancellationToken);
            Console.WriteLine($"Response headers:\n{string.Join("\n", response.Headers.Select(kv => $"({ kv.Key}, { kv.Value.FirstOrAbsent()})"))}\n");
            Console.WriteLine($"Response body:\n{await response.Content.ReadAsStringAsync()}\n\n");
            if (response.IsSuccessStatusCode is false)
                ;
            return response;
        }
    }
}
