using GraphQL.Types;
using GraphQLDemo.Dtos;

namespace GraphQLDemo.GraphQL.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class StudentType : ObjectGraphType<Student>, IGraphQLType
    {
        /// <summary>
        /// 
        /// </summary>
        public StudentType()
        {

            Field(x => x.Id).Description("学生ID");
            Field(x => x.Name).Description("学生姓名");
            Field(x => x.Sex).Description("学生性别");
        }
    }
}
