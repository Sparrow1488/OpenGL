using IntroTo.GameEngine.Exceptions;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace IntroTo.GameEngine;

public class WindowTK : GameWindow
{
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    private VertexBuffer _vertexBuffer;
    private int _program = 0;

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

    public Shader TriangleShader { get; private set; }

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

        var shaderProgram = CreateShaderProgram();

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // загружаем значения вершин
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject); // привязка
        GL.BufferData(
            BufferTarget.ArrayBuffer, 
            _vertexBuffer.Vertices.Length * sizeof(float), 
            _vertexBuffer.Vertices, 
            BufferUsageHint.StaticDraw); // загрузка

        // установим указатели на вершинные буферы (для наложения шейдеров)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.UseProgram(shaderProgram);

        var vertexArrayObject = GL.GenVertexArray();
        _vertexArrayObject = vertexArrayObject;
        GL.BindVertexArray(vertexArrayObject);
        // 2. copy our vertices array in a buffer for OpenGL to use
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(
            BufferTarget.ArrayBuffer, 
            _vertexBuffer.Vertices.Length * sizeof(float), 
            _vertexBuffer.Vertices, 
            BufferUsageHint.StaticDraw);
        // 3. then set our vertex attributes pointers
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }

    private int CreateShaderProgram()
    {
        var program = GL.CreateProgram();
        _program = program;
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

        // Так как мы загрузили и прикрепили шейдеры к программе, то надобности в них нет и мы можем открепить и удалить их
        GL.DetachShader(program, shader.VertexShader);
        GL.DetachShader(program, shader.FragmentShader);
        GL.DeleteShader(shader.VertexShader);
        GL.DeleteShader(shader.FragmentShader);

        return program;
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

        GL.UseProgram(_program);
        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        var size = new Size(Size.X, Size.Y);
        GL.Viewport(size);
    }
}
