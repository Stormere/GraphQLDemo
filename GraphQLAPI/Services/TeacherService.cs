using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using GraphQLDemo.Dtos;

namespace GraphQLDemo.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly Subject<Teacher> whenTeacherCreated;
        public TeacherService()
        {
            this.whenTeacherCreated = new Subject<Teacher>();
        }

        public IObservable<Teacher> WhenTeacherCreated => whenTeacherCreated.AsObservable();

        public Teacher Add(Teacher teacher)
        {
            this.whenTeacherCreated.OnNext(teacher);
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
        public List<Teacher> GetTeachers(string where)
        {
            return GetTeachers().Where(x => x.Name == where).ToList();
        }

    }
}
