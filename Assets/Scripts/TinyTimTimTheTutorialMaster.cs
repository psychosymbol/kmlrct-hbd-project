using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TinyTimTimTheTutorialMaster : MonoBehaviour
{
    public static TinyTimTimTheTutorialMaster TinyTimTim;
    [SerializeField] 
    private CameraControl control;

    public Sprite tinytimtimSprite;
    public string tinytimtimName;

    [TextArea(10, 7)]
    public string[] zoomTutorial;
    public Sprite[] zoomTutorialIMG;


    [TextArea(10, 7)]
    public string[] moveTutorial;
    public Sprite[] moveTutorialIMG;


    [TextArea(10, 7)]
    public string[] bookTutorial;
    public Sprite[] bookTutorialIMG;

    [TextArea(10, 7)]
    public string[] bookOpenTutorial;
    public Sprite[] bookOpenTutorialIMG;

    private bool bookAlreadyShown = false;

    public GameObject TheTutorialMasterHimself;

    public bool firstBookOpen = false;
    public bool readBookTutorial = false;
    public bool fullGameIsInit = false;
    private enum TutorialPart
    {
        Start,
        Zoom,
        Move,
        Book,
        BookOpen,
        Done
    };

    private TutorialPart currentPart;

    private void Awake()
    {
        if (!TinyTimTim) TinyTimTim = this;
        else if (TinyTimTim != this) Destroy(gameObject);
    }

    private void Start()
    {
        currentPart = TutorialPart.Start;
    }

    public void StartZoomTutorial()
    {
        currentPart = TutorialPart.Zoom;
        ChatBoxController.instance.ShowChat(tinytimtimSprite, tinytimtimName, zoomTutorial, zoomTutorialIMG);
    }

    public void StartMoveTutorial()
    {
        currentPart = TutorialPart.Move;
        TheTutorialMasterHimself.GetComponent<Button>().interactable = true;
        ChatBoxController.instance.ShowChat(tinytimtimSprite, tinytimtimName, moveTutorial, moveTutorialIMG);
    }

    public void StartBookTutorial()
    {
        control.moveable = false;
        control.zoomable = false;
        currentPart = TutorialPart.Book;
    }
    public void StartBookOpenTutorial()
    {
        currentPart = TutorialPart.BookOpen;
    }

    public void BookTutorialRead()
    {
        readBookTutorial = true;
    }

    public void FinishTutorial()
    {
        currentPart = TutorialPart.Done;
    }

    public void InitializedGame()
    {
        if (fullGameIsInit) return;
        var cm = Camera.main;
        cm
            .DOOrthoSize(2500, 1)
            .OnComplete(() =>
        {
            cm.transform
            .DOMove(new Vector3(0, 0, -10), 1)
            .OnComplete(() =>
            {
                ShowStage();
            });
        });
    }

    public CanvasGroup[] allCGInStage;

    public void ShowStage()
    {
        foreach (var item in allCGInStage)
        {
            item.interactable = true;
            item.blocksRaycasts = true;
            item
                .DOFade(1, 1)
                .OnComplete(() =>
            {
                control.moveable = true;
                control.zoomable = true;
                fullGameIsInit = true;
            });
        }
    }

    private void Update()
    {
        switch (currentPart)
        {
            case TutorialPart.Start:
                break;
            case TutorialPart.Zoom:
                if (ChatBoxController.instance.inChatSequence == false) //done zoom tutorial chatbox
                {
                    control.zoomable = true;
                }

                if(Camera.main.orthographicSize <= 300)
                {
                    StartMoveTutorial();
                }
                break;
            case TutorialPart.Move:
                if (ChatBoxController.instance.inChatSequence == false) //done zoom tutorial chatbox
                {
                    control.moveable = true;
                }
                break;
            case TutorialPart.Book:
                if (ChatBoxController.instance.inChatSequence == false) //done zoom tutorial chatbox
                {
                    if (!bookAlreadyShown)
                    {
                        bookAlreadyShown = true;
                        BookController.instance.ShowBookButton(tinytimtimSprite, tinytimtimName, bookTutorial, bookTutorialIMG);
                    }
                }
                break;
            case TutorialPart.BookOpen:
                if(!firstBookOpen)
                {
                    ChatBoxController.instance.ShowChat(tinytimtimSprite, tinytimtimName, bookOpenTutorial, bookOpenTutorialIMG);
                    firstBookOpen = true;
                }
                break;
            case TutorialPart.Done:
                if (ChatBoxController.instance.inChatSequence == false) //done zoom tutorial chatbox
                {
                    InitializedGame();
                }
                break;
        }
    }
}
