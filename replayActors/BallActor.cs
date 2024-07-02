using System.Numerics;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class BallActor() : Actor {
    public BallActor(ActorState? actor = null) : this() {
        if (actor == null) return;

        Position = new Vector3(actor!.Position.X, actor.Position.Z, actor.Position.Y) / 100;
    }

    public bool Sleeping { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 LinearVelocity { get; set; }
    public Vector3 AngularVelocity { get; set; }
    public int HitTeamNum { get; set; }
    public bool CollideActors { get; set; }
    public bool BlockActors { get; set; }
    public ReplicatedExplosionDataExtended? ExplosionDataExtended { get; set; }
    public ActiveActor? GameEvent { get; set; }
    public ActiveActor? ReplayActor { get; set; }

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
                var explosionDataExtended = (RLRPReplicatedExplosionDataExtended)property.Data;

                ExplosionDataExtended = new ReplicatedExplosionDataExtended {
                    Unknown3 = explosionDataExtended.Unknown3,
                    Unknown4 = explosionDataExtended.Unknown4
                };
                break;
            case "TAGame.Ball_TA:GameEvent":
                var gameEvent = (RLRPActiveActor)property.Data;

                GameEvent = new ActiveActor {
                    Active = gameEvent.Active,
                    ActorId = gameEvent.ActorId
                };
                break;
            case "TAGame.Ball_TA:HitTeamNum":
                HitTeamNum = (byte)property.Data;
                break;
            case "TAGame.RBActor_TA:bReplayActor":
                var replayActor = (RLRPActiveActor)property.Data;

                ReplayActor = new ActiveActor {
                    Active = replayActor.Active,
                    ActorId = replayActor.ActorId
                };
                break;
            default:
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} for object ball (TAGame.Ball_TA); data: {property.Data}");
                break;
        }
    }

    public override BallActor Clone() {
        return new BallActor {
            Sleeping = Sleeping,
            Position = new Vector3(Position.X, Position.Y, Position.Z),
            Rotation = new Quaternion(Rotation.X, Rotation.Y, Rotation.Z, Rotation.W),
            LinearVelocity = new Vector3(LinearVelocity.X, LinearVelocity.Y, LinearVelocity.Z),
            AngularVelocity = new Vector3(AngularVelocity.X, AngularVelocity.Y, AngularVelocity.Z),
            HitTeamNum = HitTeamNum,
            CollideActors = CollideActors,
            BlockActors = BlockActors,
            ExplosionDataExtended = ExplosionDataExtended?.Clone(),
            GameEvent = GameEvent?.Clone()
        };
    }
}