using System.ComponentModel.DataAnnotations;

namespace CbtBackend.Models.Requests;

public class EvaluationUpdateRequest {
    public int? Response1 { get; set; }
    public int? Response2 { get; set; }
    public int? Response3 { get; set; }
    public int? Response4 { get; set; }
    public int? Response5 { get; set; }
}
