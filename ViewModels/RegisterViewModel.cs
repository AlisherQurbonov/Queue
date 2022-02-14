using System.ComponentModel.DataAnnotations;

namespace queue.ViewModels;

public class RegisterViewModel
{
    [Key]
    public Guid Id { get; set; }


    [Required(ErrorMessage = "To'liq ism-sharfini kiritish shart!")]
    [Display(Name = "Ism-sharf")]
     public string Fullname { get; set; }

    [Required(ErrorMessage = "Telefon raqam kiritish shart!")]
    [RegularExpression(
        @"^[\+]?(998[-\s\.]?)([0-9]{2}[-\s\.]?)([0-9]{3}[-\s\.]?)([0-9]{2}[-\s\.]?)([0-9]{2}[-\s\.]?)$", 
        ErrorMessage = "Telefon raqam formati noto'g'ri.")]
      public string Phone { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset ModefiedAt { get; set; }

    public DateTimeOffset Queue { get; set; }

    public bool Active { get; set; }
}