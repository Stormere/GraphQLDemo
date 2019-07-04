using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Subscription;
using GraphQL.Types;
using GraphQLDemo.Middleware;
using GraphQLParser.AST;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketManager;

namespace GraphQLDemo.GraphQL
{
    /// <summary>
    /// Implementation of GQL interfaces for sending and receiving ws messages.
    ///</summary>
    public class GQLWebSocketReceiver : IWebSocketReceiver
    {
        private readonly IWebSocketWriter _gqlSender;
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _documentExecuter;
        public readonly ILogger _logger;
        private readonly WebSocketConnectionManager _webSocketConnectionManager;

        public GQLWebSocketReceiver(WebSocketConnectionManager webSocketConnectionManager, ILogger<IWebSocketReceiver> logger,
        ISchema schema, IDocumentExecuter documentExecuter,
        IWebSocketWriter websocketWriter)
        {
            _gqlSender = websocketWriter;
            _schema = schema;
            _documentExecuter = documentExecuter;
            _webSocketConnectionManager = webSocketConnectionManager;
            _logger = logger;
        }

        public  Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, string serialisedPayload)
        {
            _logger.LogInformation("Received payload: " + serialisedPayload);
            var received = JsonConvert.DeserializeObject<GQLMessage>(serialisedPayload);

            switch (received.Type)
            {
                case GQLMessageTypes.CONNECTION_INIT:
                    _logger.LogInformation("Connection initialisation received. Acknowledge client.");
                    // acknowledge successful connection with client
                    return _gqlSender.SendMessageAsync(socket, new GQLMessage
                    {
                        Id = _webSocketConnectionManager.GetId(socket),
                        Type = GQLMessageTypes.CONNECTION_ACK
                    });

                case GQLMessageTypes.GQL_START:
                    _logger.LogInformation("received subscription request. adding to group.");
                    var gql = new GraphQLRequest
                    {
                        Query = received.Payload.SelectToken("$..query").ToString(),
                        Variables = received.Payload.SelectToken("$..variables").ToObject<JObject>()
                    };
                    // include socket context. can also include with usercontext if using authentication
                    object context = new
                    {
                        socketId = _webSocketConnectionManager.GetId(socket),
                        opId = received.Id
                    };
                    return this.Subscribe(gql, context);

                case GQLMessageTypes.GQL_STOP:
                    var id = _webSocketConnectionManager.GetId(socket);
                    _logger.LogInformation($"stopping subscriptions for connection {id}");
                    return _webSocketConnectionManager.RemoveSocket(id);
                default:
                    _logger.LogError("Unknown GQL Message Type received: " + received.Type);
                    return Task.CompletedTask;
            }
        }

        public async Task<SubscriptionExecutionResult> Subscribe(GraphQLRequest bdy, object context)
        {
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = bdy.Query,
                // OperationName = bdy.OperationName,
                Inputs = bdy.Variables.ToInputs(),
                ExposeExceptions = false,
                UserContext = context
            };

            // call ChatSubscription's subscriber
            var result = await _documentExecuter.ExecuteAsync(executionOptions);
            return new SubscriptionExecutionResult(result);
        }

        public Task OnConnected(WebSocket socket)
        {
            _logger.LogInformation($"Added socket connection id.");
            _webSocketConnectionManager.AddSocket(socket);
            return Task.CompletedTask;
        }

        public Task OnDisconnected(WebSocket socket)
        {
            return _webSocketConnectionManager.RemoveSocket(_webSocketConnectionManager.GetId(socket));
        }
    }
}
