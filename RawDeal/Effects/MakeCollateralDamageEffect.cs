using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class MakeCollateralDamageEffect : Effect
{
    private readonly ImportantPlayerData _opponentData;
    
    public MakeCollateralDamageEffect(ImportantPlayerData superstarData, 
        ImportantPlayerData opponentData, View view) : base(superstarData, view)
    {
        _opponentData = opponentData;
    }

    public override void Apply()
    {
        View.SayThatPlayerDamagedHimself(PlayerData.Name, 1);
        View.SayThatSuperstarWillTakeSomeDamage(PlayerData.Name, 1);
        DecideToMakeDamageOrNot();
    }

    private void DecideToMakeDamageOrNot()
    {
        if (!PlayerData.DecksController.CheckForEmptyArsenal())
            ShowCardsBecauseOfDamage();
        else
            PlayerLostDueToSelfDamage();
    }

    private void ShowCardsBecauseOfDamage()
    {
        IViewableCardInfo drawnCard = PlayerData.DecksController.DrawLastCardOfArsenal();
        string formattedDrawnCard = Formatter.CardToString(drawnCard);
        View.ShowCardOverturnByTakingDamage(formattedDrawnCard, 1, 1);
        PlayerData.DecksController.PassCardToRingside(drawnCard);
    }
    
    private void PlayerLostDueToSelfDamage()
    {
        View.SayThatPlayerLostDueToSelfDamage(PlayerData.Name);
        throw new NoArsenalCardsException(_opponentData.Name);
    }
}