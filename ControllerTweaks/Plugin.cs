using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wish;
using Player = Wish.Player;

namespace ControllerTweaks;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.controllertweaks";
    private const string PluginName = "Controller Tweaks";
    private const string PluginVersion = "0.1.0";

    private static ConfigEntry<bool> LockMouseToCenter { get; set; }
    internal static ManualLogSource LOG { get; private set; }

    private static bool InventoryOpen
    {
        get
        {
            var inventory = GameObject.Find("Player(Clone)/UI/Inventory");
            return inventory != null && inventory.activeSelf;
        }
    }

    private void Awake()
    {
        LOG = new ManualLogSource(PluginName);
        BepInEx.Logging.Logger.Sources.Add(LOG);
        LockMouseToCenter = Config.Bind("01. Controller", "Lock Mouse To Center", false, new ConfigDescription("Lock the mouse to the center of the screen when no UI is open. This is for controller players. Experimental feature.", null, new ConfigurationManagerAttributes {Order = 6}));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void Update()
    {
        if (LockMouseToCenter.Value && UIHandler.Instance != null && Player.Instance != null)
        {
            if (DialogueController.Instance.DialogueOnGoing || DialogueController.Instance._dialoguePanel.activeSelf || QuestRewards.Instance.AcceptingRewards || UIHandler.InventoryOpen || UIHandler.Instance.uiOpen || UIHandler.Instance._inventoryUI.gameObject.activeSelf || UIHandler.Instance.ExternalUI.activeSelf)
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
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Player.Instance != null && UIHandler.Instance != null && InventoryOpen)
        {
            // Retrieve the index of the current active panel
            var currentPanel = UIHandler.Instance.playerInventory.majorTabIndex;

            // Check for button presses and update currentPanel index accordingly
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.JoystickButton4))
            {
                // Move to the previous panel (with wrap-around logic)
                currentPanel = (currentPanel - 1 + UIHandler.Instance.playerInventory._panels.Count) % UIHandler.Instance.playerInventory._panels.Count;
                UIHandler.Instance.playerInventory.OpenMajorPanel(currentPanel);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.JoystickButton5))
            {
                // Move to the next panel (with wrap-around logic)
                currentPanel = (currentPanel + 1) % UIHandler.Instance.playerInventory._panels.Count;
                UIHandler.Instance.playerInventory.OpenMajorPanel(currentPanel);
            }
        }
    }

    private void OnDestroy()
    {
        LOG.LogError("I've been destroyed!");
    }

    private void OnDisable()
    {
        LOG.LogError("I've been disabled!");
    }
}