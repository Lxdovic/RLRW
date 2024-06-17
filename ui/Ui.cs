using System.Numerics;
using ImGuiNET;
using NativeFileDialogSharp;
using Raylib_cs;
using rlImGui_cs;
using RLReplayWatcher.replayActors;
using RLReplayWatcher.replayHelper;
using RLReplayWatcher.ui.scene;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.ui;

internal static class Ui {
    private static string? _path;
    private static int _frameIndex;
    private static int _objId;
    private static int _nameId;
    private static string _notes = "";
    
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
            Program.Game = new Game(Program.Replay);
        }

        ImGui.InputTextMultiline("Notes", ref _notes, 1000, new Vector2(ImGui.GetColumnWidth(), 100));

        ImGui.EndChild();
        ImGui.SameLine();
        ImGui.BeginChild("PropsExplorer", new Vector2(ImGui.GetColumnWidth(), ImGui.GetWindowHeight()),
            ImGuiChildFlags.AlwaysUseWindowPadding);

        Program.Scene?.Update();
        Program.Scene?.Render();
        
        ImGui.InputInt("Object Id", ref _objId);
        ImGui.InputInt("Name Id", ref _nameId);
        
        _nameId =Math.Clamp(_nameId, 0, Program.Replay?.Names.Length - 1 ?? 0);
        _objId = Math.Clamp(_objId, 0, Program.Replay?.Objects.Length - 1 ?? 0);
        
        ImGui.Text($"Object: {Program.Replay?.Objects[_objId]}");
        ImGui.Text($"Name: {Program.Replay?.Names[_nameId]}");
        
        ImGui.InputInt("Frame Index", ref _frameIndex);
        
        for (var i = 0; i < Program.Replay?.Frames[_frameIndex].ActorStates.Count; i++) {
            var actor = Program.Replay.Frames[_frameIndex].ActorStates[i];
            
            if (actor.State != ActorStateState.Deleted) continue;

            if (ImGui.CollapsingHeader($"{i}")) {
                ImGui.SeparatorText("Ids");

                ImGui.Text($"Id {actor.Id}");
                ImGui.Text($"NameId {actor.NameId}");
                ImGui.Text($"TypeId {actor.TypeId}");
                ImGui.Text($"ClassId {actor.ClassId}");

                ImGui.SeparatorText("Properties");
                
                ImGui.Text($"Name?: {ReplayHelper.GetName(Program.Replay, actor)}");
                ImGui.Text($"Class?: {ReplayHelper.GetClass(Program.Replay, actor)?.Class}");

                foreach (var (_, prop) in actor.Properties) {
                    ImGui.Text($"{prop.PropertyName}: {prop.Data}");
                }

                ImGui.SeparatorText("Other Props");

                ImGui.Text($"Position: {actor.Position}");
                ImGui.Text($"Rotation: {actor.Rotation}");
                ImGui.Text($"Unknown1: {actor.Unknown1}");
                ImGui.Text($"State: {actor.State}");
            }
        }

        ImGui.EndChild();
        
        ImGui.End();

        ImGui.PopFont();

        Raylib.EndDrawing();
        rlImGui.End();
    }
}