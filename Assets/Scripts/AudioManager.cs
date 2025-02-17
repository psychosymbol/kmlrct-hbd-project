using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public AudioSource BGMsource;
    public List<AudioSource> SFXchannels = new List<AudioSource>();

    private void Start()
    {
        foreach (Transform item in transform)
        {
            if (item == transform.GetChild(0)) BGMsource = item.GetComponent<AudioSource>();
            else SFXchannels.Add(item.GetComponent<AudioSource>());
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
