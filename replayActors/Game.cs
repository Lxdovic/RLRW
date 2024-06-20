using System.Numerics;
using RLReplayWatcher.replayHelper;
using RocketLeagueReplayParser;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Team(int id) {
    public int Id { get; set; } = id;
    public string Name { get; set; } = "";
}

internal sealed class Game {
    public Dictionary<int, GameEntity> Objects = [];

    public Game(Replay replay) {
        Replay = replay;
    }

    private Replay Replay { get; }
    private int FrameIndex { get; set; }

    public void TryNextFrame(double time) {
        if (FrameIndex >= Replay.Frames.Count - 1 || FrameIndex < 0) return;

        if (Replay.Frames[FrameIndex].Time < time) FrameIndex++;

        var frame = Replay.Frames[FrameIndex];

        foreach (var actor in frame.ActorStates) {
            var actorId = (int)actor.Id;

            if (actor.State == ActorStateState.New) {
                var className = ReplayHelper.GetClass(Replay, actor)?.Class;

                switch (className) {
                    case "TAGame.Ball_TA":
                        Objects.TryAdd(actorId,
                            new Ball(new Vector3(actor.Position.X, actor.Position.Z, actor.Position.Y)));
                        break;
                    case "TAGame.Ball_TA:GameEvent": break;
                    case "TAGame.Car_TA":
                        Objects.TryAdd(actorId,
                            new Car(new Vector3(actor.Position.X, actor.Position.Z, actor.Position.Y)));
                        break;
                    case "TAGame.PRI_TA":
                        Objects.TryAdd(actorId, new Player());
                        break;
                }
            }

            if (actor.State == ActorStateState.Existing && Objects.TryGetValue(actorId, out var obj))
                foreach (var (_, property) in actor.Properties)
                    obj.HandleGameEvents(property);

            if (actor.State == ActorStateState.Deleted && Objects.ContainsKey(actorId)) Objects.Remove(actorId);
        }
    }
}