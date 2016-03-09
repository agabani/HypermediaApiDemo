using System;
using Api.Extensions;
using Api.Factories;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public abstract class Module
    {
        protected readonly HttpRequest Request;
        protected Uri BaseAddress;
        protected LinkFactory LinkFactory;

        protected Module(HttpRequest request)
        {
            Request = request;
            BaseAddress = request.GetBaseAddress();
            LinkFactory = new LinkFactory(BaseAddress);
        }
    }
}