using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenGl.SapperTK.Entities
{
    internal class Shader
    {
        public int UID { get; set; }
        private readonly string _vertexShaders = string.Empty;
        private readonly string _fragmentShaders = string.Empty;

        public Shader(string vertexShaderPath, string fragmentShaderPath)
        {
            _vertexShaders = File.ReadAllText(vertexShaderPath);
            _fragmentShaders = File.ReadAllText(fragmentShaderPath);

            UID = CreateShaderProgram(_vertexShaders, _fragmentShaders);
        }

        public void Use()
        {
            GL.UseProgram(UID);
        }

        private int CreateShaderProgram(string vertexShaders, string fragmentShaders)
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
    }
}
