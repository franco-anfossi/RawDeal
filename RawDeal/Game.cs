using RawDealView;
using RawDeal.Decks;
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
            TryToPlayGame();
        }
        catch (InvalidDeckException)
        {
            _view.SayThatDeckIsInvalid();
        }
    }

    private void TryToPlayGame()
    {
        var players = SelectDeck();
        InitializeGamePlayerManager(players);
        _gamePlayerManager.InitializeNecessaryPlayerVariables();
        _gamePlayerManager.DrawInitialCards();
        TryToRunPrincipalGameLoop();
    }
    
    private List<Player> SelectDeck()
    {
        return _gameDeckManager.SelectDeck(new List<Player>());
    }
    
    private void InitializeGamePlayerManager(List<Player> players)
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
        while (true)
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
            
        else if (firstOptionChoice == NextPlay.EndTurn)
        {
            _gamePlayerManager.ResetPlayersChangesByJockeyingForPosition();
            SelectEndTurnOption();
        }
            
        else if (firstOptionChoice == NextPlay.GiveUp)
            _gamePlayerManager.GiveUp();
    }
    
    private void TryToSelectPlayCardOption()
    {
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
            SelectEndTurnOption();
        }
    }
    
    private void SelectEndTurnOption()
    {
        _gamePlayerManager.CheckOpponentLoss();
        _gamePlayerManager.CheckPlayerInTurnLoss();
        throw new EndOfElectionLoop();
    }
}