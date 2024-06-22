using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal abstract class Actor {
    public abstract void HandleGameEvents(ActorStateProperty property);
}