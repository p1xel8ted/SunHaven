using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wish;
using Player = Wish.Player;

namespace EasyLiving;

public partial class Plugin
{
    private const string LoadScreen = "LoadScreen";
    private const string GameSaved = "Game Saved!";

    private static void Notify()
    {
        if (SingletonBehaviour<NotificationStack>.Instance is not null)
        {
            SingletonBehaviour<NotificationStack>.Instance.SendNotification($"Movement Speed Multiplier: {MoveSpeedMultiplier.Value}");
        }
    }



    private void Update()
    {
        if (LockMouseToCenter.Value)
        {
            if (UIHandler.Instance != null)
            {
                if (UIHandler.InventoryOpen || UIHandler.Instance.uiOpen || UIHandler.Instance._inventoryUI.gameObject.activeSelf || UIHandler.Instance.ExternalUI.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }

        if (EnableSaveShortcut.Value && SaveShortcut.Value.IsUp() && Player.Instance is not null && GameSave.Instance is not null)
        {
            SingletonBehaviour<GameSave>.Instance.SaveGame(true);
            SingletonBehaviour<NotificationStack>.Instance.SendNotification(GameSaved);
        }
        
        if(MoveSpeedMultiplierIncrease.Value.IsUp())
        {
            MoveSpeedMultiplier.Value += 0.25f;
            Notify();
        }
        else if(MoveSpeedMultiplierDecrease.Value.IsUp())
        {
            MoveSpeedMultiplier.Value -= 0.25f;
            Notify();
        }

        if (Input.GetKey(SkipAutoLoadMostRecentSaveShortcut.Value.MainKey) && SceneManager.GetActiveScene().name.Equals(LoadScreen, StringComparison.InvariantCultureIgnoreCase))
        {
            Patches.SkipAutoLoad = true;
        }
    }
}