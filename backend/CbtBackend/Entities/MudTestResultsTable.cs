namespace CbtBackend.Entities;

public class MudTestResultsTable {
    public int Id { get; set; } = default!;

    public string EntryCategory { get; set; } = default!;

    public List<MudTestResultsTableEntry> Entries { get; set; } = default!;
}
