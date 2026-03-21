using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;

namespace StableChocobo.Windows;

public class ChocoboPromptWindow : Window 
{

    public ChocoboPromptWindow() : base("Stable Chocobo")
    {
    
    }
    public override void Draw()
    {
        ImGui.Text("Your chocobo is currently summoned and you want to stable it. Unsummon it?");
        if (ImGui.Button("Yes!"))
            {
                IsOpen = false;
            }
        if (ImGui.Button("No!"))
            {
                IsOpen = false;
            }
    }
}

