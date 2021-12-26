using OpenTK.Graphics.OpenGL;
using System.IO;

namespace OpenGl.IntroTwo.Entities
{
    internal class Shader
    {
        public int Id { get; set; }
        private readonly string _vertexShader;
        private readonly string _fragmentShader;

        public Shader(string vertexShaderName, string fragmentShaderName, string directory)
        {
            string root = "./Shaders";
            _vertexShader = File.ReadAllText(Path.Combine(root, directory, vertexShaderName));
            _fragmentShader = File.ReadAllText(Path.Combine(root, directory, fragmentShaderName));
        }

        public Shader Create()
        {
            var vShader = GL.CreateShader(ShaderType.VertexShader);
            var fShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(vShader, _vertexShader);
            GL.ShaderSource(fShader, _fragmentShader);
            GL.CompileShader(vShader);
            GL.CompileShader(fShader);

            Id = GL.CreateProgram();
            GL.AttachShader(Id, vShader);
            GL.AttachShader(Id, fShader);

            GL.DeleteShader(vShader);
            GL.DeleteShader(fShader);
            GL.LinkProgram(Id);

            return this;
        }

        public void Use()
        {
            GL.UseProgram(Id);
        }
    }
}
