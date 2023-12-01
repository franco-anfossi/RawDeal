namespace RawDeal.Exceptions;

public class EndOfTurnException : OptionPlayCardException
{
    public EndOfTurnException(string message) : base(message) { }
}