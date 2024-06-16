using RLReplayWatcher.replayHelper;
using RocketLeagueReplayParser;

namespace RLReplayWatcher.replayActors;

internal sealed class CoreObject {
    public CoreObject(Replay replay) {
        foreach (var frame in replay.Frames)
        foreach (var actor in frame.ActorStates)
            switch (ReplayHelper.GetClass(replay, actor)?.Class) {
                case "TAGame.Ball_TA":
                    Console.WriteLine(actor.Position);
                    break;
            }
    }
}