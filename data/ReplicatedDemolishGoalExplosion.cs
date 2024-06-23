using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.data;

internal sealed class ReplicatedDemolishGoalExplosion : RLRPReplicatedDemolishGoalExplosion {
    public new bool GoalExplosionOwnerFlag { get; set; }
    public new int GoalExplosionOwner { get; set; }
    public new bool AttackerFlag { get; set; }
    public new int AttackerActorId { get; set; }
    public new bool VictimFlag { get; set; }
    public new uint VictimActorId { get; set; }
    public new Vector3D? AttackerVelocity { get; set; }
    public new Vector3D? VictimVelocity { get; set; }

    public ReplicatedDemolishGoalExplosion Clone() {
        return new ReplicatedDemolishGoalExplosion {
            GoalExplosionOwnerFlag = GoalExplosionOwnerFlag,
            GoalExplosionOwner = GoalExplosionOwner,
            AttackerFlag = AttackerFlag,
            AttackerActorId = AttackerActorId,
            VictimFlag = VictimFlag,
            VictimActorId = VictimActorId,
            AttackerVelocity = AttackerVelocity == null
                ? null
                : new Vector3D(AttackerVelocity.X, AttackerVelocity.Y, AttackerVelocity.Z),
            VictimVelocity = VictimVelocity == null
                ? null
                : new Vector3D(VictimVelocity.X, VictimVelocity.Y, VictimVelocity.Z)
        };
    }
}