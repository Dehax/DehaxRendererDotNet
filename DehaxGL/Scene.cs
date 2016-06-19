using DehaxGL.Math;
using DehaxGL.Model;
using System.Collections.Generic;
using System.Drawing;

namespace DehaxGL
{
    public class Scene
    {
        private const double AXIS_SCALE = 100.0;

        private List<Model.Model> _objects = new List<Model.Model>();

        public int NumObjects { get { return _objects.Count; } }

        public Model.Model this[int index]
        {
            get
            {
                return _objects[index];
            }
            set
            {
                _objects[index] = value;
            }
        }

        public void CreateAxisModels()
        {
            Model.Model axisX = ModelsFactory.Box(0.01 * AXIS_SCALE, 1.0 * AXIS_SCALE, 0.01 * AXIS_SCALE);
            axisX.Position = new Vec3f(0.5 * AXIS_SCALE, 0.0, 0.0);
            axisX.Color = Color.Red;

            Model.Model axisY = ModelsFactory.Box(0.01 * AXIS_SCALE, 0.01 * AXIS_SCALE, 1.0 * AXIS_SCALE);
            axisY.Position = new Vec3f(0.0, 0.5 * AXIS_SCALE, 0.0);
            axisY.Color = Color.Green;

            Model.Model axisZ = ModelsFactory.Box(1.0 * AXIS_SCALE, 0.01 * AXIS_SCALE, 0.01 * AXIS_SCALE);
            axisZ.Position = new Vec3f(0.0, 0.0, 0.5 * AXIS_SCALE);
            axisZ.Color = Color.Blue;

            AddModel(axisX);
            AddModel(axisY);
            AddModel(axisZ);
        }

        public Scene()
        {
            
        }

        public void MoveObject(int index, Vec3f offset)
        {
            Vec3f oldPosition = _objects[index].Position;
            Vec3f newPosition = oldPosition + offset;
            _objects[index].Position = newPosition;
        }

        public void RotateObject(int index, Vec3f rotation)
        {
            Vec3f oldRotation = _objects[index].Rotation;
            Vec3f newRotation = oldRotation + rotation;
            _objects[index].Rotation = newRotation;
        }

        public void ScaleObject(int index, Vec3f scale)
        {
            Vec3f oldScale = _objects[index].Scale;
            Vec3f newScale = oldScale + scale;
            _objects[index].Scale = newScale;
        }

        public void AddModel(Model.Model model)
        {
            _objects.Add(model);
        }

        public void RemoveModel(int index)
        {
            _objects.RemoveAt(index);
        }
    }
}
