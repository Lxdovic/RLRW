using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Game : GameEntity {
    public uint GamePlaylist { get; set; }
    public ObjectTarget? GameClass { get; set; }
    public string? ServerName { get; set; }
    public string? MatchGuid { get; set; }
    public bool GameStarted { get; set; }
    public string? GameServerId { get; set; }
    public List<Reservation> Reservations { get; set; } = [];
    public string? ServerRegion { get; set; }
    public int GameMutatorIndex { get; set; }

    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            case "ProjectX.GRI_X:ReplicatedGamePlaylist":
                GamePlaylist = (uint)property.Data;
                break;
            case "Engine.GameReplicationInfo:GameClass":
                GameClass = (ObjectTarget)property.Data;
                break;
            case "Engine.GameReplicationInfo:ServerName":
                ServerName = (string)property.Data;
                break;
            case "ProjectX.GRI_X:MatchGuid":
                MatchGuid = (string)property.Data;
                break;
            case "ProjectX.GRI_X:bGameStarted":
                GameStarted = (bool)property.Data;
                break;
            case "ProjectX.GRI_X:GameServerID":
                GameServerId = (string)property.Data;
                break;
            case "ProjectX.GRI_X:Reservations":
                Reservations = ((List<object>)property.Data).OfType<Reservation>().ToList();
                break;
            case "ProjectX.GRI_X:ReplicatedServerRegion":
                ServerRegion = (string)property.Data;
                break;
            case "ProjectX.GRI_X:ReplicatedGameMutatorIndex":
                GameMutatorIndex = (int)property.Data;
                break;

            default:
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} for object game (TAGame.GRI_TA); data: {property.Data}, type: {property.Data.GetType()}");
                break;
        }
    }
}