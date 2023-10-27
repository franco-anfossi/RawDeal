using System.Text.RegularExpressions;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class CardOpponentDiscardsController : CardController
{
    public CardOpponentDiscardsController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view) { }
    
    public override void ApplyEffect()
    {
        if (!OpponentData.DecksController.CheckForEmptyArsenal())
        {
            var effectPattern = @"When successfully played, opponent must discard (\d+) card(s?)";
            var match = Regex.Match(SelectedPlay.CardInfo.CardEffect, effectPattern);
            
            TryToReverse();
            DiscardCards(match);
        }
    }
    
    private void DiscardCards(Match match)
    {
        int numberOfCardsToDiscard = Convert.ToInt32(match.Groups[1].Value);
        var effect = new AskToDiscardHandCardsEffect(OpponentData, View, numberOfCardsToDiscard);
        effect.Apply();
    }
}