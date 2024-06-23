using System.Numerics;
using ImGuiNET;
using NativeFileDialogSharp;
using Raylib_cs;
using rlImGui_cs;
using RLReplayWatcher.replayActors;
using RLReplayWatcher.replayHelper;
using RLReplayWatcher.ui.scene;

namespace RLReplayWatcher.ui;

internal static class Ui {
    private static string? _path;

    internal static void Render() {
        rlImGui.Begin();

        // ImGui.ShowDemoWindow();

        Raylib.BeginDrawing();

        ImGui.PushFont(Theme.Fonts["Poppins-Regular"]);

        ImGui.SetNextWindowPos(Vector2.Zero, ImGuiCond.Always);
        ImGui.SetNextWindowSize(new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()), ImGuiCond.Always);

        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);

        ImGui.Begin("RLRW", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove |
                            ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoScrollbar |
                            ImGuiWindowFlags.NoScrollWithMouse |
                            ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus |
                            ImGuiWindowFlags.NoNav);
        ImGui.PopStyleVar(2);

        ImGui.BeginChild("Left Menu", new Vector2(400, ImGui.GetWindowHeight()),
            ImGuiChildFlags.ResizeX | ImGuiChildFlags.AlwaysUseWindowPadding);

        if (ImGui.Button("Parse")) {
            if (_path == null) {
                var result = Dialog.FileOpen("replay");
                _path = result.Path;
            }

            Program.Replay = ReplayHelper.Parse(_path);
            Program.Scene = new Scene();
            Program.Game = new GameManager(Program.Replay);
            Program.Game.Parse();
        }

        ImGui.EndChild();
        ImGui.SameLine();
        ImGui.BeginChild("Scene", new Vector2(ImGui.GetColumnWidth(), ImGui.GetWindowHeight()),
            ImGuiChildFlags.AlwaysUseWindowPadding);

        Program.Scene?.Update();
        Program.Scene?.Render();

        ImGui.EndChild();

        ImGui.End();

        ImGui.PopFont();

        Raylib.EndDrawing();
        rlImGui.End();
    }
}