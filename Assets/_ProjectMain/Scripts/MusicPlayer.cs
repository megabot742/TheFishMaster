using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    void Start()
    {
        int numOfMusicPlayer = FindObjectsByType<MusicPlayer>(FindObjectsSortMode.None).Length;
        if(numOfMusicPlayer > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
