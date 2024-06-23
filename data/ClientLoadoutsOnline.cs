namespace RLReplayWatcher.data;

internal sealed class ClientLoadoutsOnline : RLRPClientLoadoutsOnline {
    public new ClientLoadoutOnline? LoadoutOnline1 { get; set; }
    public new ClientLoadoutOnline? LoadoutOnline2 { get; set; }
    public new bool Unknown1 { get; set; }
    public new bool Unknown2 { get; set; }

    public ClientLoadoutsOnline Clone() {
        return new ClientLoadoutsOnline {
            LoadoutOnline1 = LoadoutOnline1?.Clone(),
            LoadoutOnline2 = LoadoutOnline2?.Clone(),
            Unknown1 = Unknown1,
            Unknown2 = Unknown2
        };
    }
}