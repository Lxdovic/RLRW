namespace RLReplayWatcher.data;

internal sealed class TeamPaint : RLRPTeamPaint {
    public new byte TeamNumber { get; set; }
    public new byte TeamColorId { get; set; }
    public new byte CustomColorId { get; set; }
    public new uint TeamFinishId { get; set; }
    public new uint CustomFinishId { get; set; }

    
    public TeamPaint Clone() {
        return new TeamPaint {
            TeamNumber = TeamNumber,
            TeamColorId = TeamColorId,
            CustomColorId = CustomColorId,
            TeamFinishId = TeamFinishId,
            CustomFinishId = CustomFinishId,
        };
    }
}