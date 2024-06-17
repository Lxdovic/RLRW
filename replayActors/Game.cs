using RLReplayWatcher.replayHelper;
using RocketLeagueReplayParser;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Ball(int id, Vector3D position) {
    public int Id { get; set; } = id;
    public Vector3D Position { get; set; } = position;
}

internal sealed class Car(int id, Vector3D position) {
    public int Id { get; set; } = id;
    public Vector3D Position { get; set; } = position;
}

internal sealed class Game {
    public Dictionary<int, object> Objects = [];

    public Game(Replay replay) {
        Replay = replay;
    }

    private Replay Replay { get; }
    private int FrameIndex { get; set; }

    public void TryNextFrame(double time) {
        if (FrameIndex >= Replay.Frames.Count || FrameIndex < 0) return;

        if (Replay.Frames[FrameIndex].Time < time) FrameIndex++;

        var frame = Replay.Frames[FrameIndex];

        foreach (var actor in frame.ActorStates) {
            var actorId = (int)actor.Id;

            if (actor.State == ActorStateState.New) {
                var className = ReplayHelper.GetClass(Replay, actor)?.Class;

                switch (className) {
                    case "TAGame.Ball_TA":
                        Objects.TryAdd(actorId, new Ball(actorId, actor.Position));
                        break;
                    case "TAGame.Car_TA":
                        Objects.TryAdd(actorId, new Car(actorId, actor.Position));
                        break;
                }
            }
            
            
            if (actor.State == ActorStateState.Existing && Objects.TryGetValue(actorId, out var obj)) {
                if (obj is Ball ball) {
                    foreach (var (_, property) in actor.Properties) {
                        if (property.PropertyName == "TAGame.RBActor_TA:ReplicatedRBState") {
                            var data = (RigidBodyState)property.Data;
                            ball.Position = new Vector3D(data.Position.X, data.Position.Z, data.Position.Y);
                        }
                    }
                }
                
                if (obj is Car car) {
                    foreach (var (_, property) in actor.Properties) {
                        if (property.PropertyName == "TAGame.RBActor_TA:ReplicatedRBState") {
                            var data = (RigidBodyState)property.Data;
                            car.Position = new Vector3D(data.Position.X, data.Position.Z, data.Position.Y);
                        }
                    }
                }
            }

            if (actor.State == ActorStateState.Deleted && Objects.ContainsKey(actorId)) Objects.Remove(actorId);
        }
    }
}