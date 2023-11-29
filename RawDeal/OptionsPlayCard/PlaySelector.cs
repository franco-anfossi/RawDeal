using RawDeal.Boundaries;
using RawDeal.Cards.Builders;
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

    public PossiblePlaysData BuildPlayablePlays(ImportantPlayerData playerData)
    {
        var playableCardManager = new PlayableCardsManager(playerData);
        return playableCardManager.BuildPlayablePlays();
    }

    public int SelectAPlay(BoundaryList<string> formattedPlays)
    {
        return _view.AskUserToSelectAPlay(formattedPlays.ToList());
    }
}