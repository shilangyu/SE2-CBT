using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CbtBackend.Entities;

public class MudTestResponse {
    public int Id { get; set; } = default!;

    [Required]
    public User Author { get; set; } = default!;

    [Required]
    public DateTime Submitted { get; set; } = default!;

    [Required]
    [ForeignKey("Id")]
    public MudTest Evaluation { get; set; } = default!;

    public int Response1 { get; set; } = default!;

    public int Response2 { get; set; } = default!;

    public int Response3 { get; set; } = default!;

    public int Response4 { get; set; } = default!;

    public int Response5 { get; set; } = default!;
}
