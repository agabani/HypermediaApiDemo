using System;
using Api.Extensions;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public abstract class Module
    {
        protected readonly HttpRequest Request;
        protected Uri BaseAddress;

        protected Module(HttpRequest request)
        {
            Request = request;
            BaseAddress = request.GetBaseAddress();
        }
    }
}