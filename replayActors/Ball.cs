using System.Numerics;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Ball : GameEntity {
    public RigidBodyState? RigidBody { get; set; }
    public int HitTeamNum { get; set; }
    public bool CollideActors { get; set; }
    public bool BlockActors { get; set; }
    public ReplicatedExplosionDataExtended? ExplosionDataExtended { get; set; }
    public ActiveActor? GameEvent { get; set; }

    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            case "TAGame.RBActor_TA:ReplicatedRBState": {
                RigidBody = (RigidBodyState)property.Data;
                break;
            }
            case "Engine.Actor:bCollideActors":
                CollideActors = (bool)property.Data;
                break;
            case "Engine.Actor:bBlockActors":
                BlockActors = (bool)property.Data;
                break;
            case "TAGame.Ball_TA:ReplicatedExplosionDataExtended":
                ExplosionDataExtended = (ReplicatedExplosionDataExtended)property.Data;
                break;
            case "TAGame.Ball_TA:GameEvent":
                GameEvent = (ActiveActor)property.Data;
                break;
            case "TAGame.Ball_TA:HitTeamNum":
                HitTeamNum = (byte)property.Data;
                break;
            default:
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} for object ball (TAGame.Ball_TA); data: {property.Data}");
                break;
        }
    }
}