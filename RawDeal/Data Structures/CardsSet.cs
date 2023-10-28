using RawDeal.Boundaries;
using RawDeal.Superstars;

namespace RawDeal.Data_Structures;

public class CardsSet
{
    public BoundaryList<CardData> PossibleCards { get; } 
    public BoundaryList<Player> PossibleSuperstars { get; }
    
    public CardsSet(BoundaryList<CardData> possibleCards, BoundaryList<Player> possibleSuperstars)
    {
        PossibleCards = possibleCards;
        PossibleSuperstars = possibleSuperstars;
    }
}

