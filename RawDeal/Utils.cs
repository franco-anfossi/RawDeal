using RawDealView.Formatters;

namespace RawDeal;

public static class Utils
{
    public static string[] OpenDeckArchive(string archive)
    {
        string[] archiveLines = File.ReadAllLines(archive);
        return archiveLines;
    }
    public static void ChangePositionsOfTheList<T>(List<T> list)
    {
        (list[0], list[1]) = (list[1], list[0]);
    }
    
    public static void ChangePositionsOfTheDictionary<T>(Dictionary<int, T> dictionary)
    {
        (dictionary[0], dictionary[1]) = (dictionary[1], dictionary[0]);
    }

    public static List<string> FormatDecksOfCards(List<IViewableCardInfo> deckOfCards)
    {
        List<string> formattedCardData = new List<string>();
        foreach (var card in deckOfCards)
        {
            string formattedCard = Formatter.CardToString(card);
            formattedCardData.Add(formattedCard);
        }

        return formattedCardData;
    }
}