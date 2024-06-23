namespace RLReplayWatcher.data;

internal sealed class ReplicatedBoost : RLRPReplicatedBoost {
    public new byte GrantCount { get; set; }
    public new byte BoostAmount { get; set; }
    public new byte Unused1 { get; set; }
    public new byte Unused2 { get; set; }

    public ReplicatedBoost Clone() {
        return new ReplicatedBoost {
            GrantCount = GrantCount,
            BoostAmount = BoostAmount,
            Unused1 = Unused1,
            Unused2 = Unused2
        };
    }
}