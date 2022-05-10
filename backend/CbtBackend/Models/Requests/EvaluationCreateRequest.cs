using System.ComponentModel.DataAnnotations;

namespace CbtBackend.Models.Requests;

public class EvaluationCreateRequest {
    [Required]
    public int TestId { get; set; }

    [Required]
    public int Response1 { get; set; }

    [Required]
    public int Response2 { get; set; }

    [Required]
    public int Response3 { get; set; }

    [Required]
    public int Response4 { get; set; }

    [Required]
    public int Response5 { get; set; }
}
