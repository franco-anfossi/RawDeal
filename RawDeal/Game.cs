using RawDeal.Deserializers;
using RawDeal.Superstars;
using RawDealView;
using RawDealView.Options;

namespace RawDeal;

public class Game
{
    private View _view;
    private string _deckFolder;
    private CardsSet _cardsSet;
    private List<Player> _players = new();
    
    private bool _principalLoopState = true;
    private bool _electionsLoopState = true;
    
    private int _inTurnPlayerIndex;
    private int _opponentPlayerIndex = 1;

    private Player _inTurnPlayer;
    private Player _opponentPlayer;
    
    
    public Game(View view, string deckFolder)
    {
        SuperstarDeserializer superstarDeserializer = new SuperstarDeserializer();
        CardDeserializer cardDeserializer = new CardDeserializer();
        
        var deserializedSuperstars = superstarDeserializer.DeserializeSuperstars();
        var deserializedCards = cardDeserializer.DeserializeCards();
        
        _cardsSet = new CardsSet(deserializedCards, deserializedSuperstars);
        _view = view;
        _deckFolder = deckFolder;
    }

    public void Play()
    {
        SelectDeck();
        if (_principalLoopState)
        {
            SelectFirstPlayer();
            DrawInitialCards();
        }
        RunPrincipalGameLoop();
    }
    private void SelectDeck()
    {
        for (int playerIndex = 0; playerIndex < 2; playerIndex++)
        {
            string[] openedDeckFromArchive = OpenDeckFromSelectedArchive();
            Deck newDeck = new Deck(openedDeckFromArchive, _cardsSet);
            playerIndex = ValidateDeck(newDeck, playerIndex);
        }
    }
    private void SelectFirstPlayer()
    {
        if (!(_players[0].GetSuperstarValue() >= _players[1].GetSuperstarValue()))
            Utils.ChangePositionsOfTheList(_players);
    }
    private void DrawInitialCards()
    {
        foreach (var player in _players)
            player.DrawCardsInTheBeginning();
    }
    
    private void RunPrincipalGameLoop()
    {
        while (_principalLoopState)
        {
            _electionsLoopState = true;
            InitializePlayerVariables();
            AddNecessarySuperstarAttributes();
            _inTurnPlayer.SayPlayerTurnBegin();
            PlaySpecialAbilityBeforeDrawingACard();
            _inTurnPlayer.ChangeAbilitySelectionVisibility();
            RunGameElectionsLoop();
        }
    }
    
    private string[] OpenDeckFromSelectedArchive()
    {
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        var openedDeckFromArchive = Utils.OpenDeckArchive(deckPath);
        return openedDeckFromArchive;
    }
    private int ValidateDeck(Deck deck, int playerIndex)
    {
        DeckValidator deckValidator = new DeckValidator(deck, _cardsSet);
        if (!deckValidator.ValidateDeckRules())
            playerIndex = ManageInvalidDeck();
        else
            playerIndex = ManageValidDeck(deck, playerIndex);
        return playerIndex;
    }
    private void PlaySpecialAbilityBeforeDrawingACard()
    {
        bool drawCardState = true;

        if (_inTurnPlayer.VerifyAbilityUsability())
            drawCardState = _inTurnPlayer.PlaySpecialAbility();

        if (drawCardState)
            _inTurnPlayer.DrawCard();
    }
    private void RunGameElectionsLoop()
    {
        while (_electionsLoopState)
        {
            _view.ShowGameInfo(_inTurnPlayer.GetPlayerInfo(), _opponentPlayer.GetPlayerInfo());
            NextPlay firstOptionChoice = ShowAppropriateOptionsSelector();
            ManagePossibleOptions(firstOptionChoice);
        }
    }

    private void InitializePlayerVariables()
    {
        _inTurnPlayer = _players[_inTurnPlayerIndex];
        _opponentPlayer = _players[_opponentPlayerIndex];
    }
    private void AddNecessarySuperstarAttributes()
    {
        _inTurnPlayer.ApplyNecessaryAttributes(_view, _opponentPlayer);
        _opponentPlayer.ApplyNecessaryAttributes(_view, _inTurnPlayer);
    }
    
    private NextPlay ShowAppropriateOptionsSelector()
    {
        NextPlay firstOptionChoice;
        if (_inTurnPlayer.VerifyAbilityUsability())
            firstOptionChoice = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
        else
            firstOptionChoice = _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible(); 
        return firstOptionChoice;
    }
    
    private int ManageValidDeck(Deck deck, int playerIndex)
    {
        _players.Add(deck.PlayerDeckOwner);
        return playerIndex;
    }

    private int ManageInvalidDeck()
    {
        _view.SayThatDeckIsInvalid();
        _principalLoopState = false;
        int numberOutOfIndex = 2;
        return numberOutOfIndex;
    }

    private void ManagePossibleOptions(NextPlay firstOptionChoice)
    {
        if (firstOptionChoice == NextPlay.ShowCards)
            SelectShowCardsOption();
            
        else if (firstOptionChoice == NextPlay.PlayCard)
            SelectPlayCardOption();
            
        else if (firstOptionChoice == NextPlay.UseAbility)
            SelectPlayAbilityOption();
            
        else if (firstOptionChoice == NextPlay.EndTurn)
            SelectEndTurnOption();
            
        else if (firstOptionChoice == NextPlay.GiveUp)
            SelectGiveUpOption();
    }
    
    private void SelectShowCardsOption()
    {
        var optionsToViewDeck = new OptionsToViewDeck(_inTurnPlayer, _opponentPlayer, _view);
        optionsToViewDeck.SelectWhatDeckToView();
    }
    private void SelectPlayCardOption()
    {
        var optionsToPlayCard = new OptionsToPlayCard(_inTurnPlayer, _opponentPlayer, _view);
        _electionsLoopState = optionsToPlayCard.StartElectionProcess();
        if (!_electionsLoopState)
            DeclareVictoryOfThePlayerInTurn();
    }

    private void SelectPlayAbilityOption()
    {
        _inTurnPlayer.PlaySpecialAbility();
    }

    private void SelectEndTurnOption()
    {
        if (_opponentPlayer.CheckForEmptyArsenal())
            DeclareVictoryOfThePlayerInTurn();
        
        else if (_inTurnPlayer.CheckForEmptyArsenal())
            DeclareLossOfThePlayerInTurn();
        
        else
            ChangePlayersPositions();
    }

    private void SelectGiveUpOption()
    {
        _opponentPlayer.NotifyThatPlayerWon();
        _principalLoopState = false; 
        _electionsLoopState = false;
    }

    private void DeclareVictoryOfThePlayerInTurn()
    {
        _electionsLoopState = false;
        _inTurnPlayer.NotifyThatPlayerWon();
        _principalLoopState = _electionsLoopState;
    }

    private void DeclareLossOfThePlayerInTurn()
    {
        _electionsLoopState = false;
        _opponentPlayer.NotifyThatPlayerWon();
        _principalLoopState = _electionsLoopState;
    }

    private void ChangePlayersPositions()
    {
        ChangePlayersIndex();
        _inTurnPlayer = _players[_inTurnPlayerIndex];
        _opponentPlayer = _players[_opponentPlayerIndex];
        _electionsLoopState = false;
    }
    
    private void ChangePlayersIndex()
    {
        if (_inTurnPlayerIndex == 0)
        {
            _inTurnPlayerIndex = 1;
            _opponentPlayerIndex = 0;
        }
        else
        {
            _inTurnPlayerIndex = 0;
            _opponentPlayerIndex = 1;
        }
    }
}