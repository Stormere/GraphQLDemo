using System;
namespace GraphQLDemo.GraphQL.Subscriptions
{
    public interface ISubscription
    {
        void Resolve(GraphQLSubscription graphQLSubcription);

    }
}
