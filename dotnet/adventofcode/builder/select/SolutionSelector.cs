namespace adventofcode.builder.select;

public class SolutionSelector
{
    public ISelector<HistorianHysteria> HistorianHysteria => new Selector<HistorianHysteria>();
    public ISelector<RedNosedReports> RedNosedReports => new Selector<RedNosedReports>();
    public ISelector<MullItOver> MullItOver => new Selector<MullItOver>();
    public ISelector<CeresSearch> CeresSearch => new Selector<CeresSearch>();
    public ISelector<PrintQueue> PrintQueue => new Selector<PrintQueue>();
    public ISelector<GuardGallivant> GuardGallivant => new Selector<GuardGallivant>();
    public ISelector<BridgeRepair> BridgeRepair => new Selector<BridgeRepair>();
    public ISelector<ResonantCollinearity> ResonantCollinearity => new Selector<ResonantCollinearity>();
    public ISelector<DiskFragmenter> DiskFragmenter => new Selector<DiskFragmenter>();
    public ISelector<HoofIt> HoofIt => new Selector<HoofIt>();
    public ISelector<PlutonianPebbles> PlutonianPebbles => new Selector<PlutonianPebbles>();
    public ISelector<GardenGroups> GardenGroups => new Selector<GardenGroups>();
    public ISelector<ClawContraption> ClawContraption => new Selector<ClawContraption>();
    public ISelector<RestroomRedoubt> RestroomRedoubt => new Selector<RestroomRedoubt>();
}
