using System.Collections.Generic;
using GraphQLDemo.Dtos;

namespace GraphQLDemo.Services
{
    public interface ITeacherService
    {
        List<Teacher> GetTeachers();

        Teacher Add(Teacher teacher);

    }
}
