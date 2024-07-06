using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.data;

internal sealed class PickupData : ReplicatedPickupData {
    public new bool Unknown1 { get; set; }
    public new int ActorId { get; set; }
    public new byte Unknown2 { get; set; }

    public PickupData Clone() {
        return new PickupData {
            Unknown1 = Unknown1,
            Unknown2 = Unknown2,
            ActorId = ActorId
        };
    }
}