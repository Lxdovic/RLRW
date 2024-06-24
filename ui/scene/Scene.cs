using System.Diagnostics;
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
        Raylib.SetTextureFilter(_viewTexture.Texture, TextureFilter.Bilinear);

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
                    Raylib.GetWorldToScreen(car.Position + new Vector3(0, 4, 0), _camera),
                    player.Name, color));
        }

        return playerTags;
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

            if (_ballPositionHistory.Count > ball.LinearVelocity.Length() / 100) _ballPositionHistory.RemoveAt(0);

            for (var i = 0; i < _ballPositionHistory.Count - 1; i++)
                Raylib.DrawLine3D(_ballPositionHistory[i], _ballPositionHistory[i + 1], color);
        }
    }

    internal void Render() {
        if (Program.Game == null) return;

        Raylib.BeginTextureMode(_viewTexture);
        Raylib.ClearBackground(Color.SkyBlue);
        Raylib.BeginMode3D(_camera);

        RenderBalls();
        var playerTags = RenderPlayers();

        Raylib.DrawPlane(new Vector3(0, 0, 0), new Vector2(100, 100), Color.White);
        Raylib.EndMode3D();

        foreach (var (pos, text, color) in playerTags) {
            var textSize = Raylib.MeasureText(text, 20);
            Raylib.DrawText(text, (int)pos.X - textSize / 2, (int)pos.Y, 20, color);
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

        // if (Program.Game == null) return;
        //
        // var ball = Program.Game.Balls.FirstOrDefault().Value;
        // var car = Program.Game.Cars.FirstOrDefault().Value;
        //
        // if (ball == null || car == null) return;
        // if (car.PlayerActor == null) return;
        //
        // Program.Game.Players.TryGetValue(car.PlayerActor.ActorId, out var player);
        //
        // if (player == null || player.Camera == null) return;
        //
        // Program.Game.CameraSettingsActors.TryGetValue(player.Camera.ActorId, out var cameraSettings);
        //
        // if (cameraSettings == null || cameraSettings.ProfileSettings == null) return;
        //
        // _camera.Position = car.Position;
        // _camera.FovY = cameraSettings.ProfileSettings.FieldOfView;
        //
        // Raylib.CameraMoveForward(ref _camera, -cameraSettings.ProfileSettings.Distance / 100, true);
        // Raylib.CameraMoveUp(ref _camera, cameraSettings.ProfileSettings.Height / 100);
        //
        // _camera.Target = ball.Position;
    }

    internal void Unload() {
        Raylib.UnloadRenderTexture(_viewTexture);
    }
}