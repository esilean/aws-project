using AWS.Insurance.Operations.Domain.Errors;
using AWS.Insurance.Operations.Domain.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace AWS.Insurance.Operations.Domain.Models
{
    public class Car
    {
        [Timestamp]
        public byte[] RowVersion { get; private set; }
        public Guid Id { get; private set; }
        public BranchType BranchType { get; private set; }
        public string Name { get; private set; }
        public int Year { get; private set; }
        public Guid CustomerId { get; private set; }
        public virtual Customer Customer { get; private set; }

        public Car(Guid customerId, BranchType branchType, string name, int year)
        {
            if (Guid.Empty == customerId)
                throw new DomainException("Customer must not be empty.");

            Id = Guid.NewGuid();
            BranchType = branchType;
            Name = name;
            Year = year;
            CustomerId = customerId;
        }

    }
}