namespace GraphQLDemo.GraphQL.Queries
{
    public interface IQuery
    {
        void Resolve(GraphQLQuery graphQLQuery);
    }

}
