using System.Numerics;
using RocketLeagueReplayParser.NetworkStream;
using Quaternion = System.Numerics.Quaternion;
using RLRPQuaternion = RocketLeagueReplayParser.NetworkStream.Quaternion;

namespace RLReplayWatcher.replayActors;

internal sealed class Ball(Vector3D position) : GameEntity {
    public bool Sleeping { get; set; }
    public Vector3 Position { get; set; } = new Vector3(position.X, position.Z, position.Y) / 100;
    public Quaternion Rotation { get; set; }
    public Vector3 LinearVelocity { get; set; }
    public Vector3 AngularVelocity { get; set; }
    public int HitTeamNum { get; set; }
    public bool CollideActors { get; set; }
    public bool BlockActors { get; set; }
    public ReplicatedExplosionDataExtended? ExplosionDataExtended { get; set; }
    public ActiveActor? GameEvent { get; set; }

    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            case "TAGame.RBActor_TA:ReplicatedRBState":
                var rbState = (RigidBodyState)property.Data;
                var rotation = (RLRPQuaternion)rbState.Rotation;

                Sleeping = rbState.Sleeping;
                Position = new Vector3(rbState.Position.X, rbState.Position.Z, rbState.Position.Y) / 100;
                Rotation = new Quaternion(rotation.X, rotation.Z, rotation.Y, rotation.W);

                if (!Sleeping) {
                    LinearVelocity =
                        new Vector3(rbState.LinearVelocity.X, rbState.LinearVelocity.Z, rbState.LinearVelocity.Y) / 100;

                    AngularVelocity = new Vector3(rbState.AngularVelocity.X, rbState.AngularVelocity.Z,
                        rbState.AngularVelocity.Y) / 100;
                }

                break;
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