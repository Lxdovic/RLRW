using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class MapScoreboardActor(ActorState? actor = null) : Actor {
    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} MapScoreboardActor; data: {property.Data}");
                Console.ResetColor();
                break;
        }
    }

    public override MapScoreboardActor Clone() {
        return new MapScoreboardActor();
    }
}