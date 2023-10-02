using RawDealView;
using RawDealView.Options;
using RawDealView.Formatters;

namespace RawDeal;

public class OptionsToViewDeck
{
    private List<Player> _players;
    private int _inTurnPlayerIndex;
    private int _opponentPlayerIndex;
    private View _view;

    private Dictionary<int, List<List<IViewableCardInfo>>> _notFormattedDecksOfPlayers = new();
    private Dictionary<int, List<List<string>>> _formattedDecksOfPlayers = new();

    private List<string> _inTurnPlayerHand;
    private List<string> _inTurnPlayerRingside;
    private List<string> _inTurnPlayerRingArea;

    private List<string> _opponentPlayerRingside;
    private List<string> _opponentPlayerRingArea;

    public OptionsToViewDeck(List<Player> players, int inTurnPlayerIndex, int opponentPlayerIndex, View view)
    {
        _players = players;
        _inTurnPlayerIndex = inTurnPlayerIndex;
        _opponentPlayerIndex = opponentPlayerIndex;
        _view = view;

        GetDecksFromPlayers();
        FormatDecks();
        ArrangeDeckAttributes();
    }

    private void GetDecksFromPlayers()
    {
        for (int playerIndex = 0; playerIndex < 2; playerIndex++)
        {
            List<List<IViewableCardInfo>> playerDecks = GetPlayerDecks(playerIndex);
            _notFormattedDecksOfPlayers[playerIndex] = playerDecks;
        }
    }

    public void SelectWhatDeckToView()
    {
        var selectedDeckToView = _view.AskUserWhatSetOfCardsHeWantsToSee();
        if (selectedDeckToView == CardSet.Hand)
            _view.ShowCards(_inTurnPlayerHand);
        else if (selectedDeckToView == CardSet.RingsidePile)
            _view.ShowCards(_inTurnPlayerRingside);
        else if (selectedDeckToView == CardSet.RingArea)
            _view.ShowCards(_inTurnPlayerRingArea);
        else if (selectedDeckToView == CardSet.OpponentsRingsidePile)
            _view.ShowCards(_opponentPlayerRingside);
        else if (selectedDeckToView == CardSet.OpponentsRingArea)
            _view.ShowCards(_opponentPlayerRingArea);
    }

    private void ArrangeDeckAttributes()
    {
        _inTurnPlayerHand = _formattedDecksOfPlayers[_inTurnPlayerIndex][0];
        _inTurnPlayerRingside = _formattedDecksOfPlayers[_inTurnPlayerIndex][1];
        _inTurnPlayerRingArea = _formattedDecksOfPlayers[_inTurnPlayerIndex][2];

        _opponentPlayerRingside = _formattedDecksOfPlayers[_opponentPlayerIndex][1];
        _opponentPlayerRingArea = _formattedDecksOfPlayers[_opponentPlayerIndex][2];
    }

    private void FormatDecks()
    {
        int dictionaryKey = 0;
        foreach (var playerDecks in _notFormattedDecksOfPlayers)
        {
            List<List<string>> formattedDeck = FormatSpecificDeck(playerDecks.Value);
            _formattedDecksOfPlayers[dictionaryKey] = formattedDeck;
            dictionaryKey++;
        }
    }

    private List<List<string>> FormatSpecificDeck(List<List<IViewableCardInfo>> playersDeck)
    {
        List<List<string>> formattedDeck = new List<List<string>>();
        foreach (var deck in playersDeck)
        {
            List<string> formattedCardData = Utils.FormatDecksOfCards(deck);
            formattedDeck.Add(formattedCardData);
        }

        return formattedDeck;
    }

    private List<List<IViewableCardInfo>> GetPlayerDecks(int playerIndex)
    {
        List<List<IViewableCardInfo>> playerDecks = new List<List<IViewableCardInfo>>();

        List<IViewableCardInfo> playerHand = _players[playerIndex].GetHand();
        List<IViewableCardInfo> playerRingside = _players[playerIndex].GetRingside();
        List<IViewableCardInfo> playerRingArea = _players[playerIndex].GetRingArea();

        playerDecks.Add(playerHand);
        playerDecks.Add(playerRingside);
        playerDecks.Add(playerRingArea);

        return playerDecks;
    }
}