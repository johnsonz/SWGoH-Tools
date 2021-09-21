using Google.Protobuf;
using SWGOH.Models.GameClients;
using SWGOH.Models.Protocols;
using SWGOH.Tools.Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SWGOH.Tools.Services
{
    public class GameClientService
    {
        private readonly HttpClient _client;
        public GameClientService()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://swprod.capitalgames.com/rpc");
            client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            client.DefaultRequestHeaders.Add("Accept-Type", "application/x-protobuf");
            _client = client;
        }
        public async Task<AuthRpcDoAuthGameCenterResponse> GetUserAuthAsync(string gc)
        {
            var payload = new AuthRpcDoAuthGameCenterRequest
            {
                GamecenterId = gc,
                DevicePlatform = "IOS",
                BindleId = "com.ea.starwarscapital.bv",
                GuestId = "C2BFF9D5-762A-4388-A5E4-C3E33E2B8E9B",
                Region = "ROW",
                LocalTimeZoneOffsetMinutes = 960
            }.ToByteString();
            var response = await PostAsync("AuthRpc", "DoAuthGameCenter", payload);
            var data = await response.Content.ReadAsByteArrayAsync();

            ResponseEnvelope responseEnvelope = ResponseEnvelope.Parser.ParseFrom(data);
            if (responseEnvelope.Code != ResponseCode.Ok)
            {
                throw new Exception(string.Format("Error code: {0}, {1}", responseEnvelope.Code, responseEnvelope.Message));
            }

            AuthRpcDoAuthGameCenterResponse authRpcDoAuthGameCenterResponse = AuthRpcDoAuthGameCenterResponse.Parser.ParseFrom(responseEnvelope.Payload.ToByteArray().UnGzip());

            return authRpcDoAuthGameCenterResponse;
        }
        public async Task<StoreRpcBuyItemResponse> GetStoreRpcBuyItemAsync(UserAuthModel auth)
        {
            var payload = new StoreRpcBuyItemRequest
            {
                BuyItemName = "premium-pack-silver",
                BuyItemKey = 4,
                BuyItemUnknown = 1
            }.ToByteString();
            var response = await PostAsync("StoreRpc", "BuyItem", payload, auth);
            var data = await response.Content.ReadAsByteArrayAsync();

            ResponseEnvelope responseEnvelope = ResponseEnvelope.Parser.ParseFrom(data);
            if (responseEnvelope.Code != ResponseCode.Ok)
            {
                throw new Exception(string.Format("Error code{0}, {1}", responseEnvelope.Code, responseEnvelope.Message));
            }

            StoreRpcBuyItemResponse storeRpcBuyItemResponse = StoreRpcBuyItemResponse.Parser.ParseFrom(responseEnvelope.Payload.ToByteArray().UnGzip());
            return storeRpcBuyItemResponse;
        }
        public async Task<PlayerRpcGetInitialDataResponse> GetPlayerRpcGetInitialDataAsync(UserAuthModel auth)
        {
            var response = await PostAsync("PlayerRpc", "GetInitialData",null,auth);
            var data = await response.Content.ReadAsByteArrayAsync();

            ResponseEnvelope responseEnvelope = ResponseEnvelope.Parser.ParseFrom(data);
            if (responseEnvelope.Code != ResponseCode.Ok)
            {
                throw new Exception(string.Format("Error code{0}, {1}", responseEnvelope.Code, responseEnvelope.Message));
            }
            PlayerRpcGetInitialDataResponse playerRpcGetInitialDataResponse = PlayerRpcGetInitialDataResponse.Parser.ParseFrom(responseEnvelope.Payload.ToByteArray().UnGzip());
            return playerRpcGetInitialDataResponse;
        }
        private async Task<HttpResponseMessage> PostAsync(string serviceName, string requestMethod, ByteString payload = null, UserAuthModel auth = null)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.Method = HttpMethod.Post;
            var data = GetRequestEnvelope(serviceName, requestMethod, payload, auth);
            httpRequest.Content = new ByteArrayContent(data);
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            var response = await _client.SendAsync(httpRequest);
            return response;
        }
        private byte[] GetRequestEnvelope(string serviceName, string requestMethod, ByteString payload, UserAuthModel auth = null)
        {
            RequestEnvelope message = new RequestEnvelope();
            if (payload != (ByteString)null)
                message.Payload = payload;
            if (auth != null)
            {
                message.AuthId = auth.PlayerId;
                message.AuthToken = auth.Token;
            }
            message.CorrelationId = 0;
            message.ServiceName = serviceName;
            message.MethodName = requestMethod;
            message.ClientVersion = 786537;
            long num = (long)(Math.Floor((double)DateTime.Now.Ticks / 1000.0) - 10.0);
            message.ClientStartupTimestamp = num;
            message.Platform = "IOS";
            message.Region = "ROW";
            message.ClientExternalVersion = "99.99.99";
            message.ClientInternalVersion = "99.99.99";
            message.RequestId = Guid.NewGuid().ToString().ToLower();
            message.AcceptEncoding = AcceptEncoding.Gzipacceptencoding;
            message.CurrentClientTime = num + 8L;
            message.NetworkAccess = "W";

            using (MemoryStream memoryStream = new MemoryStream())
            {
                message.WriteTo((Stream)memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
