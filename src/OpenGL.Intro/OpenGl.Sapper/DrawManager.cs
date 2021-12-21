using SharpGL;

namespace OpenGl.Sapper
{
    public static class DrawManager
    {
        public static void DrowQuadLeftBottom(OpenGL gl, float size)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.Scale(2f / 5f, 2f / 5f, 1f);
            gl.Translate(-5 * 0.5f, -5 * 0.5f, -1f);
            gl.Begin(OpenGL.GL_QUADS);

            gl.Vertex(0, 0);
            gl.Vertex(0, size);
            gl.Vertex(size, size);
            gl.Vertex(size, 0);

            gl.End();
            gl.Flush();
        }
    }
}
