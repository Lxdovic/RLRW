using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class Player(int id) {
    public int Id { get; set; } = id;
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
}