using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace queue.Entities;

public class User : IdentityUser
{
    [Required]
    [MaxLength(32)]
    public string Fullname { get; set; }
    
    
}

    
