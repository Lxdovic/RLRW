using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal abstract class Actor(ActorState? actor = null) : ICloneable {
    public abstract object Clone();
    public abstract void HandleGameEvents(ActorStateProperty property);
}