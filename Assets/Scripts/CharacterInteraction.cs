using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterInteraction : MonoBehaviour
{
    public Sprite imgSprite;
    public Sprite[] contentIMG;
    [TextArea(10,7)]
    public string[] contentText;
    public string contentOwner;
    public Color particleColor;
    public GameObject foundParticle;
    public UnityEvent OnChatCompleteEvent;
    public int bookIndex;
    public bool isInteracted = false;
    public bool isBookButton = false;

    private bool flagForStar = false;
    private void Start()
    {
        if (isBookButton)
            imgSprite = GetComponent<Image>().sprite;
        ParticleSystem parti = foundParticle.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = parti.main;
        main.startColor = particleColor;
    }

    public void Interacted()
    {
        isInteracted = true;
        ChatBoxController.instance.ShowChat(imgSprite, contentOwner, contentText, contentIMG);
        if (!isBookButton)
        {
            ParticleActivation(true);
            flagForStar = true;
        }
    }

    private void Update()
    {
        if (isInteracted)
            if (!ChatBoxController.instance.inChatSequence)
            {
                if (flagForStar)
                {
                    flagForStar = false;
                    BookController.instance.SpawnStar(this, OnChatCompleteEvent);
                }

                if (isBookButton)
                {
                    OnChatCompleteEvent.Invoke();
                }
            }
    }

    public void ParticleActivation(bool active)
    {
        foundParticle.SetActive(active);
    }
}
