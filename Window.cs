using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using ImGui.Window.Extensions;
using ImGui.Window.Resources;
using ImGuiNET;
using Veldrid.Sdl2;

namespace ImGui.Window
{
    // HINT: Does not derive from Container to not be a component and therefore nestable into other containers
    public abstract class GuiWindow
    {

        private Image? _icon;
        private bool _setIcon;

        #region Properties

        protected Application? Application { get; set; }
        public string Title { get; set; } = string.Empty;
        public Vector2 Position { get; set; } = new Vector2(100, 100);
        public Vector2 Size { get; set; } = new Vector2(700, 400);
        public int Width => (int)Size.X;
        public int Height => (int)Size.Y;

        public bool AllowDragDrop { get; set; }

        /// <summary>
        /// Sets the applications icon.
        /// </summary>
        /// <remarks>The icons dimensions need to be a power of 2 (eg. 32, 64, 128, etc)</remarks>
        public Image? Icon
        {
            get => _icon;
            protected set
            {
                _icon = value;
                _setIcon = true;
            }
        }

        public static Vector2 Padding => Style.GetStyleVector2(ImGuiStyleVar.WindowPadding);

        public FontResource? DefaultFont { get; set; }

        #endregion

        #region Events

        public event EventHandler<DragDropEvent>? DragDrop;
        public event EventHandler? Load;
        public event EventHandler? Resized;
        public event Func<object, ClosingEventArgs, Task>? Closing;

        #endregion

        public void Update()
        {
            if (Application is null) return;

            // Set icon
            if (_setIcon && Icon is not null)
            {
                Sdl2NativeExtensions.SetWindowIcon(Application.Window.SdlWindowHandle, (Bitmap)Icon);
                _setIcon = false;
            }

            // Set window title
            Application.Window.Title = Title;

            // Begin window
            ImGuiNET.ImGui.Begin(Title, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoMove);

            ImGuiNET.ImGui.SetWindowSize(Size, ImGuiCond.Always);
            ImGuiNET.ImGui.SetWindowPos(Position, ImGuiCond.Always);

            ImGuiNET.ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            ImGuiNET.ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 0);
            ImGuiNET.ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);

            // Apply style
            Style.ApplyStyle();

            // Push font to default to
            if (DefaultFont != null)
                ImGuiNET.ImGui.PushFont((ImFontPtr)DefaultFont);

            // Add form controls
            ImGuiNET.ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Padding);
            ImGuiNET.ImGui.SetWindowPos(new Vector2(0, 0));

            var contentPos = ImGuiNET.ImGui.GetCursorScreenPos();
            var contentWidth = Width - (int)Padding.X * 2;
            var contentHeight = Height - (int)Padding.Y * 2;
            var contentRect = new Veldrid.Rectangle((int)contentPos.X, (int)contentPos.Y, contentWidth, contentHeight);

            Ui();

            // Handle Drag and Drop after rendering
            if (AllowDragDrop)
                if (Application.TryGetDragDrop(contentRect, out var dragDrop))
                    OnDragDrop(dragDrop.Event);

            // End window
            ImGuiNET.ImGui.End();
        }

        protected virtual void Close() => Application?.Window.Close();

        #region Event Invokers

        internal void OnResized() => Resized?.Invoke(this, new EventArgs());

        internal void OnLoad() => Load?.Invoke(this, new EventArgs());

        internal async Task OnClosing(ClosingEventArgs e)
        {
            if (Closing is null) return;
            await Closing.Invoke(this, e);
        }

        private void OnDragDrop(DragDropEvent e) => DragDrop?.Invoke(this, e);

        public void Show() => Application?.Execute();

        public abstract void Ui();

        #endregion
    }

    public class ClosingEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}
