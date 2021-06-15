using System;

namespace DevIO.Business.Models
{
   public class Address : Entity
    {
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }

        /*EF Relation*/
        public Provider Provider { get; set; }
        public Guid ProviderId { get; set; }
    }
}
