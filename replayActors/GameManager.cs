using System.Numerics;
using RLReplayWatcher.replayHelper;
using RocketLeagueReplayParser;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class GameManager(Replay replay) {
    public readonly Dictionary<int, GameEntity> Objects = [];

    private Replay Replay { get; } = replay;
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
                            new Ball(actor.Position));
                        break;
                    case "TAGame.Car_TA":
                        Objects.TryAdd(actorId,
                            new Car(actor.Position));
                        break;
                    case "TAGame.PRI_TA":
                        Objects.TryAdd(actorId, new Player());
                        break;
                    case "TAGame.GRI_TA":
                        Objects.TryAdd(actorId, new Game());
                        break;
                    case "TAGame.CameraSettingsActor_TA":
                        Objects.TryAdd(actorId, new CameraSettingsActor());
                        break;
                    case "TAGame.CarComponent_Boost_TA":
                        Objects.TryAdd(actorId, new Boost());
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