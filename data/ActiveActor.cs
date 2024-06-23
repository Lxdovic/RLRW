namespace RLReplayWatcher.data;

internal sealed class ActiveActor : RLRPActiveActor {
    public new bool Active { get; set; }
    public new int ActorId { get; set; }
    
    public ActiveActor Clone() {
        return new ActiveActor {
            Active = Active,
            ActorId = ActorId,
        };
    }
}