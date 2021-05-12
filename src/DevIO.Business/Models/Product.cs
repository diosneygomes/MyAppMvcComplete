using System;

namespace DevIO.Business.Models
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public DateTime RegistraionDate { get; set; }

        /* EF Relation */
        public Provider Provider { get; set; }
        public Guid ProviderId { get; set; }
    }
}
