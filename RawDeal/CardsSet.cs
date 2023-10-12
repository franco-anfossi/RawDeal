using RawDeal.Cards;
using RawDeal.Data_Structures;
using RawDeal.Superstars;

namespace RawDeal;

public class CardsSet
{
    private readonly List<CardData> _possibleCards;
    private readonly List<Player> _possibleSuperstars;
    
    public CardsSet(List<CardData> possibleCards, List<Player> possibleSuperstars)
    {
        _possibleCards = possibleCards;
        _possibleSuperstars = possibleSuperstars;
    }
    
    public List<CardData> PossibleCards => _possibleCards;
    public List<Player> PossibleSuperstars => _possibleSuperstars;
}

