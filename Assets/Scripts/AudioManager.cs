using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;
    private static readonly object padlock = new object();

    public static AudioManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AudioManager>();
                }
                return instance;
            }
        }
    }

    
}