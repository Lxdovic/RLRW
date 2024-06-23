using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal abstract class Actor : ICloneable {
    public abstract object Clone();
    public abstract void HandleGameEvents(ActorStateProperty property);
}