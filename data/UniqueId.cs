namespace RLReplayWatcher.data;

internal class UniqueId : RLRPUniqueId {
    public new UniqueIdType Type { get; set; }
    public new byte[]? Id { get; set; }
    public new byte PlayerNumber { get; set; }

    public UniqueId Clone() {
        return new UniqueId {
            Type = Type,
            Id = (byte[]?)Id?.Clone(),
            PlayerNumber = PlayerNumber
        };
    }
}