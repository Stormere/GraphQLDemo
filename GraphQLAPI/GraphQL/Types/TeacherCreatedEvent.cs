using System;
using GraphQL.Types;
using GraphQLDemo.Services;

namespace GraphQLDemo.GraphQL.Types
{
    public class TeacherCreatedEvent: ObjectGraphType<object>, IGraphQLType
    {
        private readonly ITeacherService _teacherService;

        public TeacherCreatedEvent(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
    }
}
