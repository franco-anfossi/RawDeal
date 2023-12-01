using RawDeal.Data_Structures;
using RawDealView.Formatters;

namespace RawDeal.Conditions;

public class MinimumFortitude : Condition
{
    private readonly int _minimumFortitude;
    private readonly ImportantPlayerData _playerData;
    
    public MinimumFortitude(IViewablePlayInfo selectedPlay, 
        ImportantPlayerData playerData, int minimumFortitude) : base(selectedPlay)
    {
        _minimumFortitude = minimumFortitude;
        _playerData = playerData;
    }

    public override bool Check()
        => _playerData.SuperstarData.Fortitude >= _minimumFortitude;
}