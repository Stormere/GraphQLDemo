using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDemo.Dtos;

namespace GraphQLDemo.Services
{
    public class StudentService : IStudentService
    {
        public Student Add(Student student)
        {
            return student;
        }

        public List<Student> GetStudents()
        {
            var tags = new List<Student>();
            for (int i = 0; i < 10; i++)
            {
                Student student = new Student();
                student.Id = i;
                student.Sex = i % 2 == 0 ? "男" : "女";
                student.Name = "姓名" + i;
                Console.WriteLine("Test");
                tags.Add(student);
            }
            return tags;
        }
    }
}
