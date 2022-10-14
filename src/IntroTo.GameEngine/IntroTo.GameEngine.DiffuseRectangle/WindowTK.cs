using IntroTo.GameEngine.DiffuseRectangle.Exceptions;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;
using System.Runtime.InteropServices;

namespace IntroTo.GameEngine.DiffuseRectangle;

public class WindowTK : GameWindow
{
    public WindowTK(
        int width,
        int height,
        string title) 
    : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Context.SwapInterval = 1;
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

        //float speed = 1.2f;
        //float maxAgree = 45.0f;
        //RotateFigure(
        //    FigureShader.Handle, 
        //    agree: (float)MathHelper.Sin(GLFW.GetTime() * speed) * maxAgree, 
        //    rotateUniformName: "RotationMatrix");
        //SmoothlyChangeColor(
        //    FigureShader.Handle, 
        //    timeUniformName: "Time");

        DrawFigureVertices();

        SwapBuffers();
    }

    private void ConfigureFigureVertexes()
    {
        #region Vertex buffer and Color data
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
             0.0f, 0.5f, 0.0f
        };
        #endregion

        #region Create position buffer handle
        int positionBufferHandle = GL.GenBuffer(); 
        GL.BindBuffer(BufferTarget.ArrayBuffer, positionBufferHandle);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertexBuffer.Vertices.Length * sizeof(float),
            vertexBuffer.Vertices,
            BufferUsageHint.StaticDraw);
        #endregion

        #region Create color buffer handle
        int colorBufferHandle = GL.GenBuffer(); 
        GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferHandle);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            colorData.Length * sizeof(float),
            colorData,
            BufferUsageHint.StaticDraw);
        #endregion

        #region Create and fill an VAO

        int sizeOfVector3 = 3;
        int positionAttribLocation = FigureShader.GetAttribLocation("VertexPosition");
        int colorAttribLocation = FigureShader.GetAttribLocation("VertexColor");

        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        GL.BindBuffer(BufferTarget.ArrayBuffer, positionBufferHandle);
        SetVertexAttribPointer(positionAttribLocation, sizeOfVector3);

        GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferHandle);
        SetVertexAttribPointer(colorAttribLocation, sizeOfVector3);
        #endregion

        FillUniformBlock(FigureShader.Handle, "BlobSettings");

        #region Create EBO

        uint[] indices = {
            0, 1, 3,
            1, 2, 3
        };

        int elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(
            BufferTarget.ElementArrayBuffer,
            size: indices.Length * sizeof(uint),
            data: indices,
            BufferUsageHint.StaticDraw);
        FigureIndicesCount = indices.Length;
        #endregion
    }

    private static void RotateFigure(int programHandle, float agree, string rotateUniformName)
    {
        var rotationResult = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(agree));
        var rotationMatrixAttribLocation = GL.GetUniformLocation(programHandle, rotateUniformName);
        if (rotationMatrixAttribLocation < 0)
        {
            throw new AttributeNotFoundException(
                $"Uniform named {rotationMatrixAttribLocation} not found in program {programHandle}");
        }
        GL.UniformMatrix4(rotationMatrixAttribLocation, true, ref rotationResult);
    }

    private static void SmoothlyChangeColor(int programHandle, string timeUniformName)
    {
        var timeUniformLocation = GL.GetUniformLocation(programHandle, timeUniformName);
        var glTime = GLFW.GetTime();
        int uniformVariableIsNotAnArray = 1;
        GL.Uniform1(
            timeUniformLocation, 
            count: uniformVariableIsNotAnArray, 
            value: ref glTime);
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

    private static void FillUniformBlock(int program, string blockName)
    {
        var uniformBufferIndex = GL.GetUniformBlockIndex(program, blockName);
        GL.GetActiveUniformBlock(
            program,
            uniformBufferIndex,
            ActiveUniformBlockParameter.UniformBlockDataSize,
            out int blockSize);

        string[] names = {
            "InnerColor",
            "OuterColor",
            "RadiusInner",
            "RadiusOuter"
        };
        int[] indices = new int[names.Length]; // индексы переменных в блоке
        GL.GetUniformIndices(program, names.Length, names, indices);

        int[] offset = new int[names.Length];
        GL.GetActiveUniforms(
            program, 
            uniformCount: names.Length,
            uniformIndices: indices, 
            ActiveUniformParameter.UniformOffset, 
            offset); // смещение каждой переменной в блоке

        float[] outerColor = { 0.0f, 0.0f, 0.0f, 0.0f };
        float[] innerColor = { 1.0f, 1.0f, 0.75f, 1.0f };
        float innerRadius = 0.25f, outerRadius = 0.45f;

        var bufferData = new List<float>();
        var blockBuffer = new BlobSettings
        {
            OuterColor = new Vector4(0.0f, 0.0f, 0.0f, 0.0f),
            InnerColor = new Vector4(1.0f, 1.0f, 0.75f, 1.0f),
            InnerRadius = 0.25f,
            OuterRadius = 0.45f
        };

        var uniformBufferObject = GL.GenBuffer(); CheckErrors();
        GL.BindBuffer(BufferTarget.UniformBuffer, uniformBufferObject);
        GL.BufferData(BufferTarget.UniformBuffer,
            (IntPtr)BlobSettings.Size,
            ref blockBuffer,
            BufferUsageHint.DynamicDraw); 
        CheckErrors();

        GL.BindBufferBase(
            BufferRangeTarget.UniformBuffer, 
            index: 0,
            uniformBufferObject);
        CheckErrors();
    }

    private static void ArrayOffset(List<float> array, int offset)
    {
        for (int i = 0; i < offset; i++)
            array.Add(0.0f);
    }

    private static void CheckErrors()
    {
        var error = GL.GetError();
        while (error != OpenTK.Graphics.OpenGL4.ErrorCode.NoError)
        {
            Console.WriteLine("ERROR: " + error);
            throw new OpenGlException("GL Error: " + error);
        }
    }

    #region NotFamous

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        var size = new Size(Size.X, Size.Y);
        GL.Viewport(size);
    }

    #endregion

    struct BlobSettings
    {
        public Vector4 OuterColor;
        public Vector4 InnerColor;
        public float InnerRadius;
        public float OuterRadius;

        public static readonly int Size = Marshal.SizeOf<BlobSettings>();
    }
}
