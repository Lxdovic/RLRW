using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;
using RLReplayWatcher.replayActors;

namespace RLReplayWatcher.ui.scene;

internal sealed class Scene {
    private readonly RenderTexture2D _viewTexture;
    private Camera3D _camera;

    private Model _car =
        Raylib.LoadModel(Path.Combine(Environment.CurrentDirectory, "resources/models/cars/octane.glb"));

    internal Scene() {
        _viewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        Raylib.SetTextureFilter(_viewTexture.Texture, TextureFilter.Bilinear);

        _camera.FovY = 45;
        _camera.Up.Y = 1;
        _camera.Position.Y = 10;
        _camera.Position.Z = 10;
    }

    internal void Render() {
        rlImGui.ImageRenderTextureFit(_viewTexture, false);

        if (ImGui.IsItemClicked()) Raylib.DisableCursor();
        if (Raylib.IsKeyPressed(KeyboardKey.Escape)) Raylib.EnableCursor();
    }

    internal void Update() {
        if (Raylib.IsCursorHidden()) HandleControls();

        Raylib.BeginTextureMode(_viewTexture);
        Raylib.ClearBackground(Color.SkyBlue);
        Raylib.BeginMode3D(_camera);

        var playerTags = new List<(Vector2, string)>();

        foreach (var key in Program.Game?.Objects.Keys!) {
            Program.Game.Objects.TryGetValue(key, out var obj);

            if (obj is Car car) {
                var color = car.TeamPaint?.TeamNumber switch {
                    0 => Color.Blue,
                    1 => Color.Orange,
                    _ => Color.White
                };
                
                if (car.Hidden) continue;

                _car.Transform =
                    Matrix4x4.CreateFromQuaternion(car.Rotation) * Matrix4x4.CreateFromAxisAngle(new Vector3(1, 0, 0), MathF.PI / 2);
                
                Raylib.DrawModel(_car, car.Position, 1, color);

                if (car.PlayerActor != null)
                    if (Program.Game.Objects.TryGetValue(car.PlayerActor.ActorId, out var player))
                        if (player is Player playerObj)
                            playerTags.Add((
                                Raylib.GetWorldToScreen(car.Position + new Vector3(0, 4, 0), _camera),
                                playerObj.Name));
            }

            if (obj is Ball ball) {
                Raylib.DrawSphere(ball.Position, 1, Color.Gold);
            }
        }

        Raylib.DrawPlane(new Vector3(0, 0, 0), new Vector2(100, 100), Color.White);

        Raylib.EndMode3D();

        foreach (var (pos, text) in playerTags) Raylib.DrawText(text, (int)pos.X, (int)pos.Y, 20, Color.Red);

        Raylib.EndTextureMode();

        Program.Game.TryNextFrame(Raylib.GetTime());
    }

    private void HandleControls() {
        Raylib.UpdateCamera(ref _camera, CameraMode.Free);
    }

    internal void Unload() {
        Raylib.UnloadRenderTexture(_viewTexture);
    }
}