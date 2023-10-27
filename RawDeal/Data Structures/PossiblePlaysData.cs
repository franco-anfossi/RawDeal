using RawDealView.Formatters;

namespace RawDeal.Data_Structures;

public class PossiblePlaysData
{
    public List<string> FormattedPlays { get; }
    public List<IViewablePlayInfo> NotFormattedPlays { get; }
    
    public PossiblePlaysData(List<string> formattedPlays, List<IViewablePlayInfo> notFormattedPlays)
    {
        NotFormattedPlays = notFormattedPlays;
        FormattedPlays = formattedPlays;
    }
}