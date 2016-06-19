using System.Drawing;

namespace DehaxGL.Model
{
    public class ModelsFactory
    {
        public static Model Box(double width = 1.0, double length = 1.0, double height = 1.0)
        {
            width /= 2.0;
            length /= 2.0;
            height /= 2.0;

            Mesh mesh = new Mesh();
            Model model = new Model("(G) box", mesh, Color.Green);

            mesh.AddVertex(new Vertex(-length, -height, -width));
            mesh.AddVertex(new Vertex(-length, height, -width));
            mesh.AddVertex(new Vertex(length, -height, -width));
            mesh.AddVertex(new Vertex(length, height, -width));

            mesh.AddVertex(new Vertex(-length, -height, width));
            mesh.AddVertex(new Vertex(-length, height, width));
            mesh.AddVertex(new Vertex(length, -height, width));
            mesh.AddVertex(new Vertex(length, height, width));

            mesh.AddFace(new Face(0, 1, 3));
            mesh.AddFace(new Face(3, 1, 2));
            mesh.AddFace(new Face(3, 2, 7));
            mesh.AddFace(new Face(7, 2, 6));
            mesh.AddFace(new Face(7, 6, 4));
            mesh.AddFace(new Face(4, 6, 5));
            mesh.AddFace(new Face(4, 5, 0));
            mesh.AddFace(new Face(0, 5, 1));
            mesh.AddFace(new Face(1, 5, 2));
            mesh.AddFace(new Face(2, 5, 6));
            mesh.AddFace(new Face(3, 4, 0));
            mesh.AddFace(new Face(7, 4, 3));

            return model;
        }
    }
}
