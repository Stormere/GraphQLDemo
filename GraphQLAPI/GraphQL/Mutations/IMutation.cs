using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.GraphQL.Mutations
{
    public interface IMutation
    {
        void Resolve(GraphQLMutation mutation);

    }
}
