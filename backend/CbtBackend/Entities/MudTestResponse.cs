using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CbtBackend.Entities;

public class MudTestResponse {
    public int Id { get; set; } = default!;

    [Required]
    [JsonIgnore]
    public User Author { get; set; } = default!;

    [NotMapped]
    public int? UserId {
        get {
            if (Author == null) {
                return null;
            } else {
                return Author.Id;
            }
        }
    }

    [Required]
    public DateTime Submitted { get; set; } = default!;

    [Required]
    public MudTest Evaluation { get; set; } = default!;

    [Required]
    public int Response1 { get; set; } = default!;

    [Required]
    public int Response2 { get; set; } = default!;

    [Required]
    public int Response3 { get; set; } = default!;

    [Required]
    public int Response4 { get; set; } = default!;

    [Required]
    public int Response5 { get; set; } = default!;
}
