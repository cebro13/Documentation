public interface IMusicArea
{
    int GetParameterValue();
}

public enum MusicAreaDebug
{
    MUSIC_1_GREEN_HOUSE,
    MUSIC_2_RELEASE,
    MUSIC_3_CAVE
}

public enum MusicAreaConspirationnisteBunker
{
    MUSIC_1_1,
    MUSIC_1_2,
}

public enum MusicAreaConspirationnisteExterior
{
    MUSIC_0_EXTERIOR,
    MUSIC_1_STORE,
    MUSIC_2_TRAILER,
    MUSIC_3_CAVE,
    MUSIC_4_GREENHOUSE,
}

public enum MusicAreaConspirationnisteBoss
{
    MUSIC_3_1,
    MUSIC_3_2,
    MUSIC_3_3,
}

public abstract class MusicAreaWrapper : IMusicArea
{
    public abstract int GetParameterValue();
}

[System.Serializable]
public class MusicAreaConspirationnisteBunkerWrapper : MusicAreaWrapper
{
    public MusicAreaConspirationnisteBunker Area;

    public override int GetParameterValue()
    {
        return (int)Area;
    }
}

[System.Serializable]
public class MusicAreaConspirationnisteExteriorWrapper : MusicAreaWrapper
{
    public MusicAreaConspirationnisteExterior Area;

    public override int GetParameterValue()
    {
        return (int)Area;
    }
}

[System.Serializable]
public class MusicAreaConspirationnisteBossWrapper : MusicAreaWrapper
{
    public MusicAreaConspirationnisteBoss Area;

    public override int GetParameterValue()
    {
        return (int)Area;
    }
}

[System.Serializable]
public class MusicAreDebug : MusicAreaWrapper
{
    public MusicAreaDebug Area;

    public override int GetParameterValue()
    {
        return (int)Area;
    }
}