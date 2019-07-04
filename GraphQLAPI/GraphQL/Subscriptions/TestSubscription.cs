using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using GraphQL.Resolvers;
using GraphQL.Types;
using GraphQLDemo.Dtos;
using GraphQLDemo.GraphQL.Types;
using GraphQLDemo.Services;
using Newtonsoft.Json.Linq;
using WebSocketManager;

namespace GraphQLDemo.GraphQL.Subscriptions
{
    public class TestSubscription: ObjectGraphType<object>,ITestSubscription
    {
        private readonly ITeacherService _teacherService;
        private readonly WebSocketConnectionManager _webSocketConnectionManager;


        public TestSubscription(ITeacherService teacherService, WebSocketConnectionManager webSocketConnectionManager)
        {
            _teacherService = teacherService;
            _webSocketConnectionManager = webSocketConnectionManager;
        }

        public void Resolve(GraphQLSubscription graphQLSubcription)
        {
            Name = "subscription";
            AddField(new EventStreamFieldType
            {
                Name = "joinChat",
                Arguments = new QueryArguments {
                    new QueryArgument<StringGraphType> { Name = "teacher" }
                },
                Resolver = new FuncFieldResolver<Teacher>(x =>
                {
                    var message = x.Source as Teacher;
                    System.Console.WriteLine(x.Source);
                    return message;
                }),
                Subscriber = new EventStreamResolver<Teacher>(x =>
                {
                    var grp = x.GetArgument<string>("groupName");
                    if (string.IsNullOrWhiteSpace(grp))
                    {
                        grp = "all";
                    }
                    var context = JObject.FromObject(x.UserContext);
                    string socketId = context.SelectToken("$..socketId").ToString();
                    string opId = context.SelectToken("$..opId").ToString();
                    // connectionManager.AddToGroup(socketId, opId, grp);
                    return null;
                })
            });
        }

            //graphQLSubcription.Name = "Subscription";
            //graphQLSubcription.Description = "The subscription type, represents all updates can be pushed to the client in real time over web sockets.";
            //graphQLSubcription.FieldSubscribe<TeacherType>(
                //name: "teacherSubcribe",
                //description: "Subscribe to human created events",
                //arguments: new QueryArguments {
                //    new QueryArgument<StringGraphType>{Name = "teacher"}
                //},
                //resolve: context =>
                //{
                //    return context.Source as Teacher;
                //},
                //subscribe: context =>
                //{
                //    var argument = context.GetArgument<string>("teacher");
                //    var dd = JObject.FromObject(context.UserContext);
                //    string socketId = dd.SelectToken("$..socketId").ToString();
                //    string opId = dd.SelectToken("$..opId").ToString();
                //    _webSocketConnectionManager.AddToGroup(socketId, opId, argument);
                //    return null;
                //    // return _teacherService.WhenTeacherCreated.Where(x => x.Name == null || argument.Contains(x.Name));
                //}
                //);

           
        //}
    }
}
