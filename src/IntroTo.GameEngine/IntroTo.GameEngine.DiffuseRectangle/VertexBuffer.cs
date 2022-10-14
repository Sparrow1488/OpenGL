namespace IntroTo.GameEngine.DiffuseRectangle;

public struct VertexBuffer
{
    public VertexBuffer(float[] vertices)
    {
        Vertices = vertices;
    }

    public float[] Vertices { get; set; }
}
