using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class MapScoreboardActor(ActorState? actor = null) : Actor {
    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            default:
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} for object ball (TAGame.Ball_TA); data: {property.Data}");
                break;
        }
    }

    public override MapScoreboardActor Clone() {
        return new MapScoreboardActor();
    }
}