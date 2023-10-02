using RawDealView.Formatters;

namespace RawDeal;

public static class DeckValidator
{
    private static List<IViewableCardInfo> _equalCardsGroup;
    private static Deck _deckToReview;
    public static bool ValidateDeckRules(Deck deck, CardsSet cardsSet)
    {
        _deckToReview = deck;
        bool conditionOne = ValidateDeckLength() && ValidateCardRepetitions();
        bool conditionTwo = ValidateIfCardsAreOnlyHeelOrFace() && ValidateCardLogoEqualToSuperstarLogo(cardsSet);
        return conditionOne && conditionTwo;
    }
    
    private static bool ValidateDeckLength()
    {
        return _deckToReview.DeckCards.Count == 60;
    }

    private static bool ValidateCardRepetitions()
    {
        var cardGrouping = _deckToReview.DeckCards.GroupBy(card => card.Title);
        foreach (var cardGroupsWithSameName in cardGrouping)
        {
            _equalCardsGroup = cardGroupsWithSameName.ToList();
            int maxRepetitionsAllowed = GetMaximumRepetitionsAllowed();
            if (_equalCardsGroup.Count > maxRepetitionsAllowed)
            {
                return false;
            }
        }
        return true;
    }
    
    private static bool ValidateIfCardsAreOnlyHeelOrFace()
    {
        bool heelExists = _deckToReview.DeckCards.Any(c => c.Subtypes.Contains("Heel"));
        bool faceExists = _deckToReview.DeckCards.Any(c => c.Subtypes.Contains("Face"));
        return !(heelExists && faceExists);
    }

    private static bool ValidateCardLogoEqualToSuperstarLogo(CardsSet cardsSet)
    {
        var logoSuperstarToReview = _deckToReview.PlayerDeckOwner.GetLogo();
        foreach (var superstar in cardsSet.PossibleSuperstars)
        {
            string logoOtherSuperstar = superstar.GetLogo();
            if (!ReviewDeckCardsByLogo(logoOtherSuperstar, logoSuperstarToReview))
            {
                return false;
            }
        }
        
        return true;
    }

    private static bool ReviewDeckCardsByLogo(string logoOtherSuperstar, string logoSuperstarToReview)
    {
        if (logoOtherSuperstar != logoSuperstarToReview)
        {
            if (_deckToReview.DeckCards.Any(c => c.Subtypes.Contains(logoOtherSuperstar)))
                return false;
        }
        return true;
    }
    private static int GetMaximumRepetitionsAllowed()
    {
        int maxRepetitionsAllowed = 3;
        maxRepetitionsAllowed = ValidateUniqueRepetitions(maxRepetitionsAllowed);
        maxRepetitionsAllowed = ValidarSetUpRepetitions(maxRepetitionsAllowed);

        return maxRepetitionsAllowed;
    }
    
    private static int ValidateUniqueRepetitions(int maxRepetitionsAllowed)
    {
        if (_equalCardsGroup.Any(c => c.Subtypes.Contains("Unique"))) 
            maxRepetitionsAllowed = 1;
        return maxRepetitionsAllowed;
    }

    private static int ValidarSetUpRepetitions(int maxRepetitionsAllowed)
    {
        if (_equalCardsGroup.Any(c => c.Subtypes.Contains("SetUp"))) 
            maxRepetitionsAllowed = 70;
        return maxRepetitionsAllowed;
    }
}

