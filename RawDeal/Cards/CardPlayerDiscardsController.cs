using System.Text.RegularExpressions;
using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class CardPlayerDiscardsController : BasicCardController
{
    public CardPlayerDiscardsController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view)
    {
        View = view;
        SelectedPlay = selectedPlay;
        PlayerData = playerData;
        OpponentData = opponentData;
    }

    public override void ApplyEffect()
    {
        if (!PlayerData.DecksController.CheckForEmptyArsenal())
        {
            var effectPattern = @"When successfully played, discard (\d+) card(s?) of your choice from your hand";
            var match = Regex.Match(SelectedPlay.CardInfo.CardEffect, effectPattern);

            int numberOfCardsToDiscard = Convert.ToInt32(match.Groups[1].Value);
            
            TryToReverse();
            
            var effect = new AskToDiscardHandCardsEffect(PlayerData, View, numberOfCardsToDiscard); 
            effect.Apply();
        }
    }
}