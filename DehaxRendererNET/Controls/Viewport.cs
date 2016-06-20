using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DehaxGL;
using System.Drawing.Imaging;

namespace DehaxRendererNET.Controls
{
    public partial class Viewport : Control, IViewport
    {
        private const int BytesPerPixel = 4;
        private readonly PixelFormat PixelFormat = PixelFormat.Format32bppArgb;
        private readonly Color BACKGROUND_COLOR = Color.FromArgb(127, 127, 127);

        private Bitmap _bitmap;
        private BitmapData _bitmapData;

        public Viewport()
        {
            InitializeComponent();

            if (Width == 0 || Height == 0)
            {
                return;
            }

            _bitmap = new Bitmap(Width, Height, PixelFormat);

            Lock();
            Clear();
            Unlock();
        }

        public void Clear()
        {
            if (_bitmap == null)
            {
                return;
            }

            for (int y = 0; y < _bitmap.Height; y++)
            {
                for (int x = 0; x < _bitmap.Width; x++)
                {
                    SetPixel(x, y, BACKGROUND_COLOR);
                }
            }
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (_bitmap == null)
            {
                return;
            }

            int h = _bitmap.Height - y;

            if (x >= 0 && x < _bitmap.Width && h >= 0 && h < _bitmap.Height)
            {
                unsafe
                {
                    byte* row = (byte*)_bitmapData.Scan0 + (h * _bitmapData.Stride);

                    row[x * BytesPerPixel] = color.B;        //Blue  0-255
                    row[x * BytesPerPixel + 1] = color.G;    //Green 0-255
                    row[x * BytesPerPixel + 2] = color.R;    //Red   0-255
                    row[x * BytesPerPixel + 3] = color.A;    //Alpha 0-255
                }
            }
        }

        public void SetSize(int width, int height)
        {
            if (_bitmap != null)
            {
                _bitmap.Dispose();
            }

            _bitmap = new Bitmap(width, height, PixelFormat);

            Lock();
            Clear();
            Unlock();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_bitmap == null)
            {
                return;
            }

            e.Graphics.DrawImageUnscaled(_bitmap, 0, 0);
        }

        public void Lock()
        {
            if (_bitmap == null)
            {
                return;
            }

            _bitmapData = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.WriteOnly, PixelFormat);
        }

        public void Unlock()
        {
            if (_bitmap == null)
            {
                return;
            }

            _bitmap.UnlockBits(_bitmapData);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (Width == 0 || Height == 0)
            {
                return;
            }
            
            Invalidate();
        }
    }
}
