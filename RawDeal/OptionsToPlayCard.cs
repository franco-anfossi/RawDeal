using RawDealView;
using RawDeal.Cards;
using RawDealView.Formatters;
using RawDeal.Data_Structures;

namespace RawDeal;

public class OptionsToPlayCard
{
    private readonly View _view;
    
    private readonly ImportantPlayerData _inTurnPlayerInfo;
    private readonly ImportantPlayerData _opponentInfo;
    
    private readonly List<IViewablePlayInfo> _notFormattedPossiblePlays = new();
    private readonly List<string> _formattedPossiblePlays = new();
    
    private IViewablePlayInfo _notFormattedSelectedPlay;
    private string _formattedSelectedPlay;
    
    private CardControllerDecider _cardControllerDecider;
    private BasicCardController _cardController;

    public OptionsToPlayCard(ImportantPlayerData playerData, ImportantPlayerData opponentData, View view)
    {
        _view = view;
        _inTurnPlayerInfo = playerData;
        _opponentInfo = opponentData;
        
        CreatePlayablePlays();
        FormatPlay();
    }

    private void CreatePlayablePlays()
    {
        var playableCards = _inTurnPlayerInfo.DecksController.CheckForPlayableCards();
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

    public void StartElectionProcess()
    {
        int selectedPlayNumber = _view.AskUserToSelectAPlay(_formattedPossiblePlays);
        if (_formattedPossiblePlays.Count > 0 && selectedPlayNumber != -1)
        {
            InitializeVariables(selectedPlayNumber);
            _view.SayThatPlayerIsTryingToPlayThisCard(_inTurnPlayerInfo.Name, _formattedSelectedPlay);
            BuildGeneralCardManager();
            BuildCorrectCardController();
            _cardController.ApplyEffect();
        }
    }

    private void BuildGeneralCardManager()
    {
        _cardControllerDecider = new CardControllerDecider(_notFormattedSelectedPlay);
    }

    private void BuildCorrectCardController()
    {
        var cardControllerType = _cardControllerDecider.DecideCardController();
        if (cardControllerType == CardControllerTypes.BasicHybridCard)
            _cardController = 
                new BasicHybridController(_inTurnPlayerInfo, _opponentInfo, _notFormattedSelectedPlay, _view);
        else
            _cardController = 
                new BasicCardController(_inTurnPlayerInfo, _opponentInfo, _notFormattedSelectedPlay, _view);
    }

    private void SavePlayablePlay(IViewableCardInfo playableCard)
    {
        if (!playableCard.Types.Contains("Reversal"))
        { 
            if (playableCard.Types.Contains("Maneuver") && playableCard.Types.Contains("Action"))
            {
                for (int typesIndex = 0; typesIndex < 2; typesIndex++)
                {
                    string cardType = playableCard.Types[typesIndex].ToUpper();
                    PlayInfo possiblePlayInfo = new PlayInfo(playableCard, cardType);
                    _notFormattedPossiblePlays.Add(possiblePlayInfo);
                }
            }
            else
            {
                string cardType = playableCard.Types[0].ToUpper();
                PlayInfo possiblePlayInfo = new PlayInfo(playableCard, cardType);
                _notFormattedPossiblePlays.Add(possiblePlayInfo);
            }
        }
    }
    private void InitializeVariables(int selectedPlayNum)
    {
        _formattedSelectedPlay = _formattedPossiblePlays[selectedPlayNum];
        _notFormattedSelectedPlay = _notFormattedPossiblePlays[selectedPlayNum];
    }
}