using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDeal.Reversals;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class MakeDamageEffect : Effect
{
    private readonly IViewablePlayInfo _selectedPlay;
    private readonly ImportantPlayerData _opponentData;
    
    public MakeDamageEffect(
        ImportantPlayerData superstarData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(superstarData, view)
    {
        View = view;
        PlayerData = superstarData;
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
    }

    public override void Apply()
    {
        var reversalFromHand = new ReversalFromHandController(PlayerData, _opponentData, _selectedPlay, View);
        reversalFromHand.SelectReversalFromHand();
        
        View.SayThatPlayerSuccessfullyPlayedACard();
        PlayerData.DecksController.PassCardFromHandToRingArea(_selectedPlay.CardInfo);
        MakeDamageToOpponent();
        AddFortitudeToPlayer();
    }

    private void AddFortitudeToPlayer()
    {
        int damageDone = Convert.ToInt32(_selectedPlay.CardInfo.Damage);
        PlayerData.SuperstarData.Fortitude += damageDone;
    }

    private void MakeDamageToOpponent()
    {
        int damageDone = Convert.ToInt32(_selectedPlay.CardInfo.Damage);
        damageDone = ReduceDamageIfMankind(damageDone);
        if (damageDone > 0)
        {
            View.SayThatSuperstarWillTakeSomeDamage(_opponentData.Name, damageDone);
            ShowDamageDoneToOpponent(damageDone);
        }
        
    }

    private void ShowDamageDoneToOpponent(int totalDamageDone)
    {
        for (int currentDamage = 1; currentDamage <= totalDamageDone; currentDamage++)
            DecideToMakeDamageOrNot(currentDamage, totalDamageDone);
    }

    private void DecideToMakeDamageOrNot(int currentDamage, int totalDamageDone)
    {
        if (!_opponentData.DecksController.CheckForEmptyArsenal())
            ShowCardsBecauseOfDamage(currentDamage, totalDamageDone);
        else
            throw new NoArsenalCardsException();
    }

    private void ShowCardsBecauseOfDamage(int currentDamage, int totalDamageDone)
    {
        IViewableCardInfo drawnCard = _opponentData.DecksController.DrawLastCardOfArsenal();
        string formattedDrawnCard = Formatter.CardToString(drawnCard);
        View.ShowCardOverturnByTakingDamage(formattedDrawnCard, currentDamage, totalDamageDone);
        var damageCompleted = currentDamage == totalDamageDone;
        var reversalFromArsenal = new ReversalFromArsenalController(
            PlayerData, _opponentData, _selectedPlay, drawnCard, damageCompleted, View);
        reversalFromArsenal.ReviewIfReversalPlayable();
        _opponentData.DecksController.PassCardToRingside(drawnCard);
    }
    
    private int ReduceDamageIfMankind(int damageDone)
    {
        if (_opponentData.Name == "MANKIND")
            damageDone--;
        return damageDone;
    }
}