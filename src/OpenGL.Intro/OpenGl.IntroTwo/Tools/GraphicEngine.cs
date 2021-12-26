using OpenTK.Graphics.OpenGL;

namespace OpenGl.IntroTwo.Tools
{
    internal class GraphicEngine
    {
        /// <summary>
        /// [VBO&VAO] Создает фигуру с шагом 3
        /// </summary>
        /// <param name="vertices">Vertices of Element</param>
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

        /// <summary>
        /// [VBO&VAO&EOB] Создает фигуру с шагом 3
        /// </summary>
        /// <param name="indices">Порядок отрисовки</param>
        /// <returns>Vertices Array Object of Element</returns>
        public int Create(float[] vertices, uint[] indices)
        {
            var vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            var vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            var ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            return vao;
        }
    }
}
