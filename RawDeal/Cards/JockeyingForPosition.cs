using RawDeal.Data_Structures;
using RawDeal.Effects;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class JockeyingForPosition : CardController
{
    public JockeyingForPosition(ImportantPlayerData playerData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(playerData, opponentData, selectedPlay, view) { }

    public override void ApplyEffect()
    {
        var effect = new JockeyingForPositionEffect(PlayerData, OpponentData, SelectedPlay, View);
        effect.Apply();
    }
}