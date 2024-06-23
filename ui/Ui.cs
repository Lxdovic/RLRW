using System.Diagnostics;
using System.Numerics;
using ImGuiNET;
using NativeFileDialogSharp;
using Raylib_cs;
using rlImGui_cs;
using RLReplayWatcher.replayHelper;
using RLReplayWatcher.ui.scene;
using RocketLeagueReplayParser;

namespace RLReplayWatcher.ui;

internal static class Ui {
    private static string? _path;
    private static bool _isPlaying = false;
    private static Stopwatch _stopwatch = new();
    private static double _offsetTime;

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
        
        if (Program.Game != null) {
            var frameIndex = Program.Game?.FrameIndex ?? 0;
        
            ImGui.SliderInt("Frame Index", ref frameIndex, 0, Program.Game?.Frames.Count - 1 ?? 0);
            
            if (frameIndex != Program.Game.FrameIndex) {
                Program.Game.FrameIndex = frameIndex;

                _offsetTime = Program.Game.Frames[frameIndex].Time * 1000;
                _stopwatch.Reset();
            }
        
            if (_stopwatch.IsRunning) {
                if (ImGui.Button("Pause")) {
                    _stopwatch.Stop();
                }
            }
        
            if (!_stopwatch.IsRunning) {
                if (ImGui.Button("Play")) {
                    _stopwatch.Start();
                }
            }
        }
        
        ImGui.EndChild();
        ImGui.SameLine();
        ImGui.BeginChild("Scene", new Vector2(ImGui.GetColumnWidth(), ImGui.GetWindowHeight()),
            ImGuiChildFlags.AlwaysUseWindowPadding);

        Program.Scene?.Update(_stopwatch, _offsetTime);
        Program.Scene?.Render();

        ImGui.EndChild();

        ImGui.End();

        ImGui.PopFont();

        Raylib.EndDrawing();
        rlImGui.End();
    }
}