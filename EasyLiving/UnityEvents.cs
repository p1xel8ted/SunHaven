using UnityEngine;
using Wish;

namespace EasyLiving;

public partial class Plugin
{
    private static void Notify()
    {
        if (SingletonBehaviour<NotificationStack>.Instance is not null)
        {
            SingletonBehaviour<NotificationStack>.Instance.SendNotification($"Movement Speed: {Player.Instance.FinalMovementSpeed}");
        }
    }

    private void Update()
    {
        if (EnableSaveShortcut.Value && SaveShortcut.Value.IsUp() && Player.Instance is not null && GameSave.Instance is not null)
        {
            SingletonBehaviour<GameSave>.Instance.SaveGame(true);
            SingletonBehaviour<NotificationStack>.Instance.SendNotification("Game Saved!");
        }
    }
}