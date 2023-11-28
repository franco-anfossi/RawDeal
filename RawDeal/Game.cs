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
    {
        return _gameDeckManager.SelectDeck(new BoundaryList<Player>());
    }
    
    private void InitializeGamePlayerManager(BoundaryList<Player> players)
    {
        _gamePlayerManager = new GamePlayerManager(players, _view);
    }
    
    private void TryToRunPrincipalGameLoop()
    {
        try
        {
            RunPrincipalGameLoop();
        }
        catch (EndOfPrincipalLoop winnerName)
        {
            _view.CongratulateWinner(winnerName.Message);
        }
    }

    private void RunPrincipalGameLoop()
    {
        var continuePrincipalGameLoop = true;
        while (continuePrincipalGameLoop)
        {
            _gamePlayerManager.SayThatSuperstarStartsTurn();
            _gamePlayerManager.PlaySpecialAbilityBeforeDrawingACard();
            _gamePlayerManager.DrawCard();
            _gamePlayerManager.ResetAbility();
            TryToRunGameElectionsLoop();
        }
    }

    private void TryToRunGameElectionsLoop()
    {
        try
        {
            RunGameElectionsLoop();
        }
        catch (EndOfElectionLoop)
        {
            _gamePlayerManager.ChangePlayersPositions();
        }
    }

    private void RunGameElectionsLoop()
    {
        while (true)
        {
            var playerInfoManager = _gamePlayerManager.UpdatePlayersInfo();
            playerInfoManager.ShowPlayerInfo();
            NextPlay firstOptionChoice = _gamePlayerManager.ShowAppropriateOptionsSelector();
            ManagePossibleOptions(firstOptionChoice);
        }
    }
    
    private void ManagePossibleOptions(NextPlay firstOptionChoice)
    {
        if (firstOptionChoice == NextPlay.ShowCards)
            _gamePlayerManager.SelectShowCardsOption();
            
        else if (firstOptionChoice == NextPlay.PlayCard)
            TryToSelectPlayCardOption();
            
        else if (firstOptionChoice == NextPlay.UseAbility)
            _gamePlayerManager.SelectPlayAbilityOption();
        
        // TODO: Make this else if only one method call
        else if (firstOptionChoice == NextPlay.EndTurn)
        {
            _gamePlayerManager.ResetPlayersChangesByJockeyingForPosition();
            CheckEndTurnCondition();
        }
            
        else if (firstOptionChoice == NextPlay.GiveUp)
            _gamePlayerManager.GiveUp();
    }
    
    private void TryToSelectPlayCardOption()
    {
        // TODO: Make only one catch
        try
        {
            _gamePlayerManager.SelectPlayCardOption();
        }
        catch (NoArsenalCardsException winnerName)
        {
            throw new EndOfPrincipalLoop(winnerName.Message);
        }
        catch (EndOfTurnException)
        {
            CheckEndTurnCondition();
        }
    }
    
    private void CheckEndTurnCondition()
    {
        _gamePlayerManager.CheckOpponentLoss();
        _gamePlayerManager.CheckPlayerInTurnLoss();
        throw new EndOfElectionLoop();
    }
}