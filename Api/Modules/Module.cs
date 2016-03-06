using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public abstract class Module
    {
        protected readonly HttpRequest Request;

        protected Module(HttpRequest request)
        {
            Request = request;
        }
    }
}