namespace IntroTo.GameEngine.Triangle;

public struct VertexBuffer
{
    public VertexBuffer(float[] vertices)
    {
        Vertices = vertices;
    }

    public float[] Vertices { get; set; }
}
