namespace RLReplayWatcher.data;

internal sealed class ClientLoadouts : RLRPClientLoadouts {
    public new ClientLoadout? Loadout1 { get; set; }
    public new ClientLoadout? Loadout2 { get; set; }

    public ClientLoadouts Clone() {
        return new ClientLoadouts {
            Loadout1 = Loadout1?.Clone(),
            Loadout2 = Loadout2?.Clone()
        };
    }
}