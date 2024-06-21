using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;
using RLReplayWatcher.replayActors;

namespace RLReplayWatcher.ui.scene;

internal sealed class Scene {
    private readonly RenderTexture2D _viewTexture;
    private Camera3D _camera;
    private Mesh _test = Raylib.GenMeshCube(1, 1, 1);
    private Matrix4x4 _transform = new();

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
                
                var carPos = new Vector3(car.RigidBody?.Position.X ?? 0, car.RigidBody?.Position.Z ?? 0, car.RigidBody?.Position.Y ?? 0) / 100;
                var rotation = (RocketLeagueReplayParser.NetworkStream.Quaternion)car.RigidBody?.Rotation!;
                var carRotation = new Quaternion(rotation?.X ?? Quaternion.Identity.X, rotation?.Z ?? Quaternion.Identity.Z, rotation?.Y  ?? Quaternion.Identity.Y, rotation?.W  ?? Quaternion.Identity.W);
                
                Raylib.DrawMesh(_test, new Material(), Matrix4x4.CreateScale(1, 1, 1) * Matrix4x4.CreateFromQuaternion(carRotation) * Matrix4x4.CreateTranslation(carPos));
                
                // Raylib.DrawMesh(_test, new Material(), _transform);
                // Raylib.DrawCube(carPos, 1, 1, 1, color);
                
                if (car.PlayerActor != null) {
                    if (Program.Game.Objects.TryGetValue(car.PlayerActor.ActorId, out var player)) {
                        if (player is Player playerObj) {
                            playerTags.Add((
                                Raylib.GetWorldToScreen(carPos + new Vector3(0, 4, 0), _camera),
                                playerObj.Name));
                        }
                    }
                }
            }

            if (obj is Ball ball) {
                var ballPos = new Vector3(ball.RigidBody?.Position.X ?? 0, ball.RigidBody?.Position.Z ?? 0, ball.RigidBody?.Position.Y ?? 0) / 100;

                Raylib.DrawSphere(ballPos, 1, Color.Gold);
            }
        }
        
        Raylib.DrawPlane(new Vector3(0, 0, 0), new Vector2(100, 100), Color.White);

        Raylib.EndMode3D();

        foreach (var (pos, text) in playerTags) {
            Raylib.DrawText(text, (int)pos.X, (int)pos.Y, 20, Color.Red);
        }
        
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