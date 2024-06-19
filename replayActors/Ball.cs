using System.Numerics;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Ball(int id, Vector3 position) {
    public int Id { get; set; } = id;
    public Vector3 Position { get; set; } = position;

    public int HitTeamNum { get; set; }
    public bool CollideActors { get; set; }
    public bool BlockActors { get; set; }
    public ReplicatedExplosionDataExtended? ExplosionDataExtended { get; set; }
}