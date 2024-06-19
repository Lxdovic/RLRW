using System.Numerics;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Car(int id, Vector3 position) {
    public int Id { get; set; } = id;
    public Vector3 Position { get; set; } = position;
    public ActiveActor? PlayerActor { get; set; }
    public TeamPaint? TeamPaint { get; set; }
    public float Throttle { get; set; }
    public float Steer { get; set; }
    public bool Handbrake { get; set; }
    public bool IsDriving { get; set; }
    public ReplicatedDemolishGoalExplosion? DemoGoalExplosion { get; set; }
    public bool CollideActors { get; set; }
    public bool BlockActors { get; set; }
    public bool Hidden { get; set; }
    public bool ReplayActor { get; set; }
}