using GraphQL.Types;
using GraphQLDemo.Dtos;
using GraphQLDemo.GraphQL.Types;
using GraphQLDemo.Services;

namespace GraphQLDemo.GraphQL.Mutations
{
    public class TeacherMutation :  ITeacherMutation
    {
        /*
         {
  "operationName": "string",
  "namedQuery": "string",
  "query": "mutation ($teacher:TeacherInput!){ createTeacher(teacher: $teacher) { name } }",
  "variables": {teacher: {"name": "Boba Fett"}}
}
        
            
{
"operationName": "string",
"namedQuery": "string",
"query": "{ mutation {
               createTeacher(teacher:{name:\"DDD\",sex:\"娜娜\"}) {
                  name 
                  sex
          }}}",
"variables": {}
}

        */

        private readonly ITeacherService _teacherService;

        public TeacherMutation(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        public void Resolve(GraphQLMutation mutation)
        {
            mutation.Field<TeacherType>(
                "createTeacher",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TeacherInputType>> { Name = "teacher" }
                ),
                resolve: context =>
                {
                    var teacher = context.GetArgument<Teacher>("teacher");
                    return _teacherService.Add(teacher);
                });
        }
    }
}
