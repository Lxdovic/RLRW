using System.Numerics;
using RocketLeagueReplayParser.NetworkStream;
using Quaternion = System.Numerics.Quaternion;
using RLRPQuaternion = RocketLeagueReplayParser.NetworkStream.Quaternion;

namespace RLReplayWatcher.replayActors;

internal sealed class CarActor(Vector3D position) : Actor {
    public bool Sleeping { get; set; }
    public Vector3 Position { get; set; } = new Vector3(position.X, position.Z, position.Y) / 100;
    public Quaternion Rotation { get; set; }
    public Vector3 LinearVelocity { get; set; }
    public Vector3 AngularVelocity { get; set; }
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
    public ActiveActor? RumblePickups { get; set; }

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
            case "Engine.Actor:bHidden":
                Hidden = (bool)property.Data;
                break;
            case "TAGame.Car_TA:TeamPaint":
                TeamPaint = (TeamPaint)property.Data;
                break;
            case "Engine.Pawn:PlayerReplicationInfo":
                PlayerActor = (ActiveActor)property.Data;
                break;
            case "TAGame.Vehicle_TA:ReplicatedThrottle":
                Throttle = (byte)property.Data;
                break;
            case "TAGame.Vehicle_TA:ReplicatedSteer":
                Steer = (byte)property.Data;
                break;
            case "TAGame.Vehicle_TA:bReplicatedHandbrake":
                Handbrake = (bool)property.Data;
                break;
            case "TAGame.Car_TA:RumblePickups":
                RumblePickups = (ActiveActor)property.Data;
                break;
            case "TAGame.Vehicle_TA:bDriving":
                IsDriving = (bool)property.Data;
                break;
            case "TAGame.Car_TA:ReplicatedDemolishGoalExplosion":
                DemoGoalExplosion = (ReplicatedDemolishGoalExplosion)property.Data;
                break;
            case "TAGame.RBActor_TA:bReplayActor":
                ReplayActor = (bool)property.Data;
                break;

            default:
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} for object car (TAGame.Car_TA); data: {property.Data}");
                break;
        }
    }
}