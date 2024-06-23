namespace RLReplayWatcher.data;

internal sealed class Reservation : RLRPReservation {
    public new uint Unknown1 { get; set; }
    public new UniqueId? PlayerId { get; set; }
    public new string? PlayerName { get; set; }
    public new byte Unknown2 { get; set; }

    public Reservation Clone() {
        return new Reservation {
            Unknown1 = Unknown1,
            PlayerId = PlayerId?.Clone(),
            PlayerName = PlayerName,
            Unknown2 = Unknown2
        };
    }
}