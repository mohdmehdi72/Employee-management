using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EmployeeManagement.Data
{
    public partial class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int? Age { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public decimal? Salary { get; set; }
        [Required]
        public long? Phone { get; set; }
        [Required]
        public string Technology { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ImgUrl { get; set; }
    }
}
