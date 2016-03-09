using Api.Extensions;
using Api.Factories;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public abstract class Module
    {
        protected readonly HttpRequest Request;
        protected ActionFactory ActionFactory;
        protected LinkFactory LinkFactory;

        protected Module(HttpRequest request)
        {
            Request = request;
            ActionFactory = new ActionFactory(request.GetBaseAddress());
            LinkFactory = new LinkFactory(request.GetBaseAddress());
        }
    }
}