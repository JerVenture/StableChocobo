using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using FFXIVClientStructs.FFXIV.Component.GUI;
using FFXIVClientStructs.FFXIV.Client.Game;
using Dalamud.Plugin.Services;
using System.Threading.Tasks;
using System.Numerics;

namespace StableChocobo.Windows;



public class ChocoboPromptWindow : Window 
{
    private IFramework _framework;

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



    public ChocoboPromptWindow(IFramework framework) : base("Stable Chocobo")
    {
        _framework = framework;
        Size = new Vector2(400, 120);
        SizeCondition = ImGuiCond.Appearing;        
        Position = ImGui.GetMainViewport().GetCenter() - Size.Value / 2;
        PositionCondition = ImGuiCond.Appearing;
    }
}

