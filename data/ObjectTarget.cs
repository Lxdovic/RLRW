namespace RLReplayWatcher.data;

internal sealed class ObjectTarget : RLRPObjectTarget {
    public new bool Unknown1 { get; set; }
    public new int ObjectIndex { get; set; }

    public ObjectTarget Clone() {
        return new ObjectTarget {
            Unknown1 = Unknown1,
            ObjectIndex = ObjectIndex
        };
    }
}