namespace RLReplayWatcher.data;

internal sealed class ClientLoadout : RLRPClientLoadout {
    public new byte Version { get; set; }
    public new uint BodyProductId { get; set; }
    public new uint SkinProductId { get; set; }
    public new uint WheelProductId { get; set; }
    public new uint BoostProductId { get; set; }
    public new uint AntennaProductId { get; set; }
    public new uint HatProductId { get; set; }
    public new uint Unknown2 { get; set; }
    public new uint Unknown3 { get; set; }
    public new uint EngineAudioProductId { get; set; }
    public new uint TrailProductId { get; set; }
    public new uint GoalExplosionProductId { get; set; }
    public new uint BannerProductId { get; set; }
    public new uint Unknown4 { get; set; }
    public new uint Unknown5 { get; set; }
    public new uint Unknown6 { get; set; }
    public new uint Unknown7 { get; set; }

    public ClientLoadout Clone() {
        return new ClientLoadout {
            Version = Version,
            BodyProductId = BodyProductId,
            SkinProductId = SkinProductId,
            WheelProductId = WheelProductId,
            BoostProductId = BoostProductId,
            AntennaProductId = AntennaProductId,
            HatProductId = HatProductId,
            Unknown2 = Unknown2,
            Unknown3 = Unknown3,
            EngineAudioProductId = EngineAudioProductId,
            TrailProductId = TrailProductId,
            GoalExplosionProductId = GoalExplosionProductId,
            BannerProductId = BannerProductId,
            Unknown4 = Unknown4,
            Unknown5 = Unknown5,
            Unknown6 = Unknown6,
            Unknown7 = Unknown7
        };
    }
}