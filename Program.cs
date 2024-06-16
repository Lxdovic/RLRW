using Raylib_cs;
using rlImGui_cs;
using RLReplayWatcher.ui;
using RLReplayWatcher.ui.scene;
using RocketLeagueReplayParser;

namespace RLReplayWatcher;

internal class Program {
    private const int ScreenHeight = 780;
    private const int ScreenWidth = 1280;
    internal static Replay? Replay;
    internal static Scene? Scene;

    private static void Main(string[] args) {
        Raylib.SetWindowState(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "RLRW");

        Theme.SetupFonts();
        rlImGui.Setup(true, true);
        Theme.ApplyTheme();
        Raylib.SetExitKey(KeyboardKey.Null);

        while (!Raylib.WindowShouldClose()) Ui.Render();

        Scene?.Unload();

        rlImGui.Shutdown();
        Raylib.CloseWindow();
    }
}