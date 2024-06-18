using System.Numerics;
using RLReplayWatcher.replayHelper;
using RocketLeagueReplayParser;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Team(int id) {
    public int Id { get; set; } = id;
    public string Name { get; set; } = "";
}

internal sealed class Player(int id) {
    public int Id { get; set; } = id;
    public string Name { get; set; } = "";
    public int Team { get; set; } = 0;
}

internal sealed class Ball(int id, Vector3 position) {
    public int Id { get; set; } = id;
    public Vector3 Position { get; set; } = position;
}

internal sealed class Car(int id, Vector3 position) {
    public int Id { get; set; } = id;
    public TeamPaint TeamPaint { get; set; } = new();
    public Vector3 Position { get; set; } = position;
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
                        Objects.TryAdd(actorId,
                            new Ball(actorId, new Vector3(actor.Position.X, actor.Position.Z, actor.Position.Y)));
                        break;
                    case "TAGame.Car_TA":
                        Objects.TryAdd(actorId,
                            new Car(actorId, new Vector3(actor.Position.X, actor.Position.Z, actor.Position.Y)));
                        break;
                    // case "TAGame.PRI_TA":
                    //     Objects.TryAdd(actorId, new Player(actorId));
                    //     break;
                }
            }


            if (actor.State == ActorStateState.Existing && Objects.TryGetValue(actorId, out var obj)) {
                foreach (var (_, property) in actor.Properties) {
                    if (obj is Ball ball) {
                        if (property.PropertyName == "TAGame.RBActor_TA:ReplicatedRBState") {
                            var data = (RigidBodyState)property.Data;
                            ball.Position = new Vector3(data.Position.X, data.Position.Z, data.Position.Y);
                        }
                    }

                    if (obj is Car car) {
                        if (property.PropertyName == "TAGame.RBActor_TA:ReplicatedRBState") {
                            var data = (RigidBodyState)property.Data;
                            car.Position = new Vector3(data.Position.X, data.Position.Z, data.Position.Y);
                        }
                        
                        if (property.PropertyName == "TAGame.Car_TA:TeamPaint") {
                            car.TeamPaint = (TeamPaint)property.Data;
                        }
                    }

                    // if (obj is Player player) {
                    //     switch (property.PropertyName) {
                    //         case "Engine.PlayerReplicationInfo:PlayerName":
                    //             player.Name = (string)property.Data;
                    //             break;
                    //
                    //         // case "Engine.PlayerReplicationInfo:Team":
                    //         //     player.Team = (int)property.Data;
                    //         //     break;
                    //     }
                    // }
                }
            }
            
            if (actor.State == ActorStateState.Deleted && Objects.ContainsKey(actorId)) Objects.Remove(actorId);
        }
    }
}