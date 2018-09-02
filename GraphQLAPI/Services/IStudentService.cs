using System.Collections.Generic;
using GraphQLDemo.Dtos;

namespace GraphQLDemo.Services
{
    public interface IStudentService
    {
        List<Student> GetStudents();

        Student Add(Student student);

    }
}
