using System.Text.RegularExpressions;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class CardPlayerDiscardsController : CardController
{
    public CardPlayerDiscardsController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view) { }

    public override void ApplyEffect()
    {
        if (!PlayerData.DecksController.CheckForEmptyArsenal())
        {
            var effectPattern = @"When successfully played, discard (\d+) card(s?) of your choice from your hand";
            var match = Regex.Match(SelectedPlay.CardInfo.CardEffect, effectPattern);
            
            TryToReverse();
            DiscardCards(match);
        }
    }
    
    private void DiscardCards(Match match)
    {
        int numberOfCardsToDiscard = Convert.ToInt32(match.Groups[1].Value);
        var effect = new AskToDiscardHandCardsEffect(PlayerData, View, numberOfCardsToDiscard);
        effect.Apply();
    }
}