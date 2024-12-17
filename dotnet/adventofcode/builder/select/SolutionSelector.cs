using adventofcode.solutions;

namespace adventofcode.builder.select;

public class SolutionSelector
{
    public ISelector<HistorianHysteria> Day1 => new Selector<HistorianHysteria>();
    public ISelector<HistorianHysteria> HistorianHysteria => new Selector<HistorianHysteria>();

    public ISelector<RedNosedReports> Day2 => new Selector<RedNosedReports>();
    public ISelector<RedNosedReports> RedNosedReports => new Selector<RedNosedReports>();

    public ISelector<MullItOver> Day3 => new Selector<MullItOver>();
    public ISelector<MullItOver> MullItOver => new Selector<MullItOver>();

    public ISelector<CeresSearch> Day4 => new Selector<CeresSearch>();
    public ISelector<CeresSearch> CeresSearch => new Selector<CeresSearch>();

    public ISelector<PrintQueue> Day5 => new Selector<PrintQueue>();
    public ISelector<PrintQueue> PrintQueue => new Selector<PrintQueue>();

    public ISelector<GuardGallivant> Day6 => new Selector<GuardGallivant>();
    public ISelector<GuardGallivant> GuardGallivant => new Selector<GuardGallivant>();

    public ISelector<BridgeRepair> Day7 => new Selector<BridgeRepair>();
    public ISelector<BridgeRepair> BridgeRepair => new Selector<BridgeRepair>();

    public ISelector<ResonantCollinearity> Day8 => new Selector<ResonantCollinearity>();
    public ISelector<ResonantCollinearity> ResonantCollinearity => new Selector<ResonantCollinearity>();

    public ISelector<DiskFragmenter> Day9 => new Selector<DiskFragmenter>();
    public ISelector<DiskFragmenter> DiskFragmenter => new Selector<DiskFragmenter>();

    public ISelector<HoofIt> Day10 => new Selector<HoofIt>();
    public ISelector<HoofIt> HoofIt => new Selector<HoofIt>();

    public ISelector<PlutonianPebbles> Day11 => new Selector<PlutonianPebbles>();
    public ISelector<PlutonianPebbles> PlutonianPebbles => new Selector<PlutonianPebbles>();

    public ISelector<GardenGroups> Day12 => new Selector<GardenGroups>();
    public ISelector<GardenGroups> GardenGroups => new Selector<GardenGroups>();

    public ISelector<ClawContraption> Day13 => new Selector<ClawContraption>();
    public ISelector<ClawContraption> ClawContraption => new Selector<ClawContraption>();

    public ISelector<RestroomRedoubt> Day14 => new Selector<RestroomRedoubt>();
    public ISelector<RestroomRedoubt> RestroomRedoubt => new Selector<RestroomRedoubt>();

    public ISelector<WarehouseWoes> Day15 => new Selector<WarehouseWoes>();
    public ISelector<WarehouseWoes> WarehouseWoes => new Selector<WarehouseWoes>();

    public ISelector<ReindeerMaze> Day16 => new Selector<ReindeerMaze>();
    public ISelector<ReindeerMaze> ReindeerMaze => new Selector<ReindeerMaze>();
}
