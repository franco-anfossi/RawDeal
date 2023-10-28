using RawDeal.Boundaries;
using RawDealView.Formatters;

namespace RawDeal.Data_Structures;

public class PossiblePlaysData
{
    public BoundaryList<string> FormattedPlays { get; }
    public BoundaryList<IViewablePlayInfo> NotFormattedPlays { get; }
    
    public PossiblePlaysData(BoundaryList<string> formattedPlays, BoundaryList<IViewablePlayInfo> notFormattedPlays)
    {
        NotFormattedPlays = notFormattedPlays;
        FormattedPlays = formattedPlays;
    }
}