﻿using Wish;

namespace EasyLiving;

public partial class Plugin
{
    private void Update()
    {
        if (SaveShortcut.Value.IsUp() && Player.Instance != null)
        {
            GameSave.Instance.SaveGame();
            NotificationStack.Instance.SendNotification("Game Saved!");
        }
    }
}