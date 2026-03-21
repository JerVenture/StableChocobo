using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using StableChocobo.Windows;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Game.ClientState.Buddy;

namespace StableChocobo;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IAddonLifecycle AddonLifecycle { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static IBuddyList BuddyList { get; private set; } = null!;
    public readonly WindowSystem WindowSystem = new("StableChocobo");
    public ChocoboPromptWindow promptWindow;

    public Plugin()
    {
        // Tell the UI system that we want our windows to be drawn through the window system
        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;

        promptWindow = new ChocoboPromptWindow();
        WindowSystem.AddWindow(promptWindow);

        Log.Information("StableChocobo loaded.");

        AddonLifecycle.RegisterListener(AddonEvent.PostSetup, "SelectString", OnStableOpen);
    }

    public void Dispose()
    {
        PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        WindowSystem.RemoveAllWindows();

        AddonLifecycle.UnregisterListener(AddonEvent.PostSetup, "SelectString", OnStableOpen);
    }
    private void OnStableOpen(AddonEvent type, AddonArgs args)
    {
        Log.Information($"Addon opened: {args.AddonName}");
        Log.Information($"CompanionBuddy: {BuddyList.CompanionBuddy}");
        promptWindow.IsOpen = true;
    }
}
