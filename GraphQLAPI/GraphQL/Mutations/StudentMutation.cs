using GraphQLDemo.GraphQL.Types;
using GraphQLDemo.Services;
using GraphQL.Types;
using GraphQLDemo.GraphQL.Queries;
using GraphQLDemo.Dtos;

namespace GraphQLDemo.GraphQL.Mutations
{
    public class StudentMutation :  IStudentMutation
    {
        /*
         {
  "operationName": "string",
  "namedQuery": "string",
  "query": "mutation ($student:StudentInput!){ createStudent(student: $student) { name } }",
  "variables": {student: {"name": "Boba Fett"}}
}
        
            
{
"operationName": "string",
"namedQuery": "string",
"query": "{ mutation {
               createStudent(student:{name:\"DDD\",sex:\"娜娜\"}) {
                  name 
                  sex
          }}}",
"variables": {}
}

        */

        private readonly IStudentService _studentService;

        public StudentMutation(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public void Resolve(GraphQLMutation mutation)
        {
            mutation.Field<StudentType>(
                "createStudent",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StudentInputType>> { Name = "student" }
                ),
                resolve: context =>
                {
                    var student = context.GetArgument<Student>("student");
                    return _studentService.Add(student);
                });
        }
    }
}
