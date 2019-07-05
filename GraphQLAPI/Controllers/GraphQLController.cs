using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQLDemo.GraphQL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GraphQLDemo.Controllers
{
    public class GraphQLController : Controller
    {
        private readonly GraphQLQuery _graphQLQuery;
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        private readonly GraphQLMutation _graphMutation;


        public GraphQLController(GraphQLQuery graphQLQuery, GraphQLMutation graphMutation, IDocumentExecuter documentExecuter, ISchema schema)
        {
            _graphQLQuery = graphQLQuery;
            _documentExecuter = documentExecuter;
            _schema = schema;
            _graphMutation = graphMutation;
        }

        /// <summary>
        /// Main endpoint for retrievieng data by GrapQL query language.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <returns>Data retrieved by query.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLParameter query)
        {
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions { Schema = _schema, Query = query.Query, UserContext = User, Inputs = inputs };
            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }


        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
