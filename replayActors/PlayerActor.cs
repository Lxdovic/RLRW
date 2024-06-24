using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class PlayerActor : Actor {
    public string Name { get; set; } = "";
    public ActiveActor? Team { get; set; }
    public uint Score { get; set; }
    public uint MatchScore { get; set; }
    public uint Assists { get; set; }
    public uint Saves { get; set; }
    public uint Goals { get; set; }
    public uint Shots { get; set; }
    public ClientLoadouts? Loadouts { get; set; }
    public bool IsReady { get; set; }
    public byte Ping { get; set; }
    public UniqueId? UniqueId { get; set; }
    public uint PlayerId { get; set; }
    public uint SpecShortcut { get; set; }
    public ulong ClubId { get; set; }
    public uint TitleId { get; set; }
    public UniqueId? PartyLeader { get; set; }
    public ActiveActor? Camera { get; set; }
    public ActiveActor? PersistentCamera { get; set; }
    public ClientLoadoutsOnline? LoadoutsOnline { get; set; }
    public float SteeringSensitivity { get; set; }
    public bool HistoryValid { get; set; }
    public string? CurrentVoiceRoom { get; set; }
    public byte PawnType { get; set; }
    public string? RemoteUserData { get; set; }
    public bool IsDistracted { get; set; }
    public byte WorstNetworkQualityBeyondLatency { get; set; }
    public List<uint>? HistoryKey { get; set; }

    public override PlayerActor Clone() {
        return new PlayerActor {
            Name = Name,
            Team = Team?.Clone(),
            Score = Score,
            MatchScore = MatchScore,
            Assists = Assists,
            Saves = Saves,
            Goals = Goals,
            Shots = Shots,
            Loadouts = Loadouts?.Clone(),
            IsReady = IsReady,
            Ping = Ping,
            UniqueId = UniqueId?.Clone(),
            PlayerId = PlayerId,
            SpecShortcut = SpecShortcut,
            ClubId = ClubId,
            TitleId = TitleId,
            PartyLeader = PartyLeader?.Clone(),
            Camera = Camera?.Clone(),
            LoadoutsOnline = LoadoutsOnline?.Clone(),
            SteeringSensitivity = SteeringSensitivity,
            HistoryValid = HistoryValid,
            CurrentVoiceRoom = CurrentVoiceRoom,
            PawnType = PawnType,
            RemoteUserData = RemoteUserData,
            IsDistracted = IsDistracted,
            PersistentCamera = PersistentCamera?.Clone(),
            WorstNetworkQualityBeyondLatency = WorstNetworkQualityBeyondLatency,
            HistoryKey = [..HistoryKey ?? []]
        };
    }

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
                var clientLoadouts = (RLRPClientLoadouts)property.Data;

                var loadoutOne = clientLoadouts.Loadout1;
                var loadoutTwo = clientLoadouts.Loadout2;
                
                Loadouts = new ClientLoadouts {
                    Loadout1 = new ClientLoadout {
                        Unknown2 = loadoutOne.Unknown2,
                        Unknown3 = loadoutOne.Unknown3,
                        Unknown4 = loadoutOne.Unknown4,
                        Unknown5 = loadoutOne.Unknown5,
                        Unknown6 = loadoutOne.Unknown6,
                        Unknown7 = loadoutOne.Unknown7,
                        BannerProductId = loadoutOne.BannerProductId,
                        BoostProductId = loadoutOne.BoostProductId,
                        AntennaProductId = loadoutOne.AntennaProductId,
                        HatProductId = loadoutOne.HatProductId,
                        BodyProductId = loadoutOne.BodyProductId,
                        SkinProductId = loadoutOne.SkinProductId,
                        WheelProductId = loadoutOne.BannerProductId,
                        TrailProductId = loadoutOne.TrailProductId,
                        EngineAudioProductId = loadoutOne.EngineAudioProductId,
                        GoalExplosionProductId = loadoutOne.GoalExplosionProductId,
                        Version = loadoutOne.Version
                    },
                    
                    Loadout2 = new ClientLoadout {
                        Unknown2 = loadoutTwo.Unknown2,
                        Unknown3 = loadoutTwo.Unknown3,
                        Unknown4 = loadoutTwo.Unknown4,
                        Unknown5 = loadoutTwo.Unknown5,
                        Unknown6 = loadoutTwo.Unknown6,
                        Unknown7 = loadoutTwo.Unknown7,
                        BannerProductId = loadoutTwo.BannerProductId,
                        BoostProductId = loadoutTwo.BoostProductId,
                        AntennaProductId = loadoutTwo.AntennaProductId,
                        HatProductId = loadoutTwo.HatProductId,
                        BodyProductId = loadoutTwo.BodyProductId,
                        SkinProductId = loadoutTwo.SkinProductId,
                        WheelProductId = loadoutTwo.BannerProductId,
                        TrailProductId = loadoutTwo.TrailProductId,
                        EngineAudioProductId = loadoutTwo.EngineAudioProductId,
                        GoalExplosionProductId = loadoutTwo.GoalExplosionProductId,
                        Version = loadoutTwo.Version
                    }
                };
                break;
            case "TAGame.PRI_TA:ReplicatedGameEvent": break;
            case "TAGame.PRI_TA:bReady":
                IsReady = (bool)property.Data;
                break;
            case "Engine.PlayerReplicationInfo:Ping":
                Ping = (byte)property.Data;
                break;
            case "Engine.PlayerReplicationInfo:Team":
                var team = (RLRPActiveActor)property.Data;

                Team = new ActiveActor {
                    Active = team.Active,
                    ActorId = team.ActorId
                };
                break;
            case "Engine.PlayerReplicationInfo:UniqueId":
                var uniqueId = (RLRPUniqueId)property.Data;

                UniqueId = new UniqueId {
                    Type = uniqueId.Type,
                    Id = uniqueId.Id,
                    PlayerNumber = uniqueId.PlayerNumber
                };
                break;
            case "Engine.PlayerReplicationInfo:PlayerID":
                PlayerId = (uint)property.Data;
                break;
            case "TAGame.PRI_TA:CameraSettings":
                var camera = (RLRPActiveActor)property.Data;

                Camera = new ActiveActor {
                    Active = camera.Active,
                    ActorId = camera.ActorId
                };
                break;
            case "TAGame.PRI_TA:ClientLoadoutsOnline":
                var loadoutsOnline = (RLRPClientLoadoutsOnline)property.Data;

                var loadoutOnlineOne = (RLRPClientLoadoutOnline?)loadoutsOnline.LoadoutOnline1;
                var loadoutOnlineTwo = (RLRPClientLoadoutOnline?)loadoutsOnline.LoadoutOnline2;

                LoadoutsOnline = new ClientLoadoutsOnline {
                    LoadoutOnline1 = new ClientLoadoutOnline {
                        ProductAttributeLists = loadoutOnlineOne?.ProductAttributeLists.Select(x => x.Select(y => new ProductAttribute() {
                            Unknown1 = y.Unknown1,
                            ClassIndex = y.ClassIndex,
                            ClassName = y.ClassName,
                            HasValue = y.HasValue,
                            Value = y.Value
                        }).ToList()).ToList()
                    },
                    LoadoutOnline2 = new ClientLoadoutOnline {
                        ProductAttributeLists = loadoutOnlineTwo?.ProductAttributeLists.Select(x => x.Select(y => new ProductAttribute() {
                            Unknown1 = y.Unknown1,
                            ClassIndex = y.ClassIndex,
                            ClassName = y.ClassName,
                            HasValue = y.HasValue,
                            Value = y.Value
                        }).ToList()).ToList()
                    },
                    Unknown1 = loadoutsOnline.Unknown1,
                    Unknown2 = loadoutsOnline.Unknown2
                };
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
                var partyLeader = (RLRPPartyLeader)property.Data;

                PartyLeader = new UniqueId {
                    Type = partyLeader.Type,
                    Id = partyLeader.Id,
                    PlayerNumber = partyLeader.PlayerNumber
                };
                break;
            case "TAGame.PRI_TA:SteeringSensitivity":
                SteeringSensitivity = (float)property.Data;
                break;
            case "TAGame.PRI_TA:PersistentCamera":
                var persistentCamera = (RLRPActiveActor)property.Data;

                PersistentCamera = new ActiveActor {
                    Active = persistentCamera.Active,
                    ActorId = persistentCamera.ActorId
                };
                break;
            case "TAGame.PRI_TA:PlayerHistoryValid":
                HistoryValid = (bool)property.Data;
                break;
            case "TAGame.PRI_TA:CurrentVoiceRoom":
                CurrentVoiceRoom = (string)property.Data;
                break;
            case "TAGame.PRI_TA:PawnType":
                PawnType = (byte)property.Data;
                break;
            case "Engine.PlayerReplicationInfo:RemoteUserData":
                RemoteUserData = (string)property.Data;
                break;
            case "TAGame.PRI_TA:bIsDistracted":
                IsDistracted = (bool)property.Data;
                break;
            case "TAGame.PRI_TA:ReplicatedWorstNetQualityBeyondLatency":
                WorstNetworkQualityBeyondLatency = (byte)property.Data;
                break;
            case "TAGame.PRI_TA:PlayerHistoryKey":
                var historyKey = ((List<object>)property.Data).Select(val=> (uint)val).ToList();

                HistoryKey = historyKey;
                break;
            default:
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} for object player (TAGame.PRI_TA); data: {property.Data}, type: {property.Data.GetType()}");
                break;
        }
    }
}