namespace CbtBackend.Entities;

public class MudTestResultsTableEntry {
    public int Id { get; set; } = default!;

    public int ScoreFrom { get; set; }

    public int ScoreTo { get; set; }

    public string EntryName { get; set; } = default!;

    public string Description { get; set; } = default!;
}
