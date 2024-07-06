using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class NetModeActor(ActorState? actor = null) : Actor {
    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} NetModeActor; data: {property.Data}");
                Console.ResetColor();
                break;
        }
    }

    public override NetModeActor Clone() {
        return new NetModeActor();
    }
}