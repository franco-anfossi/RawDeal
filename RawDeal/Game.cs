using RawDeal.Decks;
using RawDeal.Deserializers;
using RawDeal.Exceptions;
using RawDeal.Superstars;
using RawDealView;
using RawDealView.Options;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;
    private readonly CardsSet _cardsSet;
    private readonly List<Player> _players = new();
    
    private bool _principalLoopState = true;
    private bool _electionsLoopState = true;
    
    private int _inTurnPlayerIndex;
    private int _opponentPlayerIndex = 1;
    
    private Player _inTurnPlayer;
    private Player _opponentPlayer;
    
    private PlayerDecksController _inTurnPlayerDecksController;
    private PlayerDecksController _opponentPlayerDecksController;
    
    
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
        try
        {
            TryToPlayGame();
        }
        catch (InvalidDeckException)
        {
            _view.SayThatDeckIsInvalid();
        }
    }

    private void TryToPlayGame()
    {
        SelectDeck();
        SelectFirstPlayer();
        InitializePlayerVariables();
        InitializePlayersDecksControllers();
        AddNecessarySuperstarAttributes();
        DrawInitialCards();
        RunPrincipalGameLoop();
    }
    
    private void SelectDeck()
    {
        for (int playerIndex = 0; playerIndex < 2; playerIndex++)
        {
            string[] openedDeckFromArchive = OpenDeckFromSelectedArchive();
            Deck newDeck = new Deck(openedDeckFromArchive, _cardsSet);
            ValidateDeck(newDeck);
        }
    }
    
    private void SelectFirstPlayer()
    {
        if (!(_players[0].GetSuperstarValue() >= _players[1].GetSuperstarValue()))
            Utils.ChangePositionsOfTheList(_players);
    }
    
    private void InitializePlayerVariables()
    {
        _inTurnPlayer = _players[_inTurnPlayerIndex];
        _opponentPlayer = _players[_opponentPlayerIndex];
    }
    
    private void InitializePlayersDecksControllers()
    {
        _inTurnPlayerDecksController = _inTurnPlayer.BuildPlayerDecksController();
        _opponentPlayerDecksController = _opponentPlayer.BuildPlayerDecksController();
    }
    
    private void ChangePlayersDecksControllers()
    {
        var newInTurnPlayerDecksController = _inTurnPlayerDecksController;
        var newOpponentPlayerDecksController = _opponentPlayerDecksController;
        
        _inTurnPlayerDecksController = newOpponentPlayerDecksController;
        _opponentPlayerDecksController = newInTurnPlayerDecksController;
    }
    
    private void DrawInitialCards()
    {
        _inTurnPlayerDecksController.DrawCardsInTheBeginning();
        _opponentPlayerDecksController.DrawCardsInTheBeginning();
    }
    
    private void RunPrincipalGameLoop()
    {
        while (_principalLoopState)
        {
            _electionsLoopState = true;
            _inTurnPlayer.SayPlayerTurnBegin();
            PlaySpecialAbilityBeforeDrawingACard();
            _inTurnPlayer.ChangeAbilitySelectionVisibility();
            RunGameElectionsLoop();
        }
    }

    private PlayerInfoManager UpdatePlayersInfo()
    {
        PlayerInfo inTurnPlayerInfo = _inTurnPlayer.BuildPlayerInfo();
        PlayerInfo opponentPlayerInfo = _opponentPlayer.BuildPlayerInfo();
        return new PlayerInfoManager(inTurnPlayerInfo, opponentPlayerInfo, _view);
    }
    
    private string[] OpenDeckFromSelectedArchive()
    {
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        var openedDeckFromArchive = Utils.OpenDeckArchive(deckPath);
        return openedDeckFromArchive;
    }
    private void ValidateDeck(Deck deck)
    {
        DeckValidator deckValidator = new DeckValidator(deck, _cardsSet);
        if (deckValidator.ValidateDeckRules())
            _players.Add(deck.PlayerDeckOwner);
        else
            throw new InvalidDeckException();
    }
    private void PlaySpecialAbilityBeforeDrawingACard()
    {
        bool drawCardState = true;
        if (_inTurnPlayer.VerifyAbilityUsability())
            drawCardState = _inTurnPlayer.PlaySpecialAbility();

        if (drawCardState)
            _inTurnPlayerDecksController.DrawTurnCard();
    }
    private void RunGameElectionsLoop()
    {
        while (_electionsLoopState)
        {
            var playerInfoManager = UpdatePlayersInfo();
            playerInfoManager.ShowPlayerInfo();
            NextPlay firstOptionChoice = ShowAppropriateOptionsSelector();
            ManagePossibleOptions(firstOptionChoice);
        }
    }

    
    private void AddNecessarySuperstarAttributes()
    {
        var opponentPlayerInfo = _opponentPlayer.BuildImportantPlayerData();
        var inTurnPlayerInfo = _inTurnPlayer.BuildImportantPlayerData();
        
        _inTurnPlayer.ApplyNecessaryAttributes(_view, opponentPlayerInfo);
        _opponentPlayer.ApplyNecessaryAttributes(_view, inTurnPlayerInfo);
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

    private void ManagePossibleOptions(NextPlay firstOptionChoice)
    {
        if (firstOptionChoice == NextPlay.ShowCards)
            SelectShowCardsOption();
            
        else if (firstOptionChoice == NextPlay.PlayCard)
            TryToSelectPlayCardOption();
            
        else if (firstOptionChoice == NextPlay.UseAbility)
            SelectPlayAbilityOption();
            
        else if (firstOptionChoice == NextPlay.EndTurn)
        {
            ResetPlayersChangesByJockeyingForPosition();
            SelectEndTurnOption();
        }
            
        else if (firstOptionChoice == NextPlay.GiveUp)
            SelectGiveUpOption();
    }
    
    private void SelectShowCardsOption()
    {
        _inTurnPlayer.ShowOptionsToViewDecks();
    }
    private void TryToSelectPlayCardOption()
    {
        try
        {
            SelectPlayCardOption();
        }
        catch (NoArsenalCardsException)
        {
            SelectEndTurnOption();
        }
        catch (EndOfTurnException)
        {
            SelectEndTurnOption();
        }
    }

    private void SelectPlayCardOption()
    {
        var inTurnPlayerInfo = _inTurnPlayer.BuildImportantPlayerData();
        var opponentPlayerInfo = _opponentPlayer.BuildImportantPlayerData();
        
        var optionsToPlayCard = new OptionsToPlayCard(inTurnPlayerInfo, opponentPlayerInfo, _view);
        optionsToPlayCard.StartElectionProcess();
    }

    private void SelectPlayAbilityOption()
    {
        _inTurnPlayer.PlaySpecialAbility();
    }

    private void SelectEndTurnOption()
    {
        if (_opponentPlayerDecksController.CheckForEmptyArsenal())
            DeclareVictoryOfThePlayerInTurn();
        
        else if (_inTurnPlayerDecksController.CheckForEmptyArsenal())
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
        ChangePlayersDecksControllers();
        _inTurnPlayer = _players[_inTurnPlayerIndex];
        _opponentPlayer = _players[_opponentPlayerIndex];
        _electionsLoopState = false;
    }
    
    private void ResetPlayersChangesByJockeyingForPosition()
    {
        _inTurnPlayer.ResetChangesByJockeyingForPosition();
        _opponentPlayer.ResetChangesByJockeyingForPosition();
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