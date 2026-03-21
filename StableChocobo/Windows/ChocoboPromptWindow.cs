using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using FFXIVClientStructs.FFXIV.Component.GUI;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using Dalamud.Plugin.Services;
using System.Threading.Tasks;

namespace StableChocobo.Windows;

public class ChocoboPromptWindow : Window 
{

    public unsafe override void Draw()
    {
        ImGui.Text("Your chocobo is currently summoned and you want to stable it. Unsummon it?");
        if (ImGui.Button("Yes!"))
            {
                OnYesClick();
            }
        if (ImGui.Button("No!"))
            {
                IsOpen = false;
            }
    }

    private async void OnYesClick()
    {
        IsOpen = false; // Closing the window
        unsafe
        {
        var addon = Plugin.GameGui.GetAddonByName("SelectString");
        if (!addon.IsNull) ((AtkUnitBase*)addon.Address)->Close(true);
        }
        await Task.Delay(500); // Half a second delay
        _framework.RunOnTick(() =>
        {
            unsafe 
            {
                ActionManager* am = ActionManager.Instance();
                if (am == null) return;
                am->UseAction(ActionType.BuddyAction, 2);
            }
        });
    }

    private IFramework _framework;

    public ChocoboPromptWindow(IFramework framework) : base("Stable Chocobo")
    {
        _framework = framework;
    }
}

