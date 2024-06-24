namespace RLReplayWatcher.data;

internal sealed class ClubColors : RLRPClubColors {
    public new bool Unknown1 { get; set; }
    public new byte Unknown2 { get; set; }
    public new bool Unknown3 { get; set; }
    public new byte Unknown4 { get; set; }
    
    public ClubColors Clone() {
        return new ClubColors {
            Unknown1 = Unknown1,
            Unknown2 = Unknown2,
            Unknown3 = Unknown3,
            Unknown4 = Unknown4
        };
    }
}