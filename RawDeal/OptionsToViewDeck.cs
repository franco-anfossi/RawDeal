using RawDealView;
using RawDealView.Options;

namespace RawDeal;

public class OptionsToViewDeck
{
    private Player _inTurnPlayer;
    private Player _opponentPlayer;
    private View _view;

    public OptionsToViewDeck(Player inTurnPlayer, Player opponentPlayer, View view)
    {
        _inTurnPlayer = inTurnPlayer;
        _opponentPlayer = opponentPlayer;
        _view = view;
    }

    public void SelectWhatDeckToView()
    {
        var selectedDeckToView = _view.AskUserWhatSetOfCardsHeWantsToSee();
        if (selectedDeckToView == CardSet.Hand)
            _inTurnPlayer.ShowFormattedHand();
        else if (selectedDeckToView == CardSet.RingsidePile)
            _inTurnPlayer.ShowFormattedRingside();
        else if (selectedDeckToView == CardSet.RingArea)
            _inTurnPlayer.ShowFormattedRingArea();
        else if (selectedDeckToView == CardSet.OpponentsRingsidePile)
            _opponentPlayer.ShowFormattedRingside();
        else if (selectedDeckToView == CardSet.OpponentsRingArea)
            _opponentPlayer.ShowFormattedRingArea();
    }
}