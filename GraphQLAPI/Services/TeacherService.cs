using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDemo.Dtos;

namespace GraphQLDemo.Services
{
    public class TeacherService : ITeacherService
    {
        public Teacher Add(Teacher teacher)
        {
            return teacher;
        }

        public List<Teacher> GetTeachers()
        {
            var tags = new List<Teacher>();
            for (int i = 0; i < 10; i++)
            {
                Teacher teacher = new Teacher();
                teacher.Id = i;
                teacher.Sex = i % 2 == 0 ? "男" : "女";
                teacher.Name = "姓名" + i;
                Console.WriteLine("Test");
                tags.Add(teacher);
            }
            return tags;
        }
    }
}
