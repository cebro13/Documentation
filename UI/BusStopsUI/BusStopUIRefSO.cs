
using UnityEngine;

[CreateAssetMenu(fileName = "BusStopUIRefSO", menuName = "Data/UI/BusStop")]
public class BusStopUIRefSO : ScriptableObject
{
    public BusStopUI.eBusStop busStop;
    public string buttonText;
    public string BusStopTextDescription = "Lore of this bus stop";
}
