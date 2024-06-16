using System.Numerics;
using ImGuiNET;
using NativeFileDialogSharp;
using Raylib_cs;
using rlImGui_cs;
using RLReplayWatcher.ui.scene;
using RocketLeagueReplayParser;

namespace RLReplayWatcher.ui;

internal static class Ui {
    internal static void Render() {
        rlImGui.Begin();

        ImGui.ShowDemoWindow();
        
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
            var result = Dialog.FileOpen("replay");

            Program.Replay = Replay.Deserialize(result.Path);
            Program.Scene = new Scene();
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