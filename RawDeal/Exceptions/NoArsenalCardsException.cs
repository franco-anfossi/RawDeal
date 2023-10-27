namespace RawDeal.Exceptions;

public class NoArsenalCardsException : ApplicationException
{
    public NoArsenalCardsException(string message) : base(message) { }
}