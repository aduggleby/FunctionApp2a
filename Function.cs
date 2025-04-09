using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace FunctionApp2a
{
    public class Function
    {
        private readonly ILogger<Function> _logger;

        public Function(ILogger<Function> logger)
        {
            _logger = logger;
        }

        [Function("Function")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var output = new StringBuilder();

            // DNS Lookup for "server.test.internal"
            try
            {
                var serverTestInternalIPs = Dns.GetHostAddresses("server.test.internal");
                output.AppendLine("IP addresses for server.test.internal:");
                foreach (var ip in serverTestInternalIPs)
                {
                    output.AppendLine(ip.ToString());
                }
            }
            catch (Exception ex)
            {
                output.AppendLine($"Error looking up server.test.internal: {ex.Message}");
            }

            // DNS Lookup for "example.com"
            try
            {
                var exampleComIPs = Dns.GetHostAddresses("example.com");
                output.AppendLine("IP addresses for example.com:");
                foreach (var ip in exampleComIPs)
                {
                    output.AppendLine(ip.ToString());
                }
            }
            catch (Exception ex)
            {
                output.AppendLine($"Error looking up example.com: {ex.Message}");
            }

            // HTTP GET request to "https://example.com/"
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://example.com/");
                response.EnsureSuccessStatusCode();
                var htmlContent = await response.Content.ReadAsStringAsync();
                output.AppendLine("HTML content from https://example.com/:");
                output.AppendLine(htmlContent);
            }
            catch (Exception ex)
            {
                output.AppendLine($"Error fetching HTML from https://example.com/: {ex.Message}");
            }

            return new OkObjectResult(output.ToString());
        }
    }
}
