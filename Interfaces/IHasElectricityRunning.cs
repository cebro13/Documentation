public interface IHasElectricityRunning
{
    public bool IsElectricityRunning();
    public void SetElectricityRunning(Utils.ElectricalContext context, bool isElectricityRunning);
}
