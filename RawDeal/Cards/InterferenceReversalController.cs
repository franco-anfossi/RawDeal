using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDeal.Exceptions;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class InterferenceReversalController : BasicCardController
{
    public InterferenceReversalController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view)
    {
        View = view;
        SelectedPlay = selectedPlay;
        PlayerData = playerData;
        OpponentData = opponentData;
    }
    
    public override void ApplyEffect()
    {
        DrawCards();
        MakeDamage();
        throw new EndOfTurnException();
    }

    private void DrawCards()
    {
        Effect drawEffect;
        if (SelectedPlay.CardInfo.Title == "Manager Interferes")
        {
            drawEffect = new DrawCardsEffect(PlayerData, View, 1);
        }
        else
        {
            drawEffect = new DrawCardsEffect(PlayerData, View, 2);
        }
        drawEffect.Apply();
    }
}