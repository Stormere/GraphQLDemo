using GraphQL.Types;
using GraphQLDemo.Dtos;

namespace GraphQLDemo.GraphQL.Types
{
    public class StudentInputType : InputObjectGraphType<Student>, IGraphQLType
    {

        public StudentInputType()
        {
            Name = "StudentInput";
            Field(x => x.Name);
            Field(x => x.Sex, nullable: true);
        }
    }
}
