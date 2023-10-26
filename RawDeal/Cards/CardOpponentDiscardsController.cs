using System.Text.RegularExpressions;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class CardOpponentDiscardsController : BasicCardController
{
    public CardOpponentDiscardsController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view)
    {
        View = view;
        SelectedPlay = selectedPlay;
        PlayerData = playerData;
        OpponentData = opponentData;
    }
    
    public override void ApplyEffect()
    {
        if (!OpponentData.DecksController.CheckForEmptyArsenal())
        {
            var effectPattern = @"When successfully played, opponent must discard (\d+) card(s?)";
            var match = Regex.Match(SelectedPlay.CardInfo.CardEffect, effectPattern);

            int numberOfCardsToDiscard = Convert.ToInt32(match.Groups[1].Value);
            
            TryToReverse();

            var effect = new AskToDiscardHandCardsEffect(OpponentData, View, numberOfCardsToDiscard);
            effect.Apply();
        }
    }
}