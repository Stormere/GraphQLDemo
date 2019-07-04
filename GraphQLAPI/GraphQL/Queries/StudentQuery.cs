using GraphQLDemo.Services;
using GraphQL.Types;
using GraphQLDemo.Types;
using GraphQLDemo.GraphQL.Types;

namespace GraphQLDemo.GraphQL.Queries
{
    public class StudentQuery : Resolver, IStudentQuery
    {
        /*
         
        {
  "operationName": "string",
  "namedQuery": "string",
  "query": "{students {statusCode data {name }}}",
  "variables": "string"
}     
        */
        private readonly IStudentService _studentService;

        public StudentQuery(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphQLQuery"></param>
        public void Resolve(GraphQLQuery graphQLQuery)
        {
            graphQLQuery.Field<ResponseListGraphType<StudentType>>(
               "students",
               resolve: context =>
               {
                   var stu = _studentService.GetStudents();
                   return Response(stu);
               }
           );
        }
    }
}
