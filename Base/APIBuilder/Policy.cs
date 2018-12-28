using Microsoft.AspNetCore.Http;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Base.APIBuilder
{
    public static class Policy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(Message => Message.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, RetryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, RetryAttempt) * 150));
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
