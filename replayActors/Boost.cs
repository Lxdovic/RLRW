using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Boost : GameEntity {
    public ActiveActor? Vehicle { get; set; }
    public ReplicatedBoost? ReplicatedBoost { get; set; }
    public byte Active { get; set; }

    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            case "TAGame.CarComponent_Boost_TA:ReplicatedBoost":
                ReplicatedBoost = (ReplicatedBoost)property.Data;
                break;
            case "TAGame.CarComponent_TA:Vehicle":
                Vehicle = (ActiveActor)property.Data;
                break;
            case "TAGame.CarComponent_TA:ReplicatedActive":
                Active = (byte)property.Data;
                break;
            
            default:
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} for object game (TAGame.GRI_TA); data: {property.Data}, type: {property.Data.GetType()}");
                break;
        }
    }
}