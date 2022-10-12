namespace IntroTo.GameEngine.Rectangle;

public struct VertexBuffer
{
    public VertexBuffer(float[] vertices)
    {
        Vertices = vertices;
    }

    public float[] Vertices { get; set; }
}
