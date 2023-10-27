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
    
    public MakeDamageEffect(ImportantPlayerData superstarData, ImportantPlayerData opponentData, 
        IViewablePlayInfo selectedPlay, View view) : base(superstarData, view)
    {
        _opponentData = opponentData;
        _selectedPlay = selectedPlay;
    }

    public override void Apply()
    {
        HandleNonReversalCardPlay();
        MakeDamageToOpponent();
        AddFortitudeToPlayer();
    }
    
    private void HandleNonReversalCardPlay()
    {
        if (CheckIfCardIsNotReversal())
        {
            View.SayThatPlayerSuccessfullyPlayedACard();
            PlayerData.DecksController.PassCardFromHandToRingArea(_selectedPlay.CardInfo);
        }
    }

    private void AddFortitudeToPlayer()
    {
        if (!CheckIfDamageIsUnknown())
        {
            int damageDone = Convert.ToInt32(_selectedPlay.CardInfo.Damage);
            PlayerData.SuperstarData.Fortitude += damageDone;
        }
    }

    private void MakeDamageToOpponent()
    {
        int damageAdded = HandleDamageAddedByJockeyingForPosition();
        int damageDone = HandleUnknownDamage() + damageAdded;
        damageDone = ReduceDamageIfMankind(damageDone);
        
        if (damageDone > 0)
        {
            View.SayThatSuperstarWillTakeSomeDamage(_opponentData.Name, damageDone);
            ShowDamageDoneToOpponent(damageDone);
        }
        
        ResetChangesByJockeyingForPosition();
    }
    
    private int HandleDamageAddedByJockeyingForPosition()
    {
        int damageAdded = 0;
        if (CheckIfSelectedCardIsGrapple())
            damageAdded = PlayerData.ChangesByJockeyingForPosition.DamageAdded;
        
        return damageAdded;
    }
    
    private int HandleUnknownDamage()
    {
        var damageDone = Convert.ToInt32(
            CheckIfDamageIsUnknown() ? _selectedPlay.CardInfo.Damage[1..] : _selectedPlay.CardInfo.Damage);
        return damageDone;
    }
    
    private void ResetChangesByJockeyingForPosition()
    {
        PlayerData.ChangesByJockeyingForPosition.Reset();
        _opponentData.ChangesByJockeyingForPosition.Reset();
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
            throw new NoArsenalCardsException(PlayerData.Name);
    }

    private void ShowCardsBecauseOfDamage(int currentDamage, int totalDamageDone)
    {
        IViewableCardInfo drawnCard = _opponentData.DecksController.DrawLastCardOfArsenal();
        string formattedDrawnCard = Formatter.CardToString(drawnCard);
        View.ShowCardOverturnByTakingDamage(formattedDrawnCard, currentDamage, totalDamageDone);
        var damageCompleted = currentDamage == totalDamageDone;
        HandleArsenalReversal(damageCompleted, drawnCard);
        _opponentData.DecksController.PassCardToRingside(drawnCard);
    }

    private void HandleArsenalReversal(bool damageCompleted, IViewableCardInfo drawnCard)
    {
        if (CheckIfCardIsNotReversal())
        {
            var reversalFromArsenal = new ReversalFromArsenalController(
                PlayerData, _opponentData, _selectedPlay, drawnCard, damageCompleted, View);
            reversalFromArsenal.ReviewIfReversalPlayable();
        }
    }
    
    private int ReduceDamageIfMankind(int damageDone)
    {
        if (_opponentData.Name == "MANKIND")
            damageDone--;
        return damageDone;
    }

    private bool CheckIfSelectedCardIsGrapple()
    {
        return _selectedPlay.CardInfo.Subtypes.Contains("Grapple");
    }
    
    private bool CheckIfCardIsNotReversal()
    {
        return _selectedPlay.PlayedAs != "REVERSAL";
    }
    
    private bool CheckIfDamageIsUnknown()
    {
        return _selectedPlay.CardInfo.Damage[0] == '#';
    }
}