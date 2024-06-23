namespace RLReplayWatcher.data;

internal sealed class ReplicatedExplosionDataExtended : RLRPReplicatedExplosionDataExtended {
    public new bool Unknown3 { get; set; }
    public new uint Unknown4 { get; set; }
    
    public ReplicatedExplosionDataExtended Clone() {
        return new ReplicatedExplosionDataExtended {
            Unknown3 = Unknown3,
            Unknown4 = Unknown4,
        };
    }
}