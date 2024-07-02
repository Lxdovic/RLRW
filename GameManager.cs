using System.Diagnostics;
using System.Reflection;
using RLReplayWatcher.replayActors;
using RLReplayWatcher.replayHelper;
using RocketLeagueReplayParser;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher;

internal sealed class Frame {
    public Frame(Frame? frame = null) {
        if (frame == null) return;

        BallActors = CloneActors(frame.BallActors);
        CarActors = CloneActors(frame.CarActors);
        BoostActors = CloneActors(frame.BoostActors);
        PlayerActors = CloneActors(frame.PlayerActors);
        GameActors = CloneActors(frame.GameActors);
        CameraSettingsActors = CloneActors(frame.CameraSettingsActors);
        Time = frame.Time;
    }

    public double Time { get; set; }
    public Dictionary<int, BallActor> BallActors { get; set; } = [];
    public Dictionary<int, CarActor> CarActors { get; set; } = [];
    public Dictionary<int, BoostActor> BoostActors { get; set; } = [];
    public Dictionary<int, PlayerActor> PlayerActors { get; set; } = [];
    public Dictionary<int, GameActor> GameActors { get; set; } = [];
    public Dictionary<int, CameraSettingsActor> CameraSettingsActors { get; set; } = [];

    private Dictionary<int, T> CloneActors<T>(Dictionary<int, T> actors) where T : Actor {
        return actors.ToDictionary(entry => entry.Key, entry => (T)entry.Value.Clone());
    }
}

internal sealed class GameManager(Replay replay) {
    public Replay Replay { get; } = replay;
    public int FrameIndex { get; set; }
    public List<Frame> Frames { get; set; } = [];

    public void Parse() {
        List<Frame> frames = [new Frame()];

        foreach (var frame in Replay.Frames) {
            var parsedFrame = new Frame(frames.Last());

            foreach (var actor in frame.ActorStates) {
                var actorId = (int)actor.Id;

                if (actor.State == ActorStateState.New) {
                    var className = ReplayHelper.GetClass(Replay, actor)?.Class;

                    switch (className) {
                        case "TAGame.Ball_TA":
                            parsedFrame.BallActors.TryAdd(actorId,
                                new BallActor(actor));
                            break;
                        case "TAGame.Car_TA":
                            parsedFrame.CarActors.TryAdd(actorId,
                                new CarActor(actor));
                            break;
                        case "TAGame.PRI_TA":
                            parsedFrame.PlayerActors.TryAdd(actorId, new PlayerActor());
                            break;
                        case "TAGame.GRI_TA":
                            parsedFrame.GameActors.TryAdd(actorId, new GameActor());
                            break;
                        case "TAGame.CameraSettingsActor_TA":
                            parsedFrame.CameraSettingsActors.TryAdd(actorId, new CameraSettingsActor());
                            break;
                        case "TAGame.CarComponent_Boost_TA":
                            parsedFrame.BoostActors.TryAdd(actorId, new BoostActor());
                            break;
                    }
                }

                if (actor.State == ActorStateState.Existing) {
                    if (parsedFrame.CarActors.TryGetValue(actorId, out var car))
                        foreach (var (_, property) in actor.Properties)
                            car.HandleGameEvents(property);

                    if (parsedFrame.BallActors.TryGetValue(actorId, out var ball))
                        foreach (var (_, property) in actor.Properties)
                            ball.HandleGameEvents(property);

                    if (parsedFrame.BoostActors.TryGetValue(actorId, out var boost))
                        foreach (var (_, property) in actor.Properties)
                            boost.HandleGameEvents(property);

                    if (parsedFrame.PlayerActors.TryGetValue(actorId, out var player))
                        foreach (var (_, property) in actor.Properties)
                            player.HandleGameEvents(property);

                    if (parsedFrame.GameActors.TryGetValue(actorId, out var game))
                        foreach (var (_, property) in actor.Properties)
                            game.HandleGameEvents(property);

                    if (parsedFrame.CameraSettingsActors.TryGetValue(actorId, out var cameraSettingsActor))
                        foreach (var (_, property) in actor.Properties)
                            cameraSettingsActor.HandleGameEvents(property);
                }

                if (actor.State == ActorStateState.Deleted) {
                    if (parsedFrame.CarActors.ContainsKey(actorId)) parsedFrame.CarActors.Remove(actorId);
                    if (parsedFrame.BallActors.ContainsKey(actorId)) parsedFrame.BallActors.Remove(actorId);
                    if (parsedFrame.BoostActors.ContainsKey(actorId)) parsedFrame.BoostActors.Remove(actorId);
                    if (parsedFrame.PlayerActors.ContainsKey(actorId)) parsedFrame.PlayerActors.Remove(actorId);
                    if (parsedFrame.GameActors.ContainsKey(actorId)) parsedFrame.GameActors.Remove(actorId);
                    if (parsedFrame.CameraSettingsActors.ContainsKey(actorId))
                        parsedFrame.CameraSettingsActors.Remove(actorId);
                }
            }

            frames.Add(parsedFrame);
        }

        Frames = frames;
    }

    public void TryNextFrame(double time) {
        if (FrameIndex >= Replay.Frames.Count - 1 || FrameIndex < 0) return;

        if (Replay.Frames[FrameIndex].Time < time / 1000) FrameIndex++;
    }
}