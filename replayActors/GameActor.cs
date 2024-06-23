using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class GameActor : Actor {
    public uint GamePlaylist { get; set; }
    public ObjectTarget? GameClass { get; set; }
    public string? ServerName { get; set; }
    public string? MatchGuid { get; set; }
    public bool GameStarted { get; set; }
    public string? GameServerId { get; set; }
    public List<Reservation> Reservations { get; set; } = [];
    public string? ServerRegion { get; set; }
    public int GameMutatorIndex { get; set; }

    public override GameActor Clone() {
        return new GameActor {
            GamePlaylist = GamePlaylist,
            GameClass = GameClass?.Clone(),
            ServerName = ServerName,
            MatchGuid = MatchGuid,
            GameStarted = GameStarted,
            GameServerId = GameServerId,
            Reservations = Reservations.Select(x => x.Clone()).ToList(),
            ServerRegion = ServerRegion,
            GameMutatorIndex = GameMutatorIndex
        };
    }

    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            case "ProjectX.GRI_X:ReplicatedGamePlaylist":
                GamePlaylist = (uint)property.Data;
                break;
            case "Engine.GameReplicationInfo:GameClass":
                var gameClass = (RLRPObjectTarget)property.Data;

                GameClass = new ObjectTarget {
                    Unknown1 = gameClass.Unknown1,
                    ObjectIndex = gameClass.ObjectIndex
                };
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
                var reservations = ((List<object>)property.Data).OfType<RLRPReservation>().ToList();

                Reservations = reservations.Select(x => new Reservation {
                    Unknown1 = x.Unknown1,
                    PlayerId = new UniqueId {
                        Id = x.PlayerId.Id,
                        PlayerNumber = x.PlayerId.PlayerNumber,
                        Type = x.PlayerId.Type
                    },
                    PlayerName = x.PlayerName,
                    Unknown2 = x.Unknown2
                }).ToList();
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