using System.Collections.Generic;

namespace RawDeal.Boundaries;

public static class BoundaryListExtensions
{
    public static BoundaryList<T> ToBoundaryList<T>(this IEnumerable<T> source)
    {
        var boundaryList = new BoundaryList<T>();
        foreach (var item in source)
        {
            boundaryList.Add(item);
        }
        return boundaryList;
    }
}
