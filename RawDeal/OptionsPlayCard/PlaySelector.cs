using RawDeal.Data_Structures;
using RawDealView;

namespace RawDeal.OptionsPlayCard;

public class PlaySelector
{
    private readonly View _view;

    public PlaySelector(View view)
    {
        _view = view;
    }

    public PossiblePlaysData GetPlayablePlays(ImportantPlayerData playerData)
    {
        var playableCardManager = new PlayableCardsManager(playerData);
        return playableCardManager.BuildPlayablePlays();
    }

    public int GetSelectedPlay(List<string> formattedPlays)
    {
        return _view.AskUserToSelectAPlay(formattedPlays);
    }
}