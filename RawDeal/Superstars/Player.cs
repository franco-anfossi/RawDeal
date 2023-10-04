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
    private int _fortitude;

    protected Player(SuperstarData superstarData)
    {
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

    public void InitializeNecessaryAttributes(List<IViewableCardInfo> deck)
    {
        Arsenal = deck;
        Hand = new List<IViewableCardInfo>();
        Ringside = new List<IViewableCardInfo>();
        RingArea = new List<IViewableCardInfo>();
        _fortitude = 0;
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

    public void ShowFormattedHand()
    {
        View.ShowCards(FormatHand());
    }
    
    public void ShowFormattedRingside()
    {
        View.ShowCards(FormatRingside());
    }
    
    public void ShowFormattedRingArea()
    {
        View.ShowCards(FormatRingArea());
    }

    public List<string> FormatHand()
    {
        return Utils.FormatDecksOfCards(Hand);
    }

    private List<string> FormatRingside()
    {
        return Utils.FormatDecksOfCards(Ringside);
    }

    private List<string> FormatRingArea()
    {
        return Utils.FormatDecksOfCards(RingArea);
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
    