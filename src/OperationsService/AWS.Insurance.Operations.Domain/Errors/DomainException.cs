using System;

namespace AWS.Insurance.Operations.Domain.Errors
{
    public class DomainException : Exception
    {
        public DomainException() : base() { }
        public DomainException(string message) : base(message) { }
        public DomainException(string message, Exception ex) : base(message, ex) { }
    }
}
