using System;
using ImGui.Window;
using ImGuiNET;
using static ImGuiNET.ImGui;

class Program
{
    public static void Main()
    {
        new MyGui().Show();
    }

    class MyGui : Form
    {
        private bool boolean = false;
        public MyGui()
        {
            Application = new(this);
        }

        public override void Ui()
        {
            Checkbox("Boolean", ref boolean);
        }
    }
}