using System.Collections.Generic;

namespace GraphQLDemo.Services
{
    public interface IValuesService
    {
        IEnumerable<string> FindAll();

        string Find(int id);
    }
}