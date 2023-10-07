using RawDealView;
using RawDealView.Formatters;

namespace RawDeal.Superstars;

public abstract class Player
{
    protected View View;
    protected Player Opponent;
    protected string Name;
    protected string Logo;
    protected int HandSize;
    protected int SuperstarValue;
    protected string SuperstarAbility;
    protected PlayerInfo PlayerInfo;
    protected List<IViewableCardInfo> Hand;
    protected List<IViewableCardInfo> Arsenal;
    protected List<IViewableCardInfo> Ringside;
    protected List<IViewableCardInfo> RingArea;
    protected Dictionary<DeckName, List<IViewableCardInfo>> Decks;
    protected Dictionary<DeckName, List<string>> FormattedDecks;
    protected PlayerDecksController PlayerDecksController;
    protected SuperstarData SuperstarData;
    private int _fortitude;

    protected Player(SuperstarData superstarData)
    {
        SuperstarData = superstarData;
        Name = superstarData.Name;
        Logo = superstarData.Logo;
        HandSize = superstarData.HandSize;
        SuperstarValue = superstarData.SuperstarValue;
        SuperstarAbility = superstarData.SuperstarAbility;
    }

    public abstract bool PlaySpecialAbility();
    
    public virtual bool VerifyAbilityUsability()
    {
        return true;
    }

    public virtual void ChangeAbilitySelectionVisibility()
    {
    }

    public void ShowOptionsToViewDecks()
    {
        var playerDecks = BuildFormattedDecks();
        var opponentDecks = Opponent.BuildFormattedDecks();
        var optionsToViewDeck = new OptionsToViewDeck(playerDecks, opponentDecks, View);
        optionsToViewDeck.SelectWhatDeckToView();
    }
    
    private Dictionary<DeckName, List<string>> BuildFormattedDecks()
    {
        FormattedDecks = new Dictionary<DeckName, List<string>>
        {
            { DeckName.Hand, FormatDeck(Hand) },
            { DeckName.Ringside, FormatDeck(Ringside) },
            { DeckName.RingArea, FormatDeck(RingArea) }
        };
        return FormattedDecks;
    }

    protected List<string> FormatDeck(List<IViewableCardInfo> deck)
    {
        return Utils.FormatDecksOfCards(deck);
    }

    public void SayThatPlayerWillTakeDamage(int damageDone)
    {
        View.SayThatSuperstarWillTakeSomeDamage(Name, damageDone);
    }

    public int AskForCardsToDiscard(List<string> formattedCards, int numberOfCardsToDiscard)
    {
        return View.AskPlayerToSelectACardToDiscard(formattedCards, Name, Name, numberOfCardsToDiscard);
    }
    public void ApplyNecessaryAttributes(View view, Player opponent)
    {
        View = view;
        Opponent = opponent;
    }
    
    public string GetLogo()
    {
        return Logo;
    }
    public bool CheckForEmptyArsenal()
    {
        return Arsenal.Count == 0;
    }
    public PlayerInfo GetPlayerInfo()
    {
        return PlayerInfo;
    }
 
    public int GetSuperstarValue()
    {
        return SuperstarValue;
    }
    public List<IViewableCardInfo> CheckForPlayableCards()
    {
        List<IViewableCardInfo> playableCards = new();
        foreach (var card in Hand)
        {
            int fortitudeInt = Convert.ToInt32(card.Fortitude);
            if (fortitudeInt <= _fortitude)
            {
                playableCards.Add(card);
            }
        }

        return playableCards;
    }

    public void InitializeNecessaryAttributes(List<IViewableCardInfo> arsenalDeck)
    {
        BuildDecksDictionary(arsenalDeck);
        
        Arsenal = arsenalDeck;
        Hand = new List<IViewableCardInfo>();
        Ringside = new List<IViewableCardInfo>();
        RingArea = new List<IViewableCardInfo>();
    }

    private void BuildDecksDictionary(List<IViewableCardInfo> arsenalDeck)
    {
        Decks = new Dictionary<DeckName, List<IViewableCardInfo>>
        {
            { DeckName.Hand, new List<IViewableCardInfo>() },
            { DeckName.Arsenal, arsenalDeck },
            { DeckName.Ringside, new List<IViewableCardInfo>() },
            { DeckName.RingArea, new List<IViewableCardInfo>() }
        };
    }

    public virtual PlayerDecksController BuildPlayerDecksController()
    {
        PlayerDecksController = new PlayerDecksController(Decks, HandSize);
        return PlayerDecksController;
    }
    
    public PlayerInfo BuildPlayerInfo()
    {
        var playerInfo = new PlayerInfo(Name, _fortitude, Decks[DeckName.Hand].Count, Decks[DeckName.Arsenal].Count);
        return playerInfo;
    }
    
    public virtual void DrawCardsInTheBeginning()
    {
        for (int cardIndex = 0; cardIndex < HandSize; cardIndex++)
            DrawCard();
        UpdatePlayerInfo();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
    
    public bool CompareNames(string opponentName)
    {
        return Name == opponentName;
    }

    public void NotifyThatPlayerWon()
    {
        View.CongratulateWinner(Name);
    }
    
    public void SayPlayerTurnBegin()
    {
        View.SayThatATurnBegins(Name);
    }
    public void TryToPlayCard(string selectedCard)
    {
        View.SayThatPlayerIsTryingToPlayThisCard(Name, selectedCard);
    }

    public bool CheckIfPlayerIsMankind()
    {
        return Name == "MANKIND";
    }
    public List<string> FormatHand()
    {
        return Utils.FormatDecksOfCards(Hand);
    }
    public void AttackTheOpponent(int damageDone)
    {
        int fortitudeToAdd = damageDone;
        if (Opponent.CheckIfPlayerIsMankind())
            damageDone--;
        
        View.SayThatSuperstarWillTakeSomeDamage(Opponent.Name, damageDone);
        AddDamageToFortitude(fortitudeToAdd);
    }

    protected void PassCardFromADeckToTheBackOfTheArsenal(List<IViewableCardInfo> fromDeck, int cardIndex)
    {
        IViewableCardInfo selectedCard = fromDeck[cardIndex]; 
        fromDeck.RemoveAt(cardIndex);
        Arsenal.Insert(0, selectedCard);
        UpdatePlayerInfo();
    }
    public IViewableCardInfo PassCardFromArsenalToRingside()
    {
        int arsenalLength = Arsenal.Count;
        IViewableCardInfo selectedCard = Arsenal[arsenalLength - 1];
        Arsenal.RemoveAt(arsenalLength - 1);
        Ringside.Add(selectedCard);
        UpdatePlayerInfo();
        return selectedCard;
    }
    
    public void PassCardFromHandToRingArea(IViewableCardInfo selectedCard)
    {
        Hand.Remove(selectedCard);
        RingArea.Add(selectedCard);
        UpdatePlayerInfo();
    }
    
    public void PassCardFromHandToRingside(int cardIndex)
    {
        IViewableCardInfo selectedCard = Hand[cardIndex]; 
        Ringside.Add(selectedCard);
        Hand.RemoveAt(cardIndex);
        UpdatePlayerInfo();
    }

    protected void PassCardFromRingsideToHand(int cardIndex)
    {
        IViewableCardInfo selectedCard = Ringside[cardIndex]; 
        Ringside.RemoveAt(cardIndex);
        Hand.Add(selectedCard);
        UpdatePlayerInfo();
    }

    protected void UpdatePlayerInfo()
    {
        PlayerInfo = new PlayerInfo(Name, _fortitude, Hand.Count, Arsenal.Count);
    }
    private void AddDamageToFortitude(int damageTaken)
    {
        _fortitude += damageTaken;
        UpdatePlayerInfo();
    }
    public virtual void DrawCard()
    {
        int lastCardOfTheArsenal = Arsenal.Count - 1;
        if (lastCardOfTheArsenal >= 0)
        {
            Hand.Add(Arsenal[lastCardOfTheArsenal]);
            Arsenal.RemoveAt(lastCardOfTheArsenal);
        }

        UpdatePlayerInfo();
    }
}
    