using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class BasicReversalCardController : BasicCardController
{
    public BasicReversalCardController(ImportantPlayerData playerData, ImportantPlayerData opponentData,
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view)
    {
        View = view;
        PlayerData = playerData;
        OpponentData = opponentData;
        SelectedPlay = selectedPlay;
    }
    
    public override void ApplyEffect()
    {
        var (reversalCardsInfo, reversalCards) = ReversalFromHand();
        var numReversalToPlay = View.AskUserToSelectAReversal(OpponentData.Name, reversalCardsInfo);
        if (numReversalToPlay != -1)
        {
            View.SayThatPlayerReversedTheCard(OpponentData.Name, reversalCardsInfo[numReversalToPlay]);
            OpponentData.DecksController.PassCardFromHandToRingArea(reversalCards[numReversalToPlay]);
            // throw new EndOfTurnException();
        }
    }
    
    private (List<string>, List<IViewableCardInfo>) ReversalFromHand()
    {
        var subtype = SelectedPlay.CardInfo.Subtypes[0]; 
        if (SelectedPlay.PlayedAs == "ACTION")
        {
            return OpponentData.DecksController.SearchForReversalInHand($"ReversalAction");
        }
        return OpponentData.DecksController.SearchForReversalInHand($"Reversal{subtype}");
    }
    
}