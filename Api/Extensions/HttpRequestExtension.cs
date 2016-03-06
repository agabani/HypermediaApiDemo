using System;
using Microsoft.AspNet.Http;

namespace Api.Extensions
{
    public static class HttpRequestExtension
    {
        public static Uri GetBaseAddress(this HttpRequest httpRequest)
        {
            return new Uri($"{httpRequest.Scheme}://{httpRequest.Host.Value}");
        }

        public static Uri GetAbsoluteAddress(this HttpRequest httpRequest)
        {
            return new Uri($"{httpRequest.Scheme}://{httpRequest.Host.Value}{httpRequest.Path}");
        }
    }
}