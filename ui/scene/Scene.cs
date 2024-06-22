using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;
using RLReplayWatcher.replayActors;

namespace RLReplayWatcher.ui.scene;

internal sealed class Scene {
    private readonly RenderTexture2D _viewTexture;
    private Camera3D _camera;
    private List<Vector3> _ballPositionHistory = [];

    private Model _fennecOrange =
        Raylib.LoadModel(Path.Combine(Environment.CurrentDirectory, "resources/models/cars/fennec-orange.glb"));

    private Model _fennecBlue =
        Raylib.LoadModel(Path.Combine(Environment.CurrentDirectory, "resources/models/cars/fennec-blue.glb"));

    internal Scene() {
        _viewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        Raylib.SetTextureFilter(_viewTexture.Texture, TextureFilter.Bilinear);

        _camera.FovY = 45;
        _camera.Up.Y = 1;
        _camera.Position.Y = 10;
        _camera.Position.Z = 10;
    }

    internal void Render() {
        Raylib.BeginTextureMode(_viewTexture);
        Raylib.ClearBackground(Color.SkyBlue);
        Raylib.BeginMode3D(_camera);

        var playerTags = new List<(Vector2, string, Color)>();

        foreach (var key in Program.Game?.Objects.Keys!) {
            Program.Game.Objects.TryGetValue(key, out var obj);

            if (obj is Car car) {
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

                if (car.PlayerActor != null)
                    if (Program.Game.Objects.TryGetValue(car.PlayerActor.ActorId, out var player))
                        if (player is Player playerObj)
                            playerTags.Add((
                                Raylib.GetWorldToScreen(car.Position + new Vector3(0, 4, 0), _camera),
                                playerObj.Name, color));
            }

            if (obj is Ball ball) {
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

        Raylib.DrawPlane(new Vector3(0, 0, 0), new Vector2(100, 100), Color.White);

        Raylib.EndMode3D();

        foreach (var (pos, text, color) in playerTags) Raylib.DrawText(text, (int)pos.X, (int)pos.Y, 20, color);

        Raylib.EndTextureMode();

        rlImGui.ImageRenderTextureFit(_viewTexture, false);

        if (ImGui.IsItemClicked()) Raylib.DisableCursor();
        if (Raylib.IsKeyPressed(KeyboardKey.Escape)) Raylib.EnableCursor();
    }

    internal void Update() {
        if (Raylib.IsCursorHidden()) HandleControls();

        Program.Game?.TryNextFrame(Raylib.GetTime());
    }

    private void HandleControls() {
        Raylib.UpdateCamera(ref _camera, CameraMode.Free);
    }

    internal void Unload() {
        Raylib.UnloadRenderTexture(_viewTexture);
    }
}