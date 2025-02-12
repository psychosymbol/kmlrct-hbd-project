using UnityEngine;
using UnityEngine.UI;

public class CharacterInteraction : MonoBehaviour
{
    private Sprite imgSprite;
    public Sprite[] contentIMG;
    [TextArea(10,7)]
    public string[] contentText;
    public string contentOwner;
    public Color particleColor;
    public GameObject foundParticle;

    private void Start()
    {
        imgSprite = GetComponent<Image>().sprite;
        ParticleSystem parti = foundParticle.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = parti.main;
        main.startColor = particleColor;
    }

    public void Interacted()
    {
        ChatBoxController.instance.ShowChat(imgSprite, contentOwner, contentText, contentIMG);
        foundParticle.SetActive(true);
    }
}
