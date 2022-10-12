namespace IntroTo.GameEngine.Rectangle.Exceptions;

public class ShaderCompileException : Exception
{
    public ShaderCompileException()
    {
    }

    public ShaderCompileException(string? message) : base(message)
    {
    }
}
