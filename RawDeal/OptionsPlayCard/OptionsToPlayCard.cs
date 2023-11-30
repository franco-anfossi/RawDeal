using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.OptionsPlayCard;

public class OptionsToPlayCard
{
    private readonly PlaySelector _playSelector;
    private readonly CardControllerBuilder _cardControllerBuilder;
    private readonly PlayExecutor _playExecutor;
    private readonly ImportantPlayerData _playerData;
    private LastCardUsed _lastCardUsed;

    public OptionsToPlayCard(ImportantPlayerData playerData, 
        ImportantPlayerData opponentData, LastCardUsed lastCardUsed, View view)
    {
        _playSelector = new PlaySelector(view);
        _cardControllerBuilder = new CardControllerBuilder(playerData, opponentData, view);
        _playExecutor = new PlayExecutor(view);
        _playerData = playerData;
        _lastCardUsed = lastCardUsed;
    }

    public LastCardUsed StartElectionProcess()
    {
        var possiblePlaysData = _playSelector.BuildPlayablePlays(_playerData, _lastCardUsed);
        int selectedPlayNumber = _playSelector.SelectAPlay(possiblePlaysData.FormattedPlays);

        if (CheckIfHasPlayablePlays(possiblePlaysData.FormattedPlays) && CheckIfCardIsSelected(selectedPlayNumber))
            ExecuteSelectedPlay(possiblePlaysData, selectedPlayNumber);
        
        return _lastCardUsed;
    }
    
    private void ExecuteSelectedPlay(PossiblePlaysData possiblePlaysData, int selectedPlayNumber)
    {
        string formattedSelectedPlay = possiblePlaysData.FormattedPlays[selectedPlayNumber];
        IViewablePlayInfo notFormattedSelectedPlay = possiblePlaysData.NotFormattedPlays[selectedPlayNumber];
        var cardController = _cardControllerBuilder.Build(notFormattedSelectedPlay);
        var selectedPlay = (notFormattedSelectedPlay, formattedSelectedPlay);
        _lastCardUsed = _playExecutor.ExecutePlay(_playerData.Name, selectedPlay, cardController);
    }

    private bool CheckIfHasPlayablePlays(BoundaryList<string> formattedPlayablePlays)
    {
        return formattedPlayablePlays.Count > 0;
    }

    private bool CheckIfCardIsSelected(int selectedPlayNumber)
    {
        return selectedPlayNumber != -1;
    }
}
