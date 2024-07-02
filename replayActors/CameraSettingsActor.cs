using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class CameraSettingsActor(ActorState? actor = null) : Actor {
    public CameraSettings? ProfileSettings { get; set; }
    public ActiveActor? GameraActor { get; set; }
    public byte GameraYaw { get; set; }
    public byte GameraPitch { get; set; }
    public bool UsingSecondaryCamera { get; set; }
    public bool UsingBehindView { get; set; }

    public override CameraSettingsActor Clone() {
        return new CameraSettingsActor {
            ProfileSettings = ProfileSettings?.Clone(),
            GameraActor = GameraActor?.Clone(),
            GameraYaw = GameraYaw,
            GameraPitch = GameraPitch,
            UsingSecondaryCamera = UsingSecondaryCamera,
            UsingBehindView = UsingBehindView
        };
    }

    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            case "TAGame.CameraSettingsActor_TA:ProfileSettings":
                var profileSettings = (RLRPCameraSettings)property.Data;

                ProfileSettings = new CameraSettings {
                    FieldOfView = profileSettings.FieldOfView,
                    Height = profileSettings.Height,
                    Pitch = profileSettings.Pitch,
                    Distance = profileSettings.Distance,
                    Stiffness = profileSettings.Stiffness,
                    SwivelSpeed = profileSettings.SwivelSpeed,
                    TransitionSpeed = profileSettings.TransitionSpeed
                };
                break;
            case "TAGame.CameraSettingsActor_TA:PRI":
                var actor = (RLRPActiveActor)property.Data;

                GameraActor = new ActiveActor {
                    Active = actor.Active,
                    ActorId = actor.ActorId
                };
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