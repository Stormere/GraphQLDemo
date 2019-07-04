using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebSocketManager
{
    /// <summary>
    // holds sockets and assigns a uuid to each socket. this should be persisted against users
    /// </summary>
    public class WebSocketConnectionManager
    {
        public WebSocketConnectionManager(ILogger<WebSocketConnectionManager> logger)
        {
            _logger = logger;
        }
        private Dictionary<string, WebSocket> _sockets = new Dictionary<string, WebSocket>();
        private Dictionary<string, List<string>> _groups = new Dictionary<string, List<string>>();
        private readonly ILogger<WebSocketConnectionManager> _logger;

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public Dictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public void AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
        }

        public Task RemoveSocket(string id)
        {
            WebSocket socket; string socketOp;
            // remove from all groups
            foreach (string k in _groups.Keys)
            {
                socketOp = _groups[k].Where(s => s.Contains(id)).FirstOrDefault();
                if (!string.IsNullOrEmpty(socketOp))
                {
                    _groups.Remove(socketOp);
                }
            }

            // remove from list of all sockets
            _sockets.Remove(id, out socket);

            return socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }

        // operations
        public void AddToGroup(string socketId, string opId, string groupName)
        {
            if (string.IsNullOrWhiteSpace(socketId) | string.IsNullOrWhiteSpace(opId))
            {
                _logger.LogError("Socket Id and Operation Id not provided.");
                throw new Exception("no socketId or opId");
            }

            // does this groupname exist?
            var exists = _groups.ContainsKey(groupName);
            if (!exists)
            {
                _groups.Add(groupName, new List<string> { socketId + "~" + opId });
            }
            else
            {
                var existingList = _groups.GetValueOrDefault(groupName);
                existingList.Add(socketId + "~" + opId);
                _groups[groupName] = existingList;
            }
            _logger.LogInformation($"Added {socketId}, opId {opId} to group {groupName}");
        }

        public List<string> SocketOperationsInGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                var all = new List<string>();
                foreach (string k in _groups.Keys)
                {
                    all.AddRange(_groups.GetValueOrDefault(k));
                }
                return all.Distinct().ToList();
            }
            return _groups.GetValueOrDefault(groupName);
        }
    }

    /// <summary>
    /// receives  communication events from client sockets. Note that this does not implement a writer
    ///</summary>
    public interface  IWebSocketReceiver
    {
        Task OnConnected(WebSocket socket);
        Task OnDisconnected(WebSocket socket);
        Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, string serialisedPayload);
    }

    public  interface IWebSocketWriter
    {
        Task SendMessageAsync(WebSocket socket, object message);
        Task SendToGroup(string groupName, object message);
    }


    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private IWebSocketReceiver _webSocketReceiver { get; set; }

        private readonly ILogger<WebSocketManagerMiddleware> _logger;

        public string _acceptSubProtocol { get; set; }

        public WebSocketManagerMiddleware(RequestDelegate next, ILogger<WebSocketManagerMiddleware> logger,
                                          IWebSocketReceiver webSocketReceiver, string acceptSubProtocol)
        {
            _next = next;
            _webSocketReceiver = webSocketReceiver;
            _logger = logger;
            _acceptSubProtocol = acceptSubProtocol;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync(_acceptSubProtocol).ConfigureAwait(false);
            await _webSocketReceiver.OnConnected(socket).ConfigureAwait(false);

            await Receive(socket, async (result, serializedInvocationDescriptor) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await _webSocketReceiver.ReceiveAsync(socket, result, serializedInvocationDescriptor).ConfigureAwait(false);
                    return;
                }

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocketReceiver.OnDisconnected(socket);
                }
            });
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, string> handleMessage)
        {
            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024 * 4]);
                string serializedInvocationDescriptor = null;
                WebSocketReceiveResult result = null;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    }
                    while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);

                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        serializedInvocationDescriptor = await reader.ReadToEndAsync().ConfigureAwait(false);
                    }
                }
                _logger.LogInformation(JsonConvert.SerializeObject(result));
                handleMessage(result, serializedInvocationDescriptor);
            }
        }
    }


}