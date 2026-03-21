using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using StableChocobo.Windows;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
namespace StableChocobo;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static IAddonLifecycle AddonLifecycle { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static IGameGui GameGui { get; private set; } = null!;

    public readonly WindowSystem WindowSystem = new("StableChocobo");
    private ChocoboPromptWindow promptWindow;

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
    private unsafe void OnStableOpen(AddonEvent type, AddonArgs args)
    {
        UIState* ui = UIState.Instance();
        if (ui == null) return;

        var buddy = ui->Buddy.CompanionInfo;

        var addon = (AtkUnitBase*)args.Addon.Address;
        var textNode = (AtkTextNode*)addon->GetNodeById(2);
        if (textNode == null) return;
        var text = textNode->NodeText.ToString();
        if (!text.Contains("Chocobos Stabled")) return;

        if (buddy.TimeLeft > 0)
        {
            promptWindow.IsOpen = true;
        }
    }
}
