using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace webapp.Entities;

public class Category
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Name length should be in range 2-16")]
    [MaxLength(16, ErrorMessage = "Name length should be in range 2-16")]
    public string Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ICollection<Expense> Expenses { get; set; }
}