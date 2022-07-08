
[System.Serializable]
public class PlayerSettings
{
    public float touchSensitivity;
    public float zoomSensitivity;
    public PlayerSettings(Settings settings)
    {
        touchSensitivity = settings.touchSensitivity;
        zoomSensitivity = settings.zoomSensitivity;
    }

    //Costruttore di default per il primo avvio.
    public PlayerSettings()
    {
        touchSensitivity = 1f;
        zoomSensitivity = 1f;
    }
}
