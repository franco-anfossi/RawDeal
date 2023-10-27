using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDeal.Exceptions;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class InterferenceReversalController : CardController
{
    public InterferenceReversalController(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view) { }
    
    public override void ApplyEffect()
    {
        DrawCards();
        MakeDamage();
        throw new EndOfTurnException();
    }

    private void DrawCards()
    {
        Effect drawEffect = SelectedPlay.CardInfo.Title == "Manager Interferes" ? 
            new DrawCardsEffect(PlayerData, View, 1) : new DrawCardsEffect(PlayerData, View, 2);
        drawEffect.Apply();
    }
}