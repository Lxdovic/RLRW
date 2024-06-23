global using RLRPActiveActor = RocketLeagueReplayParser.NetworkStream.ActiveActor;
global using RLRPReplicatedExplosionDataExtended =
    RocketLeagueReplayParser.NetworkStream.ReplicatedExplosionDataExtended;
global using RLRPQuaternion = RocketLeagueReplayParser.NetworkStream.Quaternion;
global using RLRPReplicatedDemolishGoalExplosion =
    RocketLeagueReplayParser.NetworkStream.ReplicatedDemolishGoalExplosion;
global using RLRPTeamPaint = RocketLeagueReplayParser.NetworkStream.TeamPaint;
global using RLRPReplicatedBoost = RocketLeagueReplayParser.NetworkStream.ReplicatedBoost;
global using RLRPCameraSettings = RocketLeagueReplayParser.NetworkStream.CameraSettings;
global using RLRPClientLoadouts = RocketLeagueReplayParser.NetworkStream.ClientLoadouts;
global using RLRPClientLoadout = RocketLeagueReplayParser.NetworkStream.ClientLoadout;
global using RLRPUniqueId = RocketLeagueReplayParser.NetworkStream.UniqueId;
global using RLRPPartyLeader = RocketLeagueReplayParser.NetworkStream.PartyLeader;
global using RLRPClientLoadoutsOnline = RocketLeagueReplayParser.NetworkStream.ClientLoadoutsOnline;
global using RLRPClientLoadoutOnline = RocketLeagueReplayParser.NetworkStream.ClientLoadoutOnline;
global using RLRPProductAttribute = RocketLeagueReplayParser.NetworkStream.ProductAttribute;
global using RLRPObjectTarget = RocketLeagueReplayParser.NetworkStream.ObjectTarget;
global using RLRPReservation = RocketLeagueReplayParser.NetworkStream.Reservation;

global using ActiveActor = RLReplayWatcher.data.ActiveActor;
global using ReplicatedExplosionDataExtended =
    RLReplayWatcher.data.ReplicatedExplosionDataExtended;
global using ReplicatedDemolishGoalExplosion =
    RLReplayWatcher.data.ReplicatedDemolishGoalExplosion;
global using TeamPaint = RLReplayWatcher.data.TeamPaint;
global using ReplicatedBoost = RLReplayWatcher.data.ReplicatedBoost;
global using CameraSettings = RLReplayWatcher.data.CameraSettings;
global using ClientLoadouts = RLReplayWatcher.data.ClientLoadouts;
global using ClientLoadout = RLReplayWatcher.data.ClientLoadout;
global using UniqueId = RLReplayWatcher.data.UniqueId;
global using ClientLoadoutsOnline = RLReplayWatcher.data.ClientLoadoutsOnline;
global using ClientLoadoutOnline = RLReplayWatcher.data.ClientLoadoutOnline;
global using ProductAttribute = RLReplayWatcher.data.ProductAttribute;
global using ObjectTarget = RLReplayWatcher.data.ObjectTarget;
global using Reservation = RLReplayWatcher.data.Reservation;

global using Quaternion = System.Numerics.Quaternion;

using Raylib_cs;
using rlImGui_cs;
using RLReplayWatcher.replayActors;
using RLReplayWatcher.ui;
using RLReplayWatcher.ui.scene;
using RocketLeagueReplayParser;

namespace RLReplayWatcher;

internal class Program {
    private const int ScreenHeight = 780;
    private const int ScreenWidth = 1280;
    internal static Replay? Replay;
    internal static Scene? Scene;
    internal static GameManager? Game;

    private static void Main(string[] args) {
        Raylib.SetWindowState(ConfigFlags.ResizableWindow | ConfigFlags.Msaa4xHint);
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