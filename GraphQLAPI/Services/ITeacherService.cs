using System;
using System.Collections.Generic;
using GraphQLDemo.Dtos;
using GraphQLDemo.GraphQL.Types;

namespace GraphQLDemo.Services
{
    public interface ITeacherService
    {
        List<Teacher> GetTeachers();
        List<Teacher> GetTeachers(string where);

        IObservable<Teacher> WhenTeacherCreated { get; }

        Teacher Add(Teacher teacher);

    }
}
