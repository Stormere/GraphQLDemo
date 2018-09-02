﻿using GraphQLDemo.GraphQL.Errors;

namespace GraphQLDemo.GraphQL.Queries
{
    public class Resolver
    {
        public Response Response(object data)
        {
            return new Response(data);
        }

        public Response Error(GraphQLError error)
        {
            return new Response(error.StatusCode, error.ErrorMessage);
        }

        public Response AccessDeniedError()
        {
            var error = new AccessDeniedError();
            return new Response(error.StatusCode, error.ErrorMessage);
        }

        public Response NotFoundError(string id)
        {
            var error = new NotFoundError(id);
            return new Response(error.StatusCode, error.ErrorMessage);
        }
    }

}
