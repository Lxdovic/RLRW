using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

namespace RLReplayWatcher.ui.scene;

internal sealed class Scene {
    private readonly List<Vector3> _ballPositionHistory = [];
    private readonly RenderTexture2D _viewTexture;
    private Camera3D _camera;

    private Model _fennecBlue =
        Raylib.LoadModel(Path.Combine(Environment.CurrentDirectory, "resources/models/cars/fennec-blue.glb"));

    private Model _fennecOrange =
        Raylib.LoadModel(Path.Combine(Environment.CurrentDirectory, "resources/models/cars/fennec-orange.glb"));

    internal Scene() {
        _viewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        Raylib.SetTextureFilter(_viewTexture.Texture, TextureFilter.Anisotropic16X);

        _camera.FovY = 50;
        _camera.Up.Y = 1;
        _camera.Position.Y = 10;
        _camera.Position.Z = 10;
    }

    private List<(Vector2, string, Color)> RenderPlayers() {
        var playerTags = new List<(Vector2, string, Color)>();
        var frame = Program.Game!.Frames[Program.Game.FrameIndex];

        foreach (var (id, car) in frame.CarActors) {
            var color = car.TeamPaint?.TeamNumber switch {
                0 => Color.Blue,
                1 => Color.Orange,
                _ => Color.White
            };

            if (car.Hidden) continue;

            if (car.TeamPaint?.TeamNumber == 0) {
                _fennecBlue.Transform = Matrix4x4.CreateFromQuaternion(car.Rotation);
                Raylib.DrawModel(_fennecBlue, car.Position, 0.01f, Color.White);
            }
            else {
                _fennecOrange.Transform = Matrix4x4.CreateFromQuaternion(car.Rotation);
                Raylib.DrawModel(_fennecOrange, car.Position, 0.01f, Color.White);
            }

            if (car.PlayerActor != null && frame.PlayerActors.TryGetValue(car.PlayerActor.ActorId, out var player))
                playerTags.Add((
                    Raylib.GetWorldToScreen(car.Position + new Vector3(0, 1, 0), _camera),
                    player.Name, color));
        }

        return playerTags;
    }

    private List<(Vector2, string, Color)> RenderBoosts() {
        var boostTags = new List<(Vector2, string, Color)>();
        var frame = Program.Game!.Frames[Program.Game.FrameIndex];

        foreach (var (id, boost) in frame.BoostActors) {
            if (boost.ReplicatedBoost == null || boost.Vehicle == null) continue;
            
            var car = frame.CarActors[boost.Vehicle.ActorId];
            var color = car.TeamPaint?.TeamNumber switch {
                0 => Color.Blue,
                1 => Color.Orange,
                _ => Color.White
            };
            
            var boostAmount = boost.ReplicatedBoost.BoostAmount;
            
            boostTags.Add((
                Raylib.GetWorldToScreen(car.Position + new Vector3(0, 1, 0), _camera),
                ((int)(boostAmount / 2.55)).ToString(CultureInfo.InvariantCulture), color));
        }

        return boostTags;
;
    }

    private void RenderBalls() {
        var frame = Program.Game!.Frames[Program.Game.FrameIndex];

        foreach (var (id, ball) in frame.BallActors) {
            var color = ball.HitTeamNum switch {
                0 => Color.Blue,
                1 => Color.Orange,
                _ => Color.White
            };

            Raylib.DrawSphere(ball.Position, 1, color);

            if (_ballPositionHistory.LastOrDefault() != ball.Position)
                _ballPositionHistory.Add(ball.Position);

            if (_ballPositionHistory.Count > 15) _ballPositionHistory.RemoveAt(0);

            for (var i = 0; i < _ballPositionHistory.Count - 1; i++)
                Raylib.DrawLine3D(_ballPositionHistory[i], _ballPositionHistory[i + 1], color);
        }
    }

    internal void Render() {
        if (Program.Game == null) return;

        Raylib.BeginTextureMode(_viewTexture);
        Raylib.ClearBackground(new Color(0, 0, 0, 0));
        Raylib.BeginMode3D(_camera);

        RenderBalls();
        var playerTags = RenderPlayers();
        var boostTags = RenderBoosts();

        Raylib.DrawGrid(50, 4);
        Raylib.EndMode3D();

        foreach (var (pos, text, color) in playerTags) {
            var textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text + "w", 20, 0);
            var rect = new Rectangle(
                pos.X - textSize.X / 2 - 10,
                pos.Y - textSize.Y,
                textSize.X + 20,
                textSize.Y
            );

            Raylib.DrawRectangleRounded(rect, 0.25f, 4, color);
            Raylib.DrawRectangleRoundedLines(rect, 0.25f, 4, 2, Color.White);
            Raylib.DrawText(text, (int)pos.X - (int)textSize.X / 2, (int)pos.Y - (int)textSize.Y, 20, Color.White);
        }

        foreach (var (pos, text, color) in boostTags) {
            var textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text, 20, 0);
            Raylib.DrawText(text, (int)pos.X - (int)textSize.X / 2, (int)pos.Y - (int)textSize.Y * 2, 20, Color.White);
        }

        Raylib.EndTextureMode();

        rlImGui.ImageRenderTextureFit(_viewTexture, false);

        if (ImGui.IsItemClicked()) Raylib.DisableCursor();
        if (Raylib.IsKeyPressed(KeyboardKey.Escape)) Raylib.EnableCursor();
    }

    internal void Update(Stopwatch stopwatch, double time) {
        if (Raylib.IsCursorHidden()) HandleControls();

        if (stopwatch.IsRunning) Program.Game?.TryNextFrame(stopwatch.ElapsedMilliseconds + time);
    }

    private void HandleControls() {
        Raylib.UpdateCamera(ref _camera, CameraMode.Free);
    }

    internal void Unload() {
        Raylib.UnloadRenderTexture(_viewTexture);
    }
}