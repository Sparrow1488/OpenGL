using OpenTK.Graphics.OpenGL;

namespace OpenGl.IntroTwo.Tools
{
    internal class GraphicEngine
    {
        /// <returns>Vertex Array Object of Element</returns>
        public int Create(float[] vertices)
        {
            var vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            var vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            return vao;
        }
    }
}
