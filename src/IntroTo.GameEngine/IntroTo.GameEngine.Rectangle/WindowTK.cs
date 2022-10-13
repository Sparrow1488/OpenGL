using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Drawing;

namespace IntroTo.GameEngine.Rectangle;

public class WindowTK : GameWindow
{
    public WindowTK(
        int width,
        int height,
        string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Context.SwapInterval = 2;
        Size = (width, height); 
        Title = title;
        FigureShader = new("./Shaders/basic.vert", "./Shaders/basic.frag");
    }

    public Shader FigureShader { get; private set; }
    public int VertexArrayObject { get; private set; }
    public int FigureIndicesCount { get; private set; }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        var maxVertexAttributesSupported = GL.GetInteger(GetPName.MaxVertexAttribs);
        Console.WriteLine("Maximum number of vertex attributes supported -> " + maxVertexAttributesSupported);

        ConfigureFigureVertexes();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        FigureShader.Use();
        DrawFigureVertices();

        SwapBuffers();
    }

    private void ConfigureFigureVertexes()
    {
        VertexBuffer vertexBuffer = new(new[] {
                0.5f,  0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
               -0.5f, -0.5f, 0.0f,
               -0.5f,  0.5f, 0.0f
        });

        float[] colorData = {
             1.0f, 0.0f, 0.0f,
             0.0f, 1.0f, 0.0f,
             0.0f, 0.0f, 1.0f,
             0.2f, 0.3f, 0.4f
        };

        // Create position buffer handle:
        int positionBufferHandle = GL.GenBuffer(); 
        GL.BindBuffer(BufferTarget.ArrayBuffer, positionBufferHandle);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertexBuffer.Vertices.Length * sizeof(float),
            vertexBuffer.Vertices,
            BufferUsageHint.StaticDraw);

        // Create color buffer handle:
        int colorBufferHandle = GL.GenBuffer(); 
        GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferHandle);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            colorData.Length * sizeof(float),
            colorData,
            BufferUsageHint.StaticDraw);

        // Activate vertex attrib pointers (position and color in vertex shader)
        int sizeOfVector3 = 3;
        int positionAttribLocation = FigureShader.GetAttribLocation("VertexPosition");
        int colorAttribLocation = FigureShader.GetAttribLocation("VertexColor");
        
        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        // Закрепляем значение индекса переменной (в шейдере location) за буффером с позицией
        // То-есть переменной, на которую указывает индекс positionAttribLocation присваиваем значение из positionBufferHandle
        GL.BindBuffer(BufferTarget.ArrayBuffer, positionBufferHandle);
        SetVertexAttribPointer(positionAttribLocation, sizeOfVector3);

        // Закрепляем значение индекса переменной (в шейдере location) за буффером с цветом
        GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferHandle);
        SetVertexAttribPointer(colorAttribLocation, sizeOfVector3);

        // -> EBO хранит указатели indices на вершины. Это позволит складывать и отрисовывать фигуры, ссылаясь на вершины
        //    без необходимости копировать данные
        // !!!EBO необходимо создавать после создания и привязки VAO!
        uint[] indices = {
            0, 1, 3, // triangle one
            1, 2, 3  // triangle two
        };

        int elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(
            BufferTarget.ElementArrayBuffer,
            size: indices.Length * sizeof(uint),
            data: indices,
            BufferUsageHint.StaticDraw);
        FigureIndicesCount = indices.Length;
    }

    private static void SetVertexAttribPointer(int attribLocation, int sizeOfVector)
    {
        GL.EnableVertexAttribArray(attribLocation); // Активируем массивы вершинных атрибутов
        GL.VertexAttribPointer(
            attribLocation,
            sizeOfVector,
            VertexAttribPointerType.Float,
            normalized: false,
            sizeOfVector * sizeof(float),
            offset: 0);
    }

    private void DrawFigureVertices()
    {
        GL.DrawElements(
            BeginMode.Triangles,
            count: FigureIndicesCount,
            DrawElementsType.UnsignedInt,
            offset: 0);
    }

    #region NotFamous

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        var size = new Size(Size.X, Size.Y);
        GL.Viewport(size);
    }

    #endregion
}
