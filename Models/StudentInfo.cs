using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace studentmanagementsystem.Models
{
    public class StudentInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        public DateTime DOB { get; set; }
        public int Age { get; set; }

        public string? ContactNo { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Address { get; set; }
        public string? Pincode { get; set; }

        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }

        public decimal? Amount { get; set; }
        public bool Status { get; set; }

        public string? ImagePath { get; set; }  
        public virtual Country? Country { get; set; }
        public virtual State? State { get; set; }
        public virtual City? City { get; set; }
    }

}

