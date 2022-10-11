using IntroTo.GameEngine.Exceptions;
using OpenTK.Graphics.OpenGL4;

namespace IntroTo.GameEngine;

public class Shader
{
    private readonly string _vertexPath;
    private readonly string _fragmentPath;

    public Shader(
        string vertexPath, 
        string fragmentPath,
        int handle)
    {
        _vertexPath = vertexPath;
        _fragmentPath = fragmentPath;
        Handle = handle;
    }

    public int Handle { get; }
    public int VertexShader { get; private set; }
    public int FragmentShader { get; private set; }

    public void Load()
    {
        var vertexShaderSource = File.ReadAllText(_vertexPath);
        var fragmentShaderSource = File.ReadAllText(_fragmentPath);

        VertexShader = InitShader(vertexShaderSource, ShaderType.VertexShader);
        FragmentShader = InitShader(fragmentShaderSource, ShaderType.FragmentShader);

        CompileShader(VertexShader);
        CompileShader(FragmentShader);
    }

    private static int InitShader(string shaderSource, ShaderType shaderType)
    {
        var shader = GL.CreateShader(shaderType);
        GL.ShaderSource(shader, shaderSource);
        return shader;
    }

    private static void CompileShader(int shader)
    {
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        Console.WriteLine("Compile shader status -> " + success);
        if (success is 0)
        {
            var errorLogs = GL.GetShaderInfoLog(shader);
            throw new ShaderCompileException(errorLogs);
        }
    }
}
