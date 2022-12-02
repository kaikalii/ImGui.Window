# ImGui.Window
This is a simple library that creates a window for ImGui.NET.

# Usage

Simply make a class that iherits from `Form` and override `Ui()`.

```C#
using ImGui.Window;
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
```

## Credits
ocurnut - For creating Dear ImGui
mellinoe - For creating ImGui.NET (bindings for Dear ImGui)
Veldrid Team - For creating the Veldrid rendering pipeline
onepiecefreak3 - For creating ImGui.Forms, which this library's backend is based on
