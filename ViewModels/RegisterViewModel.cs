using System.ComponentModel.DataAnnotations;

namespace queue.ViewModels;

public class RegisterViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "To'liq ism-sharfini kiritish shart!")]
    [Display(Name = "Ism-sharf")]
    
    public string Fullname { get; set; }
    public string Phone { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModefiedAt { get; set; }
    public DateTimeOffset Queue { get; set; }
    public bool Active { get; set; }
}