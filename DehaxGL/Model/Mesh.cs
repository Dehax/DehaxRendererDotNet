using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DehaxGL.Model
{
    public class Mesh
    {
        private List<Vertex> _vertices = new List<Vertex>();
        private List<Face> _faces = new List<Face>();
        private double _maxLocalScale;

        public int NumVertices { get { return _vertices.Count; } }
        public int NumFaces { get { return _faces.Count; } }
        public double MaxLocalScale { get { return _maxLocalScale; } }

        public Vertex GetVertex(int index)
        {
            return _vertices[index];
        }

        public Face GetFace(int index)
        {
            return _faces[index];
        }

        public Mesh()
        {
            _maxLocalScale = 1.0;
        }

        public Mesh(Mesh mesh)
        {
            _vertices = new List<Vertex>(mesh._vertices);
            _faces = new List<Face>(mesh._faces);
            _maxLocalScale = mesh._maxLocalScale;
        }

        public void AddVertex(Vertex vertex)
        {
            double x = System.Math.Abs(vertex.X);
            double y = System.Math.Abs(vertex.Y);
            double z = System.Math.Abs(vertex.Z);

            if (x > _maxLocalScale)
            {
                _maxLocalScale = x;
            }

            if (y > _maxLocalScale)
            {
                _maxLocalScale = y;
            }

            if (z > _maxLocalScale)
            {
                _maxLocalScale = z;
            }

            _vertices.Add(vertex);
        }

        public void AddFace(Face face)
        {
            _faces.Add(face);
        }

        public void ClearVertices()
        {
            _vertices.Clear();
        }

        public void ClearFaces()
        {
            _faces.Clear();
        }
    }
}
