namespace ExampleForGoldenMaster.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Windows;

    [Table("Service")]
    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            ClientServices = new HashSet<ClientService>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost { get; set; }

        public int DurationInSeconds { get; set; }

        public string Description { get; set; }

        public double? Discount { get; set; }

        [StringLength(1000)]
        public string MainImagePath { get; set; }

        [NotMapped]
        public string FullImagePath {get; set;}

        [NotMapped]
        public Visibility HaveDiscont { get; set; }

        [NotMapped]
        public int TotalCost { get; set; }
        
        [NotMapped]
        public string ColorBackground { get; set; }

        [NotMapped]
        public int DurationInMinutes { get; set; }

        [NotMapped]
        public Visibility VisibilityButtonForEdit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientService> ClientServices { get; set; }
    }
}
