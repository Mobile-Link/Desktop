using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MobileLink_Desktop.Utils;

namespace MobileLink_Desktop.Service.Interceptors;

public class TokenInterceptor : DelegatingHandler
{
    public TokenInterceptor()
    {
        InnerHandler = new HttpClientHandler(); 
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var storageContent = new LocalStorage().GetStorage();
        if (storageContent?.Token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", storageContent?.Token);
        }
 
        var response = await base.SendAsync(request, cancellationToken);
        return response;
    }
}