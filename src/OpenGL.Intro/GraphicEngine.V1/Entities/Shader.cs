using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.IO;
using System;
using GraphicEngine.V1.Intefaces;

namespace GraphicEngine.V1.Entities
{
    public class Shader : IClonable<Shader>
    {
        public int Id { get; set; } = -1;
        private readonly string _vertexShader = string.Empty;
        private readonly string _fragmentShader = string.Empty;

        private readonly string _vertexShaderName = string.Empty;
        private readonly string _fragmentShaderName = string.Empty;
        private readonly string _directory = string.Empty;
        private bool _wasCreated = false;

        public Shader(string vertexShaderName, string fragmentShaderName, string directory)
        {
            _vertexShaderName = vertexShaderName;
            _fragmentShaderName = fragmentShaderName;
            _directory = directory;

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

            _wasCreated = true;
            return this;
        }

        public void Use()
        {
            GL.UseProgram(Id);
        }

        public void UseColorAnimation(string uniformName)
        {
            Use();
            var time = GLFW.GetTime();
            var greenColor = (float)Math.Sin(time) / 2f + 0.5f;
            var uniformLocation = GL.GetUniformLocation(Id, uniformName);
            if (uniformLocation == -1)
                Console.WriteLine("Не удалось найти атрибут uniform с названием " + uniformName);
            GL.Uniform4(uniformLocation, new Color4(0f, greenColor, 0f, 1f));
        }

        public Shader SetMatrix4(string name, Matrix4 value)
        {
            Use();
            GL.UniformMatrix4(GL.GetUniformLocation(Id, name), true, ref value);
            return this;
        }

        public Shader SetVector4(string name, Color4 color)
        {
            Use();
            GL.Uniform4(GL.GetUniformLocation(Id, name), color);
            return this;
        }

        public Shader Clone()
        {
            var clone = new Shader(_vertexShaderName, _fragmentShaderName, _directory);
            return _wasCreated == true ? clone.Create() : clone;
        }
    }
}
