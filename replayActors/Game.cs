using RLReplayWatcher.replayHelper;
using RocketLeagueReplayParser;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Ball(int id, Vector3D position) {
    public int Id { get; set; } = id;
    public Vector3D Position { get; set; } = position;
}

internal sealed class Game {
    public Dictionary<int, Ball> Balls = [];
    public Dictionary<int, string> IdToClass = new();
    
    public Game(Replay replay) {
        foreach (var frame in replay.Frames)
        foreach (var actor in frame.ActorStates) {
            var className = ReplayHelper.GetClass(replay, actor)?.Class;
            var actorId = (int)actor.Id;
            
            if (actor.State == ActorStateState.New) {
                switch (className) {
                    case "TAGame.Ball_TA":
                        if (Balls.ContainsKey(actorId)) {
                            Balls[actorId] = new Ball(actorId, actor.Position);
                            break;
                        }
                        
                        Balls.Add(actorId, new Ball(actorId, actor.Position));
                        IdToClass.Add(actorId, className);
                        break;
                }
            }

            if (actor.State == ActorStateState.Deleted) {
                switch (IdToClass[actorId]) {
                    case "TAGame.Ball_TA":
                        Balls.Remove(actorId);
                        IdToClass.Remove(actorId);
                        break;
                }
            }
            
            // switch (IdToClass[(int)actor.Id]) {
            //     case "TAGame.Ball_TA":
            //         Balls[(int)actor.Id].Position = actor.Position;
            //         break;
            // }
        }
    }
    
    
}