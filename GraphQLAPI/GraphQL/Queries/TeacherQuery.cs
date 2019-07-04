using GraphQL.Types;
using GraphQLDemo.GraphQL.Types;
using GraphQLDemo.Services;
using GraphQLDemo.Types;

namespace GraphQLDemo.GraphQL.Queries
{
    public class TeacherQuery : Resolver, ITeacherQuery
    {
        /*
         
        {
  "operationName": "string",
  "namedQuery": "string",
  "query": "{teachers {statusCode data {name }}}",
  "variables": "string"
}     
        */
        private readonly ITeacherService _teacherService;

        public TeacherQuery(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphQLQuery"></param>
        public void Resolve(GraphQLQuery graphQLQuery)
        {
            graphQLQuery.Field<ResponseListGraphType<TeacherType>>(
               "teachers",
               arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "where" }),
               resolve: context =>
               {
                   var where = context.GetArgument<string>("where");
                   System.Console.WriteLine(where);
                   var stu = _teacherService.GetTeachers(where);
                   return Response(stu);
               }
           );
        }
    }
}
