using GraphQL;
using GraphQL.Types;

namespace GraphQLDemo.GraphQL
{
    public class GraphQLSchema : Schema
    {
        //public GraphQLSchema(Func<Type, GraphType> resolveType)
        //    : base(resolveType)
        //{
        //    Query = (GraphQLQuery)resolveType(typeof(GraphQLQuery));
        //}

        public GraphQLSchema(IDependencyResolver resolver) :base(resolver)
        {
            Query = resolver.Resolve<GraphQLQuery>();
            Mutation = resolver.Resolve<GraphQLMutation>();
        }

    }
}