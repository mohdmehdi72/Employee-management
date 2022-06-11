using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EmployeeManagement.Data
{
    public partial class UserLogin
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Phone]
        public long? Phone { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        [Required]
        public string ConPassword { get; set; }
    }
}
