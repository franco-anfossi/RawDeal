using RawDeal.Data_Structures;
using RawDeal.Decks;
using RawDealView.Formatters;

namespace RawDeal.OptionsPlayCard;

public class PlayableCardsManager
{
    private IViewableCardInfo? _playableCard;
    private readonly List<IViewablePlayInfo> _possiblePlays;
    private readonly PlayerDecksController _playerDecksController;
    
    public PlayableCardsManager(ImportantPlayerData playerData)
    {
        _possiblePlays = new List<IViewablePlayInfo>();
        _playerDecksController = playerData.DecksController;
    }

    public PossiblePlaysData BuildPlayablePlays()
    {
        FindPlayablePlays();
        var formattedPlays = FormatPlays();
        return new PossiblePlaysData(formattedPlays, _possiblePlays);
    } 
    
    private List<string> FormatPlays()
    {
        var formattedPlayablePlays = new List<string>();
        foreach (var playablePlay in _possiblePlays)
        {
            string formattedPlay = Formatter.PlayToString(playablePlay);
            formattedPlayablePlays.Add(formattedPlay);
        }

        return formattedPlayablePlays;
    }
    
    private void FindPlayablePlays()
    {
        var playableCards = _playerDecksController.CheckForPlayableCards();
        foreach (var playableCard in playableCards)
        {
            _playableCard = playableCard;
            EvaluatePlayablePlays();
        }
    }
    
    private void EvaluatePlayablePlays()
    {
        if (!CheckIfCardIsReversal())
        {
            if (CheckIfCardIsManeuver() && CheckIfCardIsAction())
                SavePlayablePlay(2);
            else
                SavePlayablePlay(1);
        }
    }

    private void SavePlayablePlay(int numberOfCardTypes)
    {
        for (int typesIndex = 0; typesIndex < numberOfCardTypes; typesIndex++)
        {
            string cardType = _playableCard!.Types[typesIndex].ToUpper();
            var possiblePlayInfo = new PlayInfo(_playableCard, cardType);
            _possiblePlays.Add(possiblePlayInfo);
        }
    }

    private bool CheckIfCardIsManeuver()
    {
        return _playableCard!.Types.Contains("Maneuver");
    }
    
    private bool CheckIfCardIsAction()
    {
        return _playableCard!.Types.Contains("Action");
    }
    
    private bool CheckIfCardIsReversal()
    {
        return _playableCard!.Types[0].Contains("Reversal");
    }
}