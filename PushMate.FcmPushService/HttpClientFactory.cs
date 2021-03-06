﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PushMate.FcmPushService
{
    public interface IHttpClientFactory
    {
        HttpClient Create(string serverKey, string senderId);
    }

    public class DefaultHttpClient : IHttpClientFactory
    {
        public HttpClient Create(string serverKey, string senderId)
        {
            if (string.IsNullOrWhiteSpace(serverKey))
                throw new ArgumentNullException(nameof(serverKey));

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={serverKey}");

            if (!String.IsNullOrEmpty(senderId))
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={senderId}");
            }

            return httpClient;
        }
    }

    public class MockedHttpClientOK : IHttpClientFactory
    {
        public HttpClient Create(string serverKey, string senderId)
        {
            if (string.IsNullOrWhiteSpace(serverKey))
                throw new ArgumentNullException(nameof(serverKey));

            var httpClient = new HttpClientMock();
            
            return httpClient;
        }

    }
    public class HttpClientMock : HttpClient
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            string validResponseJson = "{\"multicast_id\":4674514773536739316,\"success\":1,\"failure\":0,\"canonical_ids\":0,\"results\":[{\"message_id\":\"0:1529817872903218 % 3a7f5afa3a7f5afa\"}]}";
            response.Content = new StringContent(validResponseJson);

            return response;

        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}

