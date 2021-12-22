using OpenTK.Graphics.OpenGL;

namespace OpenGl.SapperTK.DrawTools
{
    internal static class UIElements
    {
        public static int CreateButton(float[] vertices)
        {
            return CreateElement(vertices, new uint[]
            {
                0, 1, 2,
                0, 3, 2
            });
        }

        public static int CreateCenterQuadre(float size)
        {
            float half = size / 2;
            var vertices = new float[]
            {
                -half, -half, 0f,
                -half, half, 0f,
                half, half, 0f,
                half, -half, 0f
            };
            var indices = new uint[]
            {
                0, 1, 2,
                0, 2, 3
            };
            return CreateElement(vertices, indices);
        }

        public static int CreateShaderProgram(string vertexShaders, string fragmentShaders)
        {
            int vertexShader;
            int fragmentShader;
            
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaders);
            GL.CompileShader(vertexShader);
            
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaders);
            GL.CompileShader(fragmentShader);

            int shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);

            GL.DetachShader(shaderProgram, vertexShader);
            GL.DetachShader(shaderProgram, fragmentShader);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            GL.UseProgram(shaderProgram);

            return shaderProgram;
        }

        /// <returns>VAO</returns>
        public static int CreateElement(float[] vertices, uint[] indices)
        {
            int verticesBufferObject;
            int elementsBufferObject;
            int vertexArrayObject;

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexArrayObject);

            verticesBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferObject);

            elementsBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsBufferObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            return vertexArrayObject;
        }

        
    }
}
