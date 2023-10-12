using RawDeal.Data_Structures;
using RawDeal.Exceptions;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Effects;

public class MakeDamageEffect : Effect
{
    private IViewablePlayInfo _selectedPlay;
    
    public MakeDamageEffect(
        ImportantPlayerData superstarData, IViewablePlayInfo selectedPlay, View view) : base(superstarData, view)
    {
        _view = view;
        _playerData = superstarData;
        _selectedPlay = selectedPlay;
    }

    public override void Apply()
    {
        MakeDamageToOpponent();
    }
    
    public void AddFortitudeToPlayer()
    {
        int damageDone = Convert.ToInt32(_selectedPlay.CardInfo.Damage);
        _playerData.SuperstarData.Fortitude += damageDone;
    }

    private void MakeDamageToOpponent()
    {
        int damageDone = Convert.ToInt32(_selectedPlay.CardInfo.Damage);
        damageDone = ReduceDamageIfMankind(damageDone);
        if (damageDone > 0)
        {
            _view.SayThatSuperstarWillTakeSomeDamage(_playerData.Name, damageDone);
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
        if (!_playerData.DecksController.CheckForEmptyArsenal())
            ShowCardsBecauseOfDamage(currentDamage, totalDamageDone);
        else
            throw new NoArsenalCardsException();
    }

    private void ShowCardsBecauseOfDamage(int currentDamage, int totalDamageDone)
    {
        IViewableCardInfo drawnCard = _playerData.DecksController.PassCardFromArsenalToRingside();
        string formattedDrawnCard = Formatter.CardToString(drawnCard);
        _view.ShowCardOverturnByTakingDamage(formattedDrawnCard, currentDamage, totalDamageDone);
    }
    
    private int ReduceDamageIfMankind(int damageDone)
    {
        if (_playerData.Name == "MANKIND")
            damageDone--;
        return damageDone;
    }
}