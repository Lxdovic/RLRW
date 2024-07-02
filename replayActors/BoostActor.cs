using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class BoostActor(ActorState? actor = null) : Actor {
    public ActiveActor? Vehicle { get; set; }
    public ReplicatedBoost? ReplicatedBoost { get; set; }
    public byte Amount { get; set; }
    public byte Active { get; set; }

    public override BoostActor Clone() {
        return new BoostActor {
            Vehicle = Vehicle?.Clone(),
            ReplicatedBoost = ReplicatedBoost?.Clone(),
            Amount = Amount,
            Active = Active
        };
    }

    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            case "TAGame.CarComponent_Boost_TA:ReplicatedBoost":
                var boost = (RLRPReplicatedBoost)property.Data;

                ReplicatedBoost = new ReplicatedBoost {
                    GrantCount = boost.GrantCount,
                    BoostAmount = boost.BoostAmount,
                    Unused1 = boost.Unused1,
                    Unused2 = boost.Unused2
                };
                break;
            case "TAGame.CarComponent_TA:Vehicle":
                var actor = (RLRPActiveActor)property.Data;

                Vehicle = new ActiveActor {
                    Active = actor.Active,
                    ActorId = actor.ActorId
                };
                break;
            case "TAGame.CarComponent_TA:ReplicatedActive":
                Active = (byte)property.Data;
                break;
            case "TAGame.CarComponent_Boost_TA:ReplicatedBoostAmount":
                Amount = (byte)property.Data;
                break;

            default:
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} for object boost (TAGame.Boost_TA); data: {property.Data}, type: {property.Data.GetType()}");
                break;
        }
    }
}