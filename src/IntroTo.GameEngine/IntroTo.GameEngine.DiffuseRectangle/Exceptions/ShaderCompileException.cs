namespace IntroTo.GameEngine.DiffuseRectangle.Exceptions;

public class ShaderCompileException : Exception
{
    public ShaderCompileException()
    {
    }

    public ShaderCompileException(string? message) : base(message)
    {
    }
}
