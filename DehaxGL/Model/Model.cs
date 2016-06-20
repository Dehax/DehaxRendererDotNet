using DehaxGL.Math;
using System;
using System.Drawing;
using System.Numerics;

namespace DehaxGL.Model
{
    public class Model
    {
        private const int NUM_PARAMETERS = 12;

        private Mesh _mesh;
        private Color _color;

        private double[] _parameters = new double[NUM_PARAMETERS];

        private Vec3f _position;
        private Vec3f _rotation;
        private Vec3f _scale;
        private Vec3f _pivot;

        private Matrix _transformMatrix;
        private Matrix _rotateMatrix;
        private Matrix _scaleMatrix;
        private Matrix _pivotMatrix;
        private Matrix _pivotInverseMatrix;

        private string _name = string.Empty;

        public Mesh Mesh { get { return _mesh; } }

        public Vec3f Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;

                _transformMatrix = Matrix.Transform(value);
            }
        }

        public Vec3f Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;

                _rotateMatrix = Matrix.Rotation(value);
            }
        }

        public Vec3f Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;

                _scaleMatrix = Matrix.Scale(value);
            }
        }

        public Vec3f Pivot
        {
            get
            {
                return _pivot;
            }
            set
            {
                _pivot = value;
            }
        }

        public Matrix WorldMatrix
        {
            get
            {
                Matrix P = _pivotMatrix;
                Matrix R = _rotateMatrix;
                Matrix S = _scaleMatrix;
                Matrix PI = _pivotInverseMatrix;
                Matrix T = _transformMatrix;

                return P * R * S * PI * T;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
            }
        }

        public Model()
        {
            _mesh = null;
        }

        public Model(Model model)
        {
            _mesh = new Mesh(model.Mesh);
            _color = model.Color;

            _position = model._position;
            _rotation = model._rotation;
            _scale = model._scale;
            _pivot = model._pivot;

            _transformMatrix = model._transformMatrix;
            _rotateMatrix = model._rotateMatrix;
            _scaleMatrix = model._scaleMatrix;
            _pivotMatrix = model._pivotMatrix;
            _pivotInverseMatrix = model._pivotInverseMatrix;

            _name = model._name;

            for (int i = 0; i < NUM_PARAMETERS; i++)
            {
                _parameters[i] = model._parameters[i];
            }
        }

        public Model(string name, Mesh mesh, Color color)
        {
            _mesh = mesh;
            _color = color;
            _name = name;

            _position = new Vec3f(0.0, 0.0, 0.0);
            _rotation = new Vec3f(0.0, 0.0, 0.0);
            _scale = new Vec3f(1.0, 1.0, 1.0);
            _pivot = new Vec3f(0.0, 0.0, 0.0);

            _pivotMatrix = new Matrix(true);
            _pivotInverseMatrix = new Matrix(true);
            _transformMatrix = new Matrix(true);
            _rotateMatrix = new Matrix(true);
            _scaleMatrix = new Matrix(true);
        }

        public Model(string name, string filePath)
        {
            _mesh = new Mesh();
            _name = name;

            _position = new Vec3f(0.0, 0.0, 0.0);
            _rotation = new Vec3f(0.0, 0.0, 0.0);
            _scale = new Vec3f(1.0, 1.0, 1.0);
            _pivot = new Vec3f(0.0, 0.0, 0.0);

            _pivotMatrix = new Matrix(true);
            _pivotInverseMatrix = new Matrix(true);
            _transformMatrix = new Matrix(true);
            _rotateMatrix = new Matrix(true);
            _scaleMatrix = new Matrix(true);

            throw new NotImplementedException("Model from object file error");
        }
    }
}
