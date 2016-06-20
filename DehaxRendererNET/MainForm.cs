using DehaxGL.Math;
using DehaxGL.Model;
using DehaxGL.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DehaxRendererNET
{
    public partial class MainForm : Form
    {
        private DehaxGL.DehaxGL _dehaxGL;

        public MainForm()
        {
            InitializeComponent();

            _dehaxGL = new DehaxGL.DehaxGL(viewport);
            _dehaxGL.Scene.CreateAxisModels();

            double size = 100.0;
            _dehaxGL.Scene.AddModel(ModelsFactory.Box(size, size, size));
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            _dehaxGL.SetViewportSize(viewport.Width, viewport.Height);
            _dehaxGL.Render(DehaxGL.DehaxGL.RenderMode.Both);
            viewport.Invalidate();
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            //_dehaxGL.Render(DehaxGL.DehaxGL.RenderMode.Both);
            //viewport.Update();
        }

        private void viewport_KeyPress(object sender, KeyPressEventArgs e)
        {
            _dehaxGL.Scene.RotateObject(3, new Vec3f(0.0, Utils.DegreeToRadian(10.0), 0.0));
            _dehaxGL.Render(DehaxGL.DehaxGL.RenderMode.Both);
            viewport.Invalidate();
        }
    }
}
