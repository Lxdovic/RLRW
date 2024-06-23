using RLReplayWatcher.replayActors;
using RocketLeagueReplayParser;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayHelper;

internal static class ReplayHelper {
    public static string? GetName(Replay? replay, ActorState? actor) {
        var actorId = actor?.NameId;
        if (actorId == null || replay == null) return null;

        return (string)replay.Names.GetValue((int)actorId)!;
    }
    
    public static ClassIndex? GetClass(Replay? replay, ActorState? actor) {
        return replay?.ClassIndexes.Find(x => x.Index == actor?.ClassId);
    }

    public static Replay Parse(string path) {
        var replay = Replay.Deserialize(path);

        return replay;
    }
}