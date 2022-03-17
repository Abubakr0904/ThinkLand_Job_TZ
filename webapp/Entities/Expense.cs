using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapp.Entities;

public class Expense
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Name length should be in range 2-32")]
    [MaxLength(32, ErrorMessage = "Name length should be in range 2-32")]
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    [Required]
    [ForeignKey(nameof(Category))]
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public string AuthorName { get; set; }
    public AppUser Author { get; set; }
}