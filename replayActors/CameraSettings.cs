using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class CameraSettingsActor : GameEntity {
    public CameraSettings? ProfileSettings { get; set; }
    public ActiveActor? GameraActor { get; set; }
    public byte GameraYaw { get; set; }
    public byte GameraPitch { get; set; }
    public bool UsingSecondaryCamera { get; set; }
    public bool UsingBehindView { get; set; }

    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            case "TAGame.CameraSettingsActor_TA:ProfileSettings":
                ProfileSettings = (CameraSettings)property.Data;
                break;
            case "TAGame.CameraSettingsActor_TA:PRI":
                GameraActor = (ActiveActor)property.Data;
                break;
            case "TAGame.CameraSettingsActor_TA:CameraYaw":
                GameraYaw = (byte)property.Data;
                break;
            case "TAGame.CameraSettingsActor_TA:CameraPitch":
                GameraPitch = (byte)property.Data;
                break;
            case "TAGame.CameraSettingsActor_TA:bUsingSecondaryCamera":
                UsingSecondaryCamera = (bool)property.Data;
                break;
            case "TAGame.CameraSettingsActor_TA:bUsingBehindView":
                UsingBehindView = (bool)property.Data;
                break;

            default:
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} for object camerasettings (TAGame.CameraSettings); data: {property.Data}, type: {property.Data.GetType()}");
                break;
        }
    }
}