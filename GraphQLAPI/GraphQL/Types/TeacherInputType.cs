using GraphQL.Types;
using GraphQLDemo.Dtos;

namespace GraphQLDemo.GraphQL.Types
{
    public class TeacherInputType : InputObjectGraphType<Teacher>, IGraphQLType
    {

        public TeacherInputType()
        {
            Name = "TeacherInput";
            Field(x => x.Name);
            Field(x => x.Sex, nullable: true);
        }
    }
}
