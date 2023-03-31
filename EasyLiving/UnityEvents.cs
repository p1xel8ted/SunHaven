using Wish;

namespace EasyLiving;

public partial class Plugin
{
    private void Update()
    {
        if (EnableSaveShortcut.Value && SaveShortcut.Value.IsUp() && Player.Instance != null)
        {
            GameSave.Instance.SaveGame();
            NotificationStack.Instance.SendNotification("Game Saved!");
        }
    }
}