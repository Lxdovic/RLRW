using System.Numerics;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class CarActor : Actor {
    public bool Sleeping { get; set; }
    public Vector3 Position { get; set; }
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

    public override CarActor Clone() {
        return new CarActor {
            Sleeping = Sleeping,
            Position = new Vector3(Position.X, Position.Y, Position.Z),
            Rotation = new Quaternion(Rotation.X, Rotation.Y, Rotation.Z, Rotation.W),
            LinearVelocity = new Vector3(LinearVelocity.X, LinearVelocity.Y, LinearVelocity.Z),
            AngularVelocity = new Vector3(AngularVelocity.X, AngularVelocity.Y, AngularVelocity.Z),
            PlayerActor = PlayerActor?.Clone(),
            TeamPaint = TeamPaint?.Clone(),
            Throttle = Throttle,
            Steer = Steer,
            Handbrake = Handbrake,
            IsDriving = IsDriving,
            DemoGoalExplosion = DemoGoalExplosion?.Clone(),
            CollideActors = CollideActors,
            BlockActors = BlockActors,
            Hidden = Hidden,
            ReplayActor = ReplayActor,
            RumblePickups = RumblePickups?.Clone()
        };
    }

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
                var teamPaint = (RLRPTeamPaint)property.Data;
                
                TeamPaint = new TeamPaint {
                    TeamNumber = teamPaint.TeamNumber,
                    CustomColorId = teamPaint.CustomColorId,
                    CustomFinishId = teamPaint.CustomFinishId,
                    TeamColorId = teamPaint.TeamColorId,
                    TeamFinishId = teamPaint.TeamFinishId,
                };
                break;
            case "Engine.Pawn:PlayerReplicationInfo":
                var activeActor = (RLRPActiveActor)property.Data;
                
                PlayerActor = new ActiveActor {
                    Active = activeActor.Active,
                    ActorId = activeActor.ActorId
                };
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
                var rumblePickups = (RLRPActiveActor)property.Data;
                
                RumblePickups = new ActiveActor {
                    Active = rumblePickups.Active,
                    ActorId = rumblePickups.ActorId
                };
                break;
            case "TAGame.Vehicle_TA:bDriving":
                IsDriving = (bool)property.Data;
                break;
            case "TAGame.Car_TA:ReplicatedDemolishGoalExplosion":
                var demolishGoalExplosion = (RLRPReplicatedDemolishGoalExplosion)property.Data;
                
                DemoGoalExplosion = new ReplicatedDemolishGoalExplosion {
                    AttackerFlag = demolishGoalExplosion.AttackerFlag,
                    VictimFlag = demolishGoalExplosion.VictimFlag,
                    AttackerActorId = demolishGoalExplosion.AttackerActorId,
                    VictimActorId = demolishGoalExplosion.VictimActorId
                };
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