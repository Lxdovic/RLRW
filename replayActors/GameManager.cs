using RLReplayWatcher.replayHelper;
using RocketLeagueReplayParser;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class GameManager(Replay replay) {
    public readonly Dictionary<int, BallActor> Balls = [];
    public readonly Dictionary<int, BoostActor> Boosts = [];
    public readonly Dictionary<int, CameraSettingsActor> CameraSettingsActors = [];
    public readonly Dictionary<int, CarActor> Cars = [];
    public readonly Dictionary<int, GameActor> Games = [];
    public readonly Dictionary<int, PlayerActor> Players = [];

    private Replay Replay { get; } = replay;
    private int FrameIndex { get; set; }

    public void TryNextFrame(double time) {
        if (FrameIndex >= Replay.Frames.Count - 1 || FrameIndex < 0) return;

        if (Replay.Frames[FrameIndex].Time < time) FrameIndex++;

        var frame = Replay.Frames[FrameIndex];

        foreach (var actor in frame.ActorStates) {
            var actorId = (int)actor.Id;

            if (actor.State == ActorStateState.New) {
                var className = ReplayHelper.GetClass(Replay, actor)?.Class;

                switch (className) {
                    case "TAGame.Ball_TA":
                        Balls.TryAdd(actorId,
                            new BallActor(actor.Position));
                        // Objects.TryAdd(actorId,
                        // new Ball(actor.Position));
                        break;
                    case "TAGame.Car_TA":
                        Cars.TryAdd(actorId,
                            new CarActor(actor.Position));
                        break;
                    case "TAGame.PRI_TA":
                        Players.TryAdd(actorId, new PlayerActor());
                        break;
                    case "TAGame.GRI_TA":
                        Games.TryAdd(actorId, new GameActor());
                        break;
                    case "TAGame.CameraSettingsActor_TA":
                        CameraSettingsActors.TryAdd(actorId, new CameraSettingsActor());
                        break;
                    case "TAGame.CarComponent_Boost_TA":
                        Boosts.TryAdd(actorId, new BoostActor());
                        break;
                }
            }

            if (actor.State == ActorStateState.Existing) {
                if (Cars.TryGetValue(actorId, out var car))
                    foreach (var (_, property) in actor.Properties)
                        car.HandleGameEvents(property);

                if (Balls.TryGetValue(actorId, out var ball))
                    foreach (var (_, property) in actor.Properties)
                        ball.HandleGameEvents(property);

                if (Boosts.TryGetValue(actorId, out var boost))
                    foreach (var (_, property) in actor.Properties)
                        boost.HandleGameEvents(property);

                if (Players.TryGetValue(actorId, out var player))
                    foreach (var (_, property) in actor.Properties)
                        player.HandleGameEvents(property);

                if (Games.TryGetValue(actorId, out var game))
                    foreach (var (_, property) in actor.Properties)
                        game.HandleGameEvents(property);

                if (CameraSettingsActors.TryGetValue(actorId, out var cameraSettingsActor))
                    foreach (var (_, property) in actor.Properties)
                        cameraSettingsActor.HandleGameEvents(property);
            }

            if (actor.State == ActorStateState.Deleted) {
                if (Cars.ContainsKey(actorId)) Cars.Remove(actorId);
                if (Balls.ContainsKey(actorId)) Balls.Remove(actorId);
                if (Boosts.ContainsKey(actorId)) Boosts.Remove(actorId);
                if (Players.ContainsKey(actorId)) Players.Remove(actorId);
                if (Games.ContainsKey(actorId)) Games.Remove(actorId);
                if (CameraSettingsActors.ContainsKey(actorId)) CameraSettingsActors.Remove(actorId);
            }
        }
    }
}