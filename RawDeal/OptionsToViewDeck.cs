using RawDealView;
using RawDealView.Options;

namespace RawDeal;

public class OptionsToViewDeck
{
    private readonly Dictionary<DeckName, List<string>> _playerDecks;
    private readonly Dictionary<DeckName, List<string>> _opponentDecks;
    private readonly View _view;

    public OptionsToViewDeck(Dictionary<DeckName, List<string>> playerDecks, Dictionary<DeckName, List<string>> opponentDecks, View view)
    {
        _playerDecks = playerDecks;
        _opponentDecks = opponentDecks;
        _view = view;
    }

    public void SelectWhatDeckToView()
    {
        var selectedDeckToView = _view.AskUserWhatSetOfCardsHeWantsToSee();
        if (selectedDeckToView == CardSet.Hand)
            _view.ShowCards(_playerDecks[DeckName.Hand]);
        
        else if (selectedDeckToView == CardSet.RingsidePile)
            _view.ShowCards(_playerDecks[DeckName.Ringside]);
        
        else if (selectedDeckToView == CardSet.RingArea)
            _view.ShowCards(_playerDecks[DeckName.RingArea]);
        
        else if (selectedDeckToView == CardSet.OpponentsRingsidePile)
            _view.ShowCards(_opponentDecks[DeckName.Ringside]);
        
        else if (selectedDeckToView == CardSet.OpponentsRingArea)
            _view.ShowCards(_opponentDecks[DeckName.RingArea]);
    }
}