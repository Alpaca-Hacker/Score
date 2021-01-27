using System;
using Microsoft.AspNetCore.TestHost;

namespace FunctionalTests.Extensions
{
    public static class TestHttpServerExtensions
    {
        public static Uri GetAbsoluteUri(this TestServer server, string absoluteUri)
        {
            return new Uri(server.BaseAddress + absoluteUri);
        }
    }
}
