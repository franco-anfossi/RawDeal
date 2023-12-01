using RawDeal.Boundaries;
using RawDeal.Cards.Builders;
using RawDeal.Data_Structures;
using RawDeal.Decks;
using RawDealView.Formatters;

namespace RawDeal.OptionsPlayCard;

public class PlayableCardsManager
{
    private readonly ImportantPlayerData _playerData;
    private IViewableCardInfo _playableCard;
    private readonly BoundaryList<IViewablePlayInfo> _possiblePlays;
    private readonly PlayerDecksController _playerDecksController;
    
    public PlayableCardsManager(ImportantPlayerData playerData)
    {
        _playerData = playerData;
        _possiblePlays = new BoundaryList<IViewablePlayInfo>();
        _playerDecksController = playerData.DecksController;
    }

    public PossiblePlaysData BuildPlayablePlays()
    {
        FindPlayablePlays();
        var formattedPlays = FormatPlays();
        return new PossiblePlaysData(formattedPlays, _possiblePlays);
    } 
    
    private void FindPlayablePlays()
    {
        var playableCards = _playerDecksController.CheckForPlayableCards();
        foreach (IViewableCardInfo playableCard in playableCards)
        {
            _playableCard = playableCard;
            EvaluatePlayablePlays();
        }
        
        FilterByPrecondition();
    }
    
    private BoundaryList<string> FormatPlays()
    {
        var formattedPlayablePlays = new BoundaryList<string>();
        foreach (IViewablePlayInfo playablePlay in _possiblePlays)
        {
            string formattedPlay = Formatter.PlayToString(playablePlay);
            formattedPlayablePlays.Add(formattedPlay);
        }

        return formattedPlayablePlays;
    }

    private void FilterByPrecondition()
    {
        for (int index = 0; index < _possiblePlays.Count; index++)
        {
            var preconditionBuilder = new PreconditionBuilder(_possiblePlays[index], _playerData);
            var conditions = preconditionBuilder.BuildConditions();
            if (!conditions.All(condition => condition.Check()))
            {
                _possiblePlays.RemoveAt(index);
                index--;
            }
        }
    }
    
    private void EvaluatePlayablePlays()
    {
        if (!CheckIfCardIsReversal())
        {
            if (CheckIfCardIsManeuver() && CheckIfCardIsAction())
            {
                int numberOfCardTypesForPlays = 2;
                SavePlayablePlay(numberOfCardTypesForPlays);
            }
            else
            {
                int numberOfCardTypesForPlays = 1;
                SavePlayablePlay(numberOfCardTypesForPlays);
            }
        }
    }

    private void SavePlayablePlay(int numberOfCardTypes)
    {
        for (int typesIndex = 0; typesIndex < numberOfCardTypes; typesIndex++)
        {
            string cardType = _playableCard.Types[typesIndex].ToUpper();
            var possiblePlayInfo = new PlayInfo(_playableCard, cardType);
            _possiblePlays.Add(possiblePlayInfo);
        }
    }
    
    private bool CheckIfCardIsManeuver()
        => _playableCard.Types.Contains("Maneuver");
    
    private bool CheckIfCardIsAction()
        => _playableCard.Types.Contains("Action");
    
    private bool CheckIfCardIsReversal()
        => _playableCard.Types[0].Contains("Reversal");
}