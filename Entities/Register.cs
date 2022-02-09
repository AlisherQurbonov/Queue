using System.ComponentModel.DataAnnotations;

namespace queue.Entities;

public class Register
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Fullname { get; set; }

    [Required]
    public string Phone { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModefiedAt { get; set; }
    public DateTimeOffset Queue { get; set; }
    public bool Active { get; set; }
}