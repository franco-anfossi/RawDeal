using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class MinimumCardsNecessary : Condition
{
    private readonly BoundaryList<IViewableCardInfo> _playerHand;
    private readonly int _numberOfCardsToHave;
    
    public MinimumCardsNecessary(IViewablePlayInfo selectedPlay, 
        ImportantPlayerData playerData, int numberOfCardsToHave) : base(selectedPlay)
    {
        var playerDecks = playerData.DecksController.BuildDecks();
        _playerHand = playerDecks.Hand;
        _numberOfCardsToHave = numberOfCardsToHave;
    }

    public override bool Check()
        => _playerHand.Count >= _numberOfCardsToHave;
}