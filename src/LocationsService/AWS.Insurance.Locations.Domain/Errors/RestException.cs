using System;
using System.Net;

namespace AWS.Insurance.Locations.Domain.Errors
{
    public class RestException : Exception
    {
        public HttpStatusCode Code { get; }
        public dynamic Errors { get; }

        public RestException(HttpStatusCode code, dynamic errors = null)
        {
            Code = code;
            Errors = errors;
        }
    }
}
