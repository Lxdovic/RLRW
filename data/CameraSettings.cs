namespace RLReplayWatcher.data;

internal sealed class CameraSettings : RLRPCameraSettings {
    public new float FieldOfView { get; set; }
    public new float Height { get; set; }
    public new float Pitch { get; set; }
    public new float Distance { get; set; }
    public new float Stiffness { get; set; }
    public new float SwivelSpeed { get; set; }
    public new float TransitionSpeed { get; set; }

    public CameraSettings Clone() {
        return new CameraSettings {
            FieldOfView = FieldOfView,
            Height = Height,
            Pitch = Pitch,
            Distance = Distance,
            Stiffness = Stiffness,
            SwivelSpeed = SwivelSpeed,
            TransitionSpeed = TransitionSpeed
        };
    }
}