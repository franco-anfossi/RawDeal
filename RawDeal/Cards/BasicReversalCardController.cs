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
        var reversalCards = ReversalFromHand();
        var reversalPlaysInfo = OpponentData.DecksController.CreateFormattedPlays(reversalCards);
        var numReversalToPlay = View.AskUserToSelectAReversal(OpponentData.Name, reversalPlaysInfo);
        if (numReversalToPlay != -1)
        {
            View.SayThatPlayerReversedTheCard(OpponentData.Name, reversalPlaysInfo[numReversalToPlay]);
            OpponentData.DecksController.PassCardFromHandToRingArea(reversalCards[numReversalToPlay]);
            PlayerData.DecksController.PassCardFromHandToRingside(SelectedPlay.CardInfo);
            throw new EndOfTurnException();
        }
    }
    
    private List<IViewableCardInfo> ReversalFromHand()
    {
        if (SelectedPlay.PlayedAs == "ACTION")
        {
            return OpponentData.DecksController.SearchForReversalInHand($"ReversalAction");
        }
        var subtype = SelectedPlay.CardInfo.Subtypes[0];
        return OpponentData.DecksController.SearchForReversalInHand($"Reversal{subtype}");
    }
    
}