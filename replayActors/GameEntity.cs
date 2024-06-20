using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal abstract class GameEntity {
    public abstract void HandleGameEvents(ActorStateProperty property);
}