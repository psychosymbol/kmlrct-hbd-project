using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxController : MonoBehaviour
{
    public static ChatBoxController instance;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private CanvasGroup chatBoxBG;
    [SerializeField]
    private CanvasGroup chatBox;
    [SerializeField]
    private CanvasGroup chatImg;
    [SerializeField]
    private Image chatChara;
    [SerializeField]
    private Image chatImgSprite;
    [SerializeField]
    private TextMeshProUGUI chatBoxContent;
    [SerializeField]
    private TextMeshProUGUI chatBoxOwner;

    private List<string> chatList = new List<string>();
    private List<Sprite> chatSpriteList = new List<Sprite>();
    public bool inChatSequence = false;

    private enum ChatMode
    {
        Text,
        Image
    }

    private ChatMode chatMode;

    private void Awake()
    {
        if (!instance) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public void ShowChat(Sprite sprite, string contentOwner, string[] contentText, Sprite[] contentImg)
    {
        chatChara.sprite = sprite;
        chatBoxOwner.text = contentOwner;
        chatList = new List<string>();
        chatSpriteList = new List<Sprite>();
        chatIndex = 0;
        if (contentText.Length == 0)
            chatBox.alpha = 0;
        else
        {
            chatMode = ChatMode.Text;
            chatList = new List<string>(contentText);
            chatBox.alpha = 1;
            chatBoxContent.text = chatList[chatIndex];
            PlayChatSound(chatList[chatIndex]);
        }

        if (contentImg.Length == 0)
            chatImg.alpha = 0;
        else
        {
            chatMode = ChatMode.Image;
            chatSpriteList = new List<Sprite>(contentImg);
            chatImg.alpha = 1;
            chatImgSprite.sprite = chatSpriteList[0];
            chatImgSprite.SetNativeSize();
            AudioManager.Instance.PlaySound("pop");
        }


        anim.SetBool("isShowing", true);
        chatBoxBG.interactable = true;
        chatBoxBG.blocksRaycasts = true;
        inChatSequence = true;
    }

    private int chatIndex = 0;

    public void OnBGClick()
    {
        chatIndex = chatIndex + 1;
        switch (chatMode)
        {
            case ChatMode.Text:
                if (chatIndex >= chatList.Count) { HideChat(); return; }
                else 
                {
                    chatBoxContent.text = chatList[chatIndex];
                    PlayChatSound(chatList[chatIndex]);
                }
                break;
            case ChatMode.Image:
                if (chatIndex >= chatSpriteList.Count) { HideChat(); return; }
                else
                {
                    chatImgSprite.sprite = chatSpriteList[chatIndex];
                    AudioManager.Instance.PlaySound("pop");
                }
                break;
        }
        anim.SetTrigger("Switch");
    }

    void PlayChatSound(string text)
    {
        string soundPlay = "short";
        if(text.Length < 40)
        {
            soundPlay = "short";
        }
        else if(text.Length >= 40 && text.Length < 80)
        {
            soundPlay = "long";
        }
        else
        {
            soundPlay = "longest";
        }
        AudioManager.Instance.PlaySound(soundPlay);
    }

    public void HideChat()
    {
        anim.SetBool("isShowing", false);
        chatBoxBG.interactable = false;
        chatBoxBG.blocksRaycasts = false;
        inChatSequence = false;
    }
}
