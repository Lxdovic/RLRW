using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class SoccarActor(ActorState? actor = null) : Actor {
    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} SoccarActor; data: {property.Data}");
                Console.ResetColor();
                break;
        }
    }

    public override SoccarActor Clone() {
        return new SoccarActor();
    }
}