using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using FFXIVClientStructs.FFXIV.Component.GUI;
using FFXIVClientStructs.FFXIV.Client.Game;
using System;
using StableChocobo;

namespace StableChocobo.Windows;

public class ChocoboPromptWindow : Window 
{

    public ChocoboPromptWindow() : base("Stable Chocobo")
    {
    
    }
    public unsafe override void Draw()
    {
        ImGui.Text("Your chocobo is currently summoned and you want to stable it. Unsummon it?");
        if (ImGui.Button("Yes!"))
            {
                var addon = Plugin.GameGui.GetAddonByName("SelectString");
                if (!addon.IsNull) ((AtkUnitBase*)addon.Address)->Close(true);
                IsOpen = false;
            }
        if (ImGui.Button("No!"))
            {
                IsOpen = false;
            }
    }
}

