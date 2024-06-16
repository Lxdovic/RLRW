using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

namespace RLReplayWatcher.ui.scene;

internal sealed class Scene {
    internal Camera3D Camera;
    internal bool IsGrabbed;
    internal RenderTexture2D ViewTexture;

    internal Scene() {
        ViewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        Camera.FovY = 45;
        Camera.Up.Y = 1;
        Camera.Position.Y = 3;
        Camera.Position.Z = -25;
    }

    internal void Render() {
        rlImGui.ImageRenderTextureFit(ViewTexture);
    }

    internal void Update() {
        if (IsGrabbed) HandleControls();

        Raylib.BeginTextureMode(ViewTexture);
        Raylib.ClearBackground(Color.Green);
        Raylib.BeginMode3D(Camera);
        Raylib.DrawPlane(new Vector3(0, 0, 0), new Vector2(50, 50), Color.Beige);

        Raylib.EndMode3D();
        Raylib.EndTextureMode();
    }

    private void HandleControls() {
        Raylib.UpdateCamera(ref Camera, CameraMode.FirstPerson);

        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) IsGrabbed = true;
        if (Raylib.IsKeyPressed(KeyboardKey.Escape)) IsGrabbed = false;
        
        if (IsGrabbed) Raylib.DisableCursor();
        if (!IsGrabbed) Raylib.EnableCursor();
    }

    internal void Unload() {
        Raylib.UnloadRenderTexture(ViewTexture);
    }
}