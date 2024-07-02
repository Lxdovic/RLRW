using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class GameActor(ActorState? actor = null) : Actor {
    public uint GamePlaylist { get; set; }
    public ObjectTarget? GameClass { get; set; }
    public string? ServerName { get; set; }
    public string? MatchGuid { get; set; }
    public bool GameStarted { get; set; }
    public string? GameServerId { get; set; }
    public List<Reservation>? Reservations { get; set; }
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
            Reservations = Reservations?.Select(res => res.Clone()).ToList(),
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
                if (property.Data is RLRPReservation reservation)
                    Reservations?.Add(new Reservation {
                        Unknown1 = reservation.Unknown1,
                        PlayerId = new UniqueId {
                            Type = reservation.PlayerId.Type,
                            PlayerNumber = reservation.PlayerId.PlayerNumber,
                            Id = reservation.PlayerId.Id
                        },
                        Unknown2 = reservation.Unknown2,
                        PlayerName = reservation.PlayerName
                    });

                if (property.Data is List<RLRPReservation> reservations)
                    Reservations = reservations.Select(res => new Reservation {
                        Unknown1 = res.Unknown1,
                        PlayerId = new UniqueId {
                            Type = res.PlayerId.Type,
                            PlayerNumber = res.PlayerId.PlayerNumber,
                            Id = res.PlayerId.Id
                        },
                        Unknown2 = res.Unknown2,
                        PlayerName = res.PlayerName
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