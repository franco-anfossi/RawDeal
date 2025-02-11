using RawDeal.Data_Structures;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.OptionsViewDeck;

public class OptionsToViewDeck
{
    private readonly FormattedDecksInfo _playerFormattedDecks;
    private readonly FormattedDecksInfo _opponentFormattedDecks;
    private readonly View _view;

    public OptionsToViewDeck(FormattedDecksInfo playerDecks, FormattedDecksInfo opponentDecks, View view)
    {
        _view = view;
        _playerFormattedDecks = playerDecks;
        _opponentFormattedDecks = opponentDecks;
    }

    public void SelectWhatDeckToView()
    {
        var selectedDeckToView = _view.AskUserWhatSetOfCardsHeWantsToSee();
        if (selectedDeckToView == CardSet.Hand)
            _view.ShowCards(_playerFormattedDecks.Hand.ToList());
        
        else if (selectedDeckToView == CardSet.RingsidePile)
            _view.ShowCards(_playerFormattedDecks.Ringside.ToList());
        
        else if (selectedDeckToView == CardSet.RingArea)
            _view.ShowCards(_playerFormattedDecks.RingArea.ToList());
        
        else if (selectedDeckToView == CardSet.OpponentsRingsidePile)
            _view.ShowCards(_opponentFormattedDecks.Ringside.ToList());
        
        else if (selectedDeckToView == CardSet.OpponentsRingArea)
            _view.ShowCards(_opponentFormattedDecks.RingArea.ToList());
    }
}