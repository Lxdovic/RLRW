using System.Numerics;
using RLReplayWatcher.replayHelper;
using RocketLeagueReplayParser;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Team(int id) {
    public int Id { get; set; } = id;
    public string Name { get; set; } = "";
}

internal sealed class Game {
    public Dictionary<int, object> Objects = [];

    public Game(Replay replay) {
        Replay = replay;
    }

    private Replay Replay { get; }
    private int FrameIndex { get; set; }

    public void TryNextFrame(double time) {
        if (FrameIndex >= Replay.Frames.Count || FrameIndex < 0) return;

        if (Replay.Frames[FrameIndex].Time < time) FrameIndex++;

        var frame = Replay.Frames[FrameIndex];

        foreach (var actor in frame.ActorStates) {
            var actorId = (int)actor.Id;

            if (actor.State == ActorStateState.New) {
                var className = ReplayHelper.GetClass(Replay, actor)?.Class;

                switch (className) {
                    case "TAGame.Ball_TA":
                        Objects.TryAdd(actorId,
                            new Ball(actorId, new Vector3(actor.Position.X, actor.Position.Z, actor.Position.Y)));
                        break;
                    case "TAGame.Ball_TA:GameEvent": break;
                    case "TAGame.Car_TA":
                        Objects.TryAdd(actorId,
                            new Car(actorId, new Vector3(actor.Position.X, actor.Position.Z, actor.Position.Y)));
                        break;
                    case "TAGame.PRI_TA":
                        Objects.TryAdd(actorId, new Player(actorId));
                        break;
                }
            }
            
            if (actor.State == ActorStateState.Existing && Objects.TryGetValue(actorId, out var obj))
                foreach (var (_, property) in actor.Properties) {
                    if (obj is Ball ball)
                        switch (property.PropertyName) {
                            case "TAGame.RBActor_TA:ReplicatedRBState": {
                                var data = (RigidBodyState)property.Data;
                                ball.Position = new Vector3(data.Position.X, data.Position.Z, data.Position.Y);
                                break;
                            }
                            case "Engine.Actor:bCollideActors":
                                ball.CollideActors = (bool)property.Data;
                                break;
                            case "Engine.Actor:bBlockActors": 
                                ball.BlockActors = (bool)property.Data;
                                break;
                            case "TAGame.Ball_TA:ReplicatedExplosionDataExtended":
                                ball.ExplosionDataExtended = (ReplicatedExplosionDataExtended)property.Data;
                                break;
                            case "TAGame.Ball_TA:GameEvent":
                                // var gameEvent = (GameEvent)property.Data;
                                break;
                            case "TAGame.Ball_TA:HitTeamNum":
                                ball.HitTeamNum = (byte)property.Data;
                                break;
                            default:
                                Console.WriteLine(
                                    $"Unhandled property: {property.PropertyName} for object ball (TAGame.Ball_TA); data: {property.Data}");
                                break;
                        }

                    if (obj is Car car)
                        switch (property.PropertyName) {
                            case "TAGame.RBActor_TA:ReplicatedRBState": {
                                var data = (RigidBodyState)property.Data;
                                car.Position = new Vector3(data.Position.X, data.Position.Z, data.Position.Y);
                                break;
                            }
                            case "Engine.Actor:bCollideActors":
                                car.CollideActors = (bool)property.Data;
                                break;
                            case "Engine.Actor:bBlockActors": 
                                car.BlockActors = (bool)property.Data;
                                break;
                            case "Engine.Actor:bHidden":
                                car.Hidden = (bool)property.Data;
                                break;
                            case "TAGame.Car_TA:TeamPaint":
                                car.TeamPaint = (TeamPaint)property.Data;
                                break;
                            case "Engine.Pawn:PlayerReplicationInfo":
                                car.PlayerActor = (ActiveActor)property.Data;
                                break;
                            case "TAGame.Vehicle_TA:ReplicatedThrottle":
                                car.Throttle = (byte)property.Data;
                                break;
                            case "TAGame.Vehicle_TA:ReplicatedSteer":
                                car.Steer = (byte)property.Data;
                                break;
                            case "TAGame.Vehicle_TA:bReplicatedHandbrake":
                                car.Handbrake = (bool)property.Data;
                                break;
                            case "TAGame.Car_TA:RumblePickups":
                                // var rpActor = (ActiveActor)property.Data;
                                break;
                            case "TAGame.Vehicle_TA:bDriving":
                                car.IsDriving = (bool)property.Data;
                                break;
                            case "TAGame.Car_TA:ReplicatedDemolishGoalExplosion": 
                                car.DemoGoalExplosion = (ReplicatedDemolishGoalExplosion)property.Data;
                                break;
                            case "TAGame.RBActor_TA:bReplayActor": 
                                car.ReplayActor = (bool)property.Data;
                                break;
                            
                            default:
                                Console.WriteLine(
                                    $"Unhandled property: {property.PropertyName} for object car (TAGame.Car_TA); data: {property.Data}");
                                break;
                        }

                    if (obj is Player player)
                        switch (property.PropertyName) {
                            case "Engine.PlayerReplicationInfo:PlayerName":
                                player.Name = (string)property.Data;
                                break;
                            case "TAGame.PRI_TA:MatchScore":
                                player.MatchScore = (uint)property.Data;
                                break;
                            case "Engine.PlayerReplicationInfo:Score":
                                player.Score = (uint)property.Data;
                                break;
                            case "TAGame.PRI_TA:MatchAssists":
                                player.Assists = (uint)property.Data;
                                break;
                            case "TAGame.PRI_TA:MatchSaves":
                                player.Saves = (uint)property.Data;
                                break;
                            case "TAGame.PRI_TA:MatchGoals":
                                player.Goals = (uint)property.Data;
                                break;
                            case "TAGame.PRI_TA:MatchShots":
                                player.Shots = (uint)property.Data;
                                break;
                            case "TAGame.PRI_TA:ClientLoadouts":
                                player.Loadout = (ClientLoadouts)property.Data;
                                break;
                            case "TAGame.PRI_TA:ReplicatedGameEvent": break;
                            case "TAGame.PRI_TA:bReady":
                                player.IsReady = (bool)property.Data;
                                break;
                            case "Engine.PlayerReplicationInfo:Ping":
                                player.Ping = (byte)property.Data;
                                break;
                            case "Engine.PlayerReplicationInfo:Team":
                                player.Team = (ActiveActor)property.Data;
                                break;
                            case "Engine.PlayerReplicationInfo:UniqueId":
                                player.UniqueId = (UniqueId)property.Data;
                                break;
                            case "Engine.PlayerReplicationInfo:PlayerID":
                                player.PlayerId = (uint)property.Data;
                                break;
                            case "TAGame.PRI_TA:CameraSettings":
                                player.Camera = (ActiveActor)property.Data;
                                break;
                            case "TAGame.PRI_TA:ClientLoadoutsOnline":
                                player.LoadoutsOnline = (ClientLoadoutsOnline)property.Data;
                                break;
                            case "TAGame.PRI_TA:SpectatorShortcut":
                                player.SpecShortcut = (uint)property.Data;
                                break;
                            case "TAGame.PRI_TA:ClubID":
                                player.ClubId = (ulong)property.Data;
                                break;
                            case "TAGame.PRI_TA:Title":
                                player.TitleId = (uint)property.Data;
                                break;
                            case "TAGame.PRI_TA:PartyLeader":
                                player.PartyLeader = (PartyLeader)property.Data;
                                break;
                            case "TAGame.PRI_TA:SteeringSensitivity":
                                player.SteeringSensitivity = (float)property.Data;
                                break;
                            case "TAGame.PRI_TA:PersistentCamera":
                                player.Camera = (ActiveActor)property.Data;
                                break;
                            case "TAGame.PRI_TA:PlayerHistoryValid":
                                player.HistoryValid = (bool)property.Data;
                                break;

                            default:
                                Console.WriteLine(
                                    $"Unhandled property: {property.PropertyName} for object player (TAGame.PRI_TA); data: {property.Data}");
                                break;
                        }
                }

            if (actor.State == ActorStateState.Deleted && Objects.ContainsKey(actorId)) Objects.Remove(actorId);
        }
    }
}