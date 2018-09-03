using GraphQLDemo.Services;
using GraphQLDemo.Types;
using GraphQLDemo.GraphQL.Types;

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
               resolve: context =>
               {
                   var stu = _teacherService.GetTeachers();
                   return Response(stu);
               }
           );
        }
    }
}
