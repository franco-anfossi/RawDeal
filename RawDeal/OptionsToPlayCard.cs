using RawDealView.Formatters;
using RawDealView;

namespace RawDeal;

public class OptionsToPlayCard
{
    private List<IViewablePlayInfo> _notFormattedPossiblePlays = new();
    private List<string> _formattedPossiblePlays = new();
    
    private IViewablePlayInfo _notFormattedSelectedPlay;
    private string _formattedSelectedPlay;
    
    private View _view;
    
    private Player _inTurnPlayer;
    private Player _opponentPlayer;
    
    private bool _activeGame = true;

    public OptionsToPlayCard(Player inTurnPlayer, Player opponentPlayer, View view)
    {
        _view = view;
        _inTurnPlayer = inTurnPlayer;
        _opponentPlayer = opponentPlayer;
        CreatePlays();
        FormatPlay();
    }

    private void CreatePlays()
    {
        List<IViewableCardInfo> playableCards = _inTurnPlayer.CheckForPlayableCards();
        foreach (var playableCard in playableCards)
            SavePlayablePlay(playableCard);
    }

    private void FormatPlay()
    {
        foreach (var possiblePlay in _notFormattedPossiblePlays)
        {
            string formattedPlay = Formatter.PlayToString(possiblePlay);
            _formattedPossiblePlays.Add(formattedPlay);
        }
    }

    public bool StartElectionProcess()
    {
        int selectedPlayNumber = _view.AskUserToSelectAPlay(_formattedPossiblePlays);
        if (_formattedPossiblePlays.Count >= 0 && selectedPlayNumber != -1)
        {
            AskUserToPlayCardAndPutItIntoRingArea(selectedPlayNumber);
            _inTurnPlayer.TryToPlayCard(_formattedSelectedPlay);
            _view.SayThatPlayerSuccessfullyPlayedACard();
            MakeDamageToOpponent();
        }

        return _activeGame;
    }

    private void SavePlayablePlay(IViewableCardInfo playableCard)
    {
        string cardType = playableCard.Types[0].ToUpper();
        if (cardType != "REVERSAL")
        {
            PlayInfo possiblePlayInfo = new PlayInfo(playableCard, cardType);
            _notFormattedPossiblePlays.Add(possiblePlayInfo);
        }
    }
    private void AskUserToPlayCardAndPutItIntoRingArea(int selectedPlayNumber)
    {
        _formattedSelectedPlay = _formattedPossiblePlays[selectedPlayNumber];
        _notFormattedSelectedPlay = _notFormattedPossiblePlays[selectedPlayNumber];
        _inTurnPlayer.PassCardFromHandToRingArea(_notFormattedSelectedPlay.CardInfo);
    }

    private void MakeDamageToOpponent()
    {
        IViewableCardInfo selectedCard = _notFormattedSelectedPlay.CardInfo;
        int damageDone = Convert.ToInt32(selectedCard.Damage);
        _inTurnPlayer.AttackTheOpponent(damageDone);
        if (_opponentPlayer.CheckIfPlayerIsMankind())
        {
            damageDone--;
        }
        
        ShowDamageDoneToOpponent(damageDone);
    }

    private void ShowDamageDoneToOpponent(int totalDamageDone)
    {
        for (int currentDamage = 1; currentDamage <= totalDamageDone; currentDamage++)
            DecideToMakeDamageOrNot(currentDamage, totalDamageDone);
    }

    private void DecideToMakeDamageOrNot(int currentDamage, int totalDamageDone)
    {
        if (!_opponentPlayer.CheckForEmptyArsenal())
            ShowCardsBecauseOfDamage(currentDamage, totalDamageDone);
        else
            _activeGame = false;
    }

    private void ShowCardsBecauseOfDamage(int currentDamage, int totalDamageDone)
    {
        IViewableCardInfo drawnCard = _opponentPlayer.PassCardFromArsenalToRingside();
        string formattedDrawnCard = Formatter.CardToString(drawnCard);
        _view.ShowCardOverturnByTakingDamage(formattedDrawnCard, currentDamage, totalDamageDone);
    }
}