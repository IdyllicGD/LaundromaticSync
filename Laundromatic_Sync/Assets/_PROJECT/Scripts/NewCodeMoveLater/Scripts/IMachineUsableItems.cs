public interface IMachineUsableItems
{
    public MachineUsableItemsType ReturnMachineUsableItemsType();

}

public enum MachineUsableItemsType
{
    Detergent, FabricSoftener, Basket, None
}

public enum CleanOrDirty
{
    Clean, Dirty
}
