namespace RawDeal.Exceptions;

public class EndOfPrincipalLoop : ApplicationException
{
    public EndOfPrincipalLoop(string message) : base(message) { }
}