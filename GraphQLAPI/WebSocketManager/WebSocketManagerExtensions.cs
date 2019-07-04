using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebSocketManager;

namespace GraphQLDemo.WebSocketManager
{
    public static class WebSocketManagerExtensions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                              PathString path,
                                                              IWebSocketReceiver handler, string acceptSubProtocol)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler, acceptSubProtocol));
        }
    }
}
