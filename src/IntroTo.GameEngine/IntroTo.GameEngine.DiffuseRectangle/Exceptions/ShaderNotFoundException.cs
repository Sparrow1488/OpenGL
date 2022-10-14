namespace IntroTo.GameEngine.DiffuseRectangle.Exceptions;

public class ShaderNotFoundException : Exception
{
    public ShaderNotFoundException()
    {
    }

    public ShaderNotFoundException(string? message) : base(message)
    {
    }
}
