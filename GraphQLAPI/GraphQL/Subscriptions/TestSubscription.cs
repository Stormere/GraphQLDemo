using GraphQL.Types;
using GraphQLDemo.Dtos;
using GraphQLDemo.GraphQL.Types;
using GraphQLDemo.Services;
using Newtonsoft.Json.Linq;

namespace GraphQLDemo.GraphQL.Subscriptions
{
    public class TestSubscription: ObjectGraphType<object>,ITestSubscription
    {
        private readonly ITeacherService _teacherService;


        public TestSubscription(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public void Resolve(GraphQLSubscription graphQLSubcription)
        {
            graphQLSubcription.Name = "Subscription";
            graphQLSubcription.Description = "The subscription type, represents all updates can be pushed to the client in real time over web sockets.";
            graphQLSubcription.FieldSubscribe<TeacherType>(
                name: "teacherSubcribe",
                description: "Subscribe to human created events",
                arguments: new QueryArguments {
                    new QueryArgument<StringGraphType>{Name = "teacher"}
                },
                resolve: context =>
                {
                    return context.Source as Teacher;
                },
                subscribe: context =>
                {
                    var argument = context.GetArgument<string>("teacher");
                    var dd = JObject.FromObject(context.UserContext);
                    string socketId = dd.SelectToken("$..socketId").ToString();
                    string opId = dd.SelectToken("$..opId").ToString();
                    return null;
                }
                );
        }
    }
}
