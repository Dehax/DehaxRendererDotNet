using System.Drawing;

namespace DehaxGL
{
    public interface IViewport
    {
        int Width { get; }
        int Height { get; }

        void SetPixel(int x, int y, Color color);

        void SetSize(int width, int height);
        void Clear();

        void Lock();
        void Unlock();
    }
}
