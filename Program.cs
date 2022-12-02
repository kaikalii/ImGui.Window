using ImGui.Window;
using static ImGuiNET.ImGui;

class Program
{
    public static void Main()
    {
        new MyGui().Show();
    }

    class MyGui : GuiWindow
    {
        private bool boolean = false;
        private float number = 0f;
        private string str = "Hello World!";
        public MyGui() => Application = new(this);
        public override void Ui()
        {
            // Put ImGui.NET calls here
            Checkbox("Boolean", ref boolean);
            SliderFloat("Number", ref number, 0f, 10f);
            InputText("String", ref str, 1000);
            LabelText("", str);
        }
    }
}