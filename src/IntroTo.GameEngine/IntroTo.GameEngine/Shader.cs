using IntroTo.GameEngine.Exceptions;
using OpenTK.Graphics.OpenGL4;

namespace IntroTo.GameEngine;

public class Shader
{
    public Shader(
        string vertexPath, 
        string fragmentPath)
    {
        VertexShader = Load(vertexPath, ShaderType.VertexShader);
        FragmentShader = Load(fragmentPath, ShaderType.FragmentShader);

        CompileShader(VertexShader);
        CompileShader(FragmentShader);

        Handle = GL.CreateProgram();
        AttachToProgram(VertexShader, Handle);
        AttachToProgram(FragmentShader, Handle);

        DeleteShaderFromProgram(VertexShader, Handle);
        DeleteShaderFromProgram(FragmentShader, Handle);
    }

    /// <summary>
    ///     Shader Program
    /// </summary>
    public int Handle { get; }
    public int VertexShader { get; private set; }
    public int FragmentShader { get; private set; }

    /// <summary>
    ///     Load and generate shader using OpenGL
    /// </summary>
    /// <returns>Shader</returns>
    private int Load(string shaderPath, ShaderType shaderType)
    {
        if (!File.Exists(shaderPath))
        {
            throw new ShaderNotFoundException($"Shader at \"{shaderPath}\" not found");
        }

        var shaderSource = File.ReadAllText(shaderPath);
        var shader = InitShader(shaderSource, shaderType);
        return shader;
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
        Console.WriteLine("Compile shader status -> " + success); // TODO: REMOVE
        if (success is 0)
        {
            var errorLogs = GL.GetShaderInfoLog(shader);
            throw new ShaderCompileException(errorLogs);
        }
    }

    private void AttachToProgram(int shader, int program)
    {
        GL.AttachShader(program, shader);

        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
        Console.WriteLine("Link program status -> " + success); // TODO: REMOVE
        if (success is 0)
        {
            var linkLogs = GL.GetProgramInfoLog(program);
            throw new LinkProgramException(linkLogs);
        }
    }

    private static void DeleteShaderFromProgram(int shader, int program)
    {
        GL.DetachShader(program, shader);
        GL.DeleteShader(shader);
    }

    public void Use() => GL.UseProgram(Handle);
}
