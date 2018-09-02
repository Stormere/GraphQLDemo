using Newtonsoft.Json.Linq;

namespace GraphQLDemo.GraphQL
{
    public class GraphQLParameter
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
