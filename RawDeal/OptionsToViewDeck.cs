using RawDealView;
using RawDealView.Options;
using RawDealView.Formatters;

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
            _inTurnPlayer.ShowFormatedHand();
        else if (selectedDeckToView == CardSet.RingsidePile)
            _inTurnPlayer.ShowFormatedRingside();
        else if (selectedDeckToView == CardSet.RingArea)
            _inTurnPlayer.ShowFormatedRingArea();
        else if (selectedDeckToView == CardSet.OpponentsRingsidePile)
            _opponentPlayer.ShowFormatedRingside();
        else if (selectedDeckToView == CardSet.OpponentsRingArea)
            _opponentPlayer.ShowFormatedRingArea();
    }
}