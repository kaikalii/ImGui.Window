using System;

namespace ImGui.Window.Resources
{
    /// <summary>
    /// Allows access to built-in images.
    /// </summary>
    /// <remarks>To load and use your own images, see <see cref="ImageResource.FromFile"/>, <see cref="ImageResource.FromResource"/>, <see cref="ImageResource.FromStream"/>, and <see cref="ImageResource.FromBitmap"/>.</remarks>
    public class ImageResources
    {
        private readonly Application _application;
        private const string ErrorPath_ = "error.png";
        private ImageResource? _error;
        public ImageResource Error
        {
            get
            {
                _error ??= ImageResource.FromResource(_application, typeof(ImageResources).Assembly, ErrorPath_);
                return _error;
            }
        }
        public ImageResources(Application application) => _application = application;
    }
}
