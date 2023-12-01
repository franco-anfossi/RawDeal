namespace RawDeal.Exceptions;

public class NoArsenalCardsException : OptionPlayCardException
{
    public NoArsenalCardsException(string message) : base(message) { }
}