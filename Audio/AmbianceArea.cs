public interface IAmbianceArea
{
    float GetParameterValue();
}

public enum AmbianceAreaConspirationnisteBunker
{
    AMBIANCE_1_1,
    AMBIANCE_1_2,
}

public enum AmbianceAreaConspirationnisteExterior
{
    AMBIANCE_0_EXTERIOR,
    AMBIANCE_1_STORE,
    AMBIANCE_2_TRAILER,
    AMBIANCE_3_CAVE,
    AMBIANCE_4_GREENHOUSE,
}

public enum AmbianceAreaConspirationnisteBoss
{
    AMBIANCE_3_1,
    AMBIANCE_3_2,
    AMBIANCE_3_3,
}

public abstract class AmbianceAreaWrapper : IAmbianceArea
{
    public abstract float GetParameterValue();
}

[System.Serializable]
public class AmbianceAreaConspirationnisteBunkerWrapper : AmbianceAreaWrapper
{
    public AmbianceAreaConspirationnisteBunker Area;

    public override float GetParameterValue()
    {
        return (float)Area;
    }
}

[System.Serializable]
public class AmbianceAreaConspirationnisteExteriorWrapper : AmbianceAreaWrapper
{
    public AmbianceAreaConspirationnisteExterior Area;

    public override float GetParameterValue()
    {
        return (float)Area;
    }
}

[System.Serializable]
public class AmbianceAreaConspirationnisteBossWrapper : AmbianceAreaWrapper
{
    public AmbianceAreaConspirationnisteBoss Area;

    public override float GetParameterValue()
    {
        return (float)Area;
    }
}