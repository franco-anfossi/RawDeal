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

    public OptionsToPlayCard(ImportantPlayerData playerData, ImportantPlayerData opponentData, View view)
    {
        _playSelector = new PlaySelector(view);
        _cardControllerBuilder = new CardControllerBuilder(playerData, opponentData, view);
        _playExecutor = new PlayExecutor(view);
        _playerData = playerData;
    }

    public void StartElectionProcess()
    {
        var possiblePlaysData = _playSelector.GetPlayablePlays(_playerData);
        int selectedPlayNumber = _playSelector.GetSelectedPlay(possiblePlaysData.FormattedPlays);

        if (CheckIfHasPlayablePlays(possiblePlaysData.FormattedPlays) && CheckIfCardIsSelected(selectedPlayNumber))
            ExecuteSelectedPlay(possiblePlaysData, selectedPlayNumber);
    }
    
    private void ExecuteSelectedPlay(PossiblePlaysData possiblePlaysData, int selectedPlayNumber)
    {
        string formattedSelectedPlay = possiblePlaysData.FormattedPlays[selectedPlayNumber];
        IViewablePlayInfo notFormattedSelectedPlay = possiblePlaysData.NotFormattedPlays[selectedPlayNumber];
        var cardController = _cardControllerBuilder.Build(notFormattedSelectedPlay);
        _playExecutor.ExecutePlay(_playerData.Name, formattedSelectedPlay, cardController);
    }

    private bool CheckIfHasPlayablePlays(List<string> formattedPlayablePlays)
    {
        return formattedPlayablePlays.Count > 0;
    }

    private bool CheckIfCardIsSelected(int selectedPlayNumber)
    {
        return selectedPlayNumber != -1;
    }
}
