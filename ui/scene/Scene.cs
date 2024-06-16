using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

namespace RLReplayWatcher.ui.scene;

internal sealed class Scene {
    private readonly RenderTexture2D _viewTexture;
    private Camera3D _camera;

    internal Scene() {
        _viewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        _camera.FovY = 45;
        _camera.Up.Y = 1;
        _camera.Position.Y = 3;
        _camera.Position.Z = -25;
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

        Raylib.DrawPlane(new Vector3(0, 0, 0), new Vector2(50, 50), Color.Beige);

        Raylib.EndMode3D();
        Raylib.EndTextureMode();
    }

    private void HandleControls() {
        Raylib.UpdateCamera(ref _camera, CameraMode.FirstPerson);
    }

    internal void Unload() {
        Raylib.UnloadRenderTexture(_viewTexture);
    }
}