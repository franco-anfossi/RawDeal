using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDeal.Decks;
using RawDeal.Exceptions;
using RawDeal.Superstars;
using RawDealView;

namespace RawDeal;

public class GameDeckManager
{
    private readonly View _view;
    private readonly string _deckFolder;
    private readonly CardsSet _cardsSet;

    public GameDeckManager(string deckFolder, CardsSet cardsSet, View view)
    {
        _view = view;
        _deckFolder = deckFolder;
        _cardsSet = cardsSet;
    }
    
    public BoundaryList<Player> SelectDeck(BoundaryList<Player> players)
    {
        for (int playerIndex = 0; playerIndex < 2; playerIndex++)
            CreateDeckFromArchive(players);

        return players;
    }
    
    private void CreateDeckFromArchive(BoundaryList<Player> players)
    {
        string[] openedDeckFromArchive = OpenDeckFromSelectedArchive();
        var newDeck = new Deck(openedDeckFromArchive, _cardsSet);
        ValidateDeck(newDeck, players);
    }

    private string[] OpenDeckFromSelectedArchive()
    {
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        return Utils.OpenDeckArchive(deckPath);
    }

    private void ValidateDeck(Deck deck, BoundaryList<Player> players)
    {
        var deckValidator = new DeckValidator(deck, _cardsSet);
        if (deckValidator.ValidateDeckRules())
            players.Add(deck.PlayerDeckOwner);
        else
            throw new InvalidDeckException();
    }
}
