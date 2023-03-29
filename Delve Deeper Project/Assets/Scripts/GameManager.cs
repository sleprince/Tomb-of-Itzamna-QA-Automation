using UnityEngine;

public sealed class GameManager
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance; 
        }
    }

    public bool GotKey { get; set; }
}
