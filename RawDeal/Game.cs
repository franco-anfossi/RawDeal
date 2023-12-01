using RawDeal.Boundaries;
using RawDeal.Data_Structures;
using RawDealView;
using RawDeal.Exceptions;
using RawDeal.Superstars;
using RawDealView.Options;
using RawDeal.Deserializers;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly GameDeckManager _gameDeckManager;
    private GamePlayerManager _gamePlayerManager;

    public Game(View view, string deckFolder)
    {
        var cardsSet = DeserializeArchives();
        
        _view = view;
        _gameDeckManager = new GameDeckManager(deckFolder, cardsSet, _view);
    }

    private CardsSet DeserializeArchives()
    {
        SuperstarDeserializer superstarDeserializer = new SuperstarDeserializer();
        CardDeserializer cardDeserializer = new CardDeserializer();
        
        var deserializedSuperstars = superstarDeserializer.DeserializeSuperstars();
        var deserializedCards = cardDeserializer.DeserializeCards();
        
        return new CardsSet(deserializedCards, deserializedSuperstars);
    }

    public void Play()
    {
        try
        {
            StartMatch();
        }
        catch (InvalidDeckException)
        {
            _view.SayThatDeckIsInvalid();
        }
    }

    private void StartMatch()
    {
        var players = SelectDeck();
        InitializeGamePlayerManager(players);
        _gamePlayerManager.InitializeNecessaryPlayerVariables();
        _gamePlayerManager.DrawInitialCards();
        TryToRunPrincipalGameLoop();
    }
    
    private BoundaryList<Player> SelectDeck()
        => _gameDeckManager.SelectDeck(new BoundaryList<Player>());
    
    private void InitializeGamePlayerManager(BoundaryList<Player> players)
        => _gamePlayerManager = new GamePlayerManager(players, _view);
    
    private void TryToRunPrincipalGameLoop()
    {
        try
        {
            StartPrincipalGameLoop();
        }
        catch (EndOfPrincipalLoop winnerName)
        {
            _view.CongratulateWinner(winnerName.Message);
        }
    }

    private void StartPrincipalGameLoop()
    {
        var continuePrincipalGameLoop = true;
        while (continuePrincipalGameLoop)
            ExecutePrincipalGameLoopMethods();
    }
    
    private void ExecutePrincipalGameLoopMethods()
    {
        _gamePlayerManager.SayThatSuperstarStartsTurn();
        _gamePlayerManager.PlaySpecialAbilityBeforeDrawingACard();
        _gamePlayerManager.DrawCard();
        _gamePlayerManager.ResetAbility();
        TryToRunGameElectionsLoop();
    }

    private void TryToRunGameElectionsLoop()
    {
        try
        {
            StartGameElectionsLoop();
        }
        catch (EndOfElectionLoop)
        {
            EndOfElectionLoop();
        }
    }
    
    private void StartGameElectionsLoop()
    {
        var continueElectionsLoop = true;
        while (continueElectionsLoop) 
            ExecuteGameElectionsLoopMethods();
    }
    
    private void EndOfElectionLoop()
    {
        _gamePlayerManager.ChangePlayersPositions();
        _gamePlayerManager.ResetLastCardUsed();
    }

    private void ExecuteGameElectionsLoopMethods()
    {
        var playerInfoManager = _gamePlayerManager.UpdatePlayersInfo();
        playerInfoManager.ShowPlayerInfo();
        NextPlay firstOptionChoice = _gamePlayerManager.ShowAppropriateOptionsSelector();
        ManagePossibleOptions(firstOptionChoice);
    }
    
    private void ManagePossibleOptions(NextPlay firstOptionChoice)
    {
        if (firstOptionChoice == NextPlay.ShowCards)
            _gamePlayerManager.SelectShowCardsOption();
            
        else if (firstOptionChoice == NextPlay.PlayCard)
            TryToSelectPlayCardOption();
            
        else if (firstOptionChoice == NextPlay.UseAbility)
            _gamePlayerManager.SelectPlayAbilityOption();
        
        else if (firstOptionChoice == NextPlay.EndTurn)
            SelectEndTurnOption();
            
        else if (firstOptionChoice == NextPlay.GiveUp)
            _gamePlayerManager.GiveUp();
    }
    
    private void TryToSelectPlayCardOption()
    {
        try
        {
            _gamePlayerManager.SelectPlayCardOption();
        }
        catch (OptionPlayCardException exception)
        {
            HandleOptionPlayCardException(exception.Message);
        }
    }
    
    private void SelectEndTurnOption()
    {
        _gamePlayerManager.ResetPlayersChangesByJockeyingForPosition();
        CheckEndTurnCondition();
    }
    
    private void HandleOptionPlayCardException(string message)
    {
        if (message == "End of turn")
            CheckEndTurnCondition();
        else
            throw new EndOfPrincipalLoop(message);
    }

    private void CheckEndTurnCondition()
    {
        _gamePlayerManager.CheckOpponentLoss();
        _gamePlayerManager.CheckPlayerInTurnLoss();
        throw new EndOfElectionLoop();
    }
}