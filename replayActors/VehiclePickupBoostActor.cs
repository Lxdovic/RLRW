using RLReplayWatcher.data;
using RocketLeagueReplayParser.NetworkStream;

namespace RLReplayWatcher.replayActors;

internal sealed class VehiclePickupBoostActor(ActorState? actor = null) : Actor {
    public PickupData? PickupData { get; set; }

    public override void HandleGameEvents(ActorStateProperty property) {
        switch (property.PropertyName) {
            case "TAGame.VehiclePickup_TA:NewReplicatedPickupData":
                var pickupData = (NewReplicatedPickupData)property.Data;

                PickupData = new PickupData {
                    Unknown1 = pickupData.Unknown1,
                    Unknown2 = pickupData.Unknown2,
                    ActorId = pickupData.ActorId
                };
                break;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"Unhandled property: {property.PropertyName} VehiclePickupBoostActor; data: {property.Data}");
                Console.ResetColor();
                break;
        }
    }

    public override VehiclePickupBoostActor Clone() {
        return new VehiclePickupBoostActor();
    }
}