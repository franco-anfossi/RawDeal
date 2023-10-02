namespace RawDeal;

public class CardsSet
{
    private readonly List<Card> _possibleCards;
    private readonly List<Player> _possibleSuperstars;
    
    public CardsSet(List<Card> possibleCards, List<Player> possibleSuperstars)
    {
        _possibleCards = possibleCards;
        _possibleSuperstars = possibleSuperstars;
    }
    
    public List<Card> PossibleCards => _possibleCards;
    public List<Player> PossibleSuperstars => _possibleSuperstars;
}

