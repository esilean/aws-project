using AWS.Insurance.Operations.Domain.Errors;
using AWS.Insurance.Operations.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AWS.Insurance.Operations.Domain.Models
{
    public class Customer
    {
        private readonly List<Car> _cars;

        [Timestamp]
        public byte[] RowVersion { get; private set; }
        public Guid Id { get; private set; }
        public int CNumber { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public DateTime Dob { get; private set; }
        public int ZipCode { get; private set; }
        public Zone Zone { get; private set; }
        public virtual ICollection<Car> Cars => _cars;

        public Customer(int cNumber, string name, int age, DateTime dob, int zipCode)
        {
            if (cNumber <= 0)
                throw new DomainException("Cnumber must be greater than zero.");

            Id = Guid.NewGuid();
            CNumber = cNumber;
            Name = name;
            Age = age;
            Dob = dob;
            ZipCode = zipCode;
            Zone = Zone.Red;

            _cars = new List<Car>();
        }

        public void AddZone(Zone zone)
        {
            Zone = zone;
        }

    }
}