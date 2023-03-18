
using System.Collections;
using Wish;

namespace LiveEasy;

public partial class Plugin
{
    private void Update()
    {
        if (SaveShortcut.Value.IsUp() && Player.Instance != null)
        {
            StartCoroutine(SaveGame());
        }
    }
    
    private static IEnumerator SaveGame()
    {
        GameSave.Instance.SaveGame();
        NotificationStack.Instance.SendNotification("Game Saved!");
        yield break;
    }
}