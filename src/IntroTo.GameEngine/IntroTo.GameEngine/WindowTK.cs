using IntroTo.GameEngine.Exceptions;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace IntroTo.GameEngine;

public class WindowTK : GameWindow
{
    public WindowTK(
        int width,
        int height,
        string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Size = (width, height); 
        Title = title;

        _vertexBuffer = new(new[] {
                -0.5f,-0.5f, 0.0f,
                 0.0f, 0.5f, 0.0f,
                 0.5f,-0.5f, 0.0f
            }
        );
    }

    private int _vertexBufferObject;
    private VertexBuffer _vertexBuffer;

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        DrawTriangle();
    }

    private void DrawTriangle()
    {
        _vertexBufferObject = GL.GenBuffer();

        GL.BindBuffer(
            BufferTarget.ArrayBuffer,
            _vertexBufferObject); // привязка созданного буффера к определенному типу

        GL.BufferData(
            BufferTarget.ArrayBuffer,
            _vertexBuffer.Vertices.Length * sizeof(float),
            _vertexBuffer.Vertices,
            BufferUsageHint.StaticDraw); // BufferUsageHint - то, как мы хотим, чтобы видюха обрабатывала данные

        CreateShaderProgram();
    }

    private static void CreateShaderProgram()
    {
        var program = GL.CreateProgram();
        var shader = new Shader("./Shaders/shader.vert", "./Shaders/shader.frag", program);
        shader.Load();

        GL.AttachShader(program, shader.VertexShader);
        GL.AttachShader(program, shader.FragmentShader);

        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
        Console.WriteLine("Link program status -> " + success);
        if (success is 0)
        {
            var linkLogs = GL.GetProgramInfoLog(program);
            throw new LinkProgramException(linkLogs);
        }
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        var state = KeyboardState;
        if (state.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        var size = new Size(Size.X, Size.Y);
        GL.Viewport(size);
    }
}
