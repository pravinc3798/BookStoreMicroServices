using System.ComponentModel.DataAnnotations;

namespace BookStore.User.Entity;

public class UserEntity
{
    [Key]
    public int UserId { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public string ContactNumber { get; set; }
    [Required]
    public string EmailId { get; set; }
    [Required]
    public string UserPassword { get; set; }
}
