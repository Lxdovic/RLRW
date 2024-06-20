using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Player : GameEntity {
    public string Name { get; set; } = "";
    public ActiveActor? Team { get; set; }
    public uint Score { get; set; }
    public uint MatchScore { get; set; }
    public uint Assists { get; set; }
    public uint Saves { get; set; }
    public uint Goals { get; set; }
    public uint Shots { get; set; }
    public ClientLoadouts? Loadout { get; set; }
    public bool IsReady { get; set; }
    public byte Ping { get; set; }
    public UniqueId? UniqueId { get; set; }
    public uint PlayerId { get; set; }
    public uint SpecShortcut { get; set; }
    public ulong ClubId { get; set; }
    public uint TitleId { get; set; }
    public PartyLeader? PartyLeader { get; set; }
    public ActiveActor? Camera { get; set; }
    public ClientLoadoutsOnline? LoadoutsOnline { get; set; }
    public float SteeringSensitivity { get; set; }
    public bool HistoryValid { get; set; }

    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            case "Engine.PlayerReplicationInfo:PlayerName":
                Name = (string)property.Data;
                break;
            case "TAGame.PRI_TA:MatchScore":
                MatchScore = (uint)property.Data;
                break;
            case "Engine.PlayerReplicationInfo:Score":
                Score = (uint)property.Data;
                break;
            case "TAGame.PRI_TA:MatchAssists":
                Assists = (uint)property.Data;
                break;
            case "TAGame.PRI_TA:MatchSaves":
                Saves = (uint)property.Data;
                break;
            case "TAGame.PRI_TA:MatchGoals":
                Goals = (uint)property.Data;
                break;
            case "TAGame.PRI_TA:MatchShots":
                Shots = (uint)property.Data;
                break;
            case "TAGame.PRI_TA:ClientLoadouts":
                Loadout = (ClientLoadouts)property.Data;
                break;
            case "TAGame.PRI_TA:ReplicatedGameEvent": break;
            case "TAGame.PRI_TA:bReady":
                IsReady = (bool)property.Data;
                break;
            case "Engine.PlayerReplicationInfo:Ping":
                Ping = (byte)property.Data;
                break;
            case "Engine.PlayerReplicationInfo:Team":
                Team = (ActiveActor)property.Data;
                break;
            case "Engine.PlayerReplicationInfo:UniqueId":
                UniqueId = (UniqueId)property.Data;
                break;
            case "Engine.PlayerReplicationInfo:PlayerID":
                PlayerId = (uint)property.Data;
                break;
            case "TAGame.PRI_TA:CameraSettings":
                Camera = (ActiveActor)property.Data;
                break;
            case "TAGame.PRI_TA:ClientLoadoutsOnline":
                LoadoutsOnline = (ClientLoadoutsOnline)property.Data;
                break;
            case "TAGame.PRI_TA:SpectatorShortcut":
                SpecShortcut = (uint)property.Data;
                break;
            case "TAGame.PRI_TA:ClubID":
                ClubId = (ulong)property.Data;
                break;
            case "TAGame.PRI_TA:Title":
                TitleId = (uint)property.Data;
                break;
            case "TAGame.PRI_TA:PartyLeader":
                PartyLeader = (PartyLeader)property.Data;
                break;
            case "TAGame.PRI_TA:SteeringSensitivity":
                SteeringSensitivity = (float)property.Data;
                break;
            case "TAGame.PRI_TA:PersistentCamera":
                Camera = (ActiveActor)property.Data;
                break;
            case "TAGame.PRI_TA:PlayerHistoryValid":
                HistoryValid = (bool)property.Data;
                break;

            default:
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} for object player (TAGame.PRI_TA); data: {property.Data}");
                break;
        }
    }
}