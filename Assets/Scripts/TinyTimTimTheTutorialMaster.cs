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
    public string tinytimtimTrueName;

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

    [TextArea(10, 7)]
    public string[] EndGameSequence;
    public Sprite[] EndGameSequenceIMG;

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
        BookDone,
        CompleteTutorial,
        GameEnd,
        CreditSequence,
        RestartSequence
    };

    private TutorialPart currentPart;
    public bool isReadyToRestart = false;
    public bool isInCreditSequence = false;

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
        control.FreezeControl();
        currentPart = TutorialPart.Book;
    }
    public void StartBookOpenTutorial()
    {
        currentPart = TutorialPart.BookOpen;
    }

    public void StartEndGameSequence()
    {
        currentPart = TutorialPart.GameEnd;
        ChatBoxController.instance.ShowChat(tinytimtimSprite, tinytimtimTrueName, EndGameSequence, EndGameSequenceIMG);
    }

    public void BookTutorialRead()
    {
        readBookTutorial = true;
    }

    public void FinishTutorial()
    {
        currentPart = TutorialPart.BookDone;
    }

    public void InitializedGame()
    {
        if (fullGameIsInit) return;
        var cm = Camera.main;
        cm
        .DOOrthoSize(2500, 1);

        cm.transform
        .DOMove(new Vector3(0, 0, -10), 1)
        .OnComplete(() =>
        {
            ShowStage();
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
                control.UnFreezeControl();
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
            case TutorialPart.BookOpen: //done book tutorial chatbox
                if (!firstBookOpen)
                {
                    ChatBoxController.instance.ShowChat(tinytimtimSprite, tinytimtimName, bookOpenTutorial, bookOpenTutorialIMG);
                    firstBookOpen = true;
                }
                break;
            case TutorialPart.BookDone:
                if (ChatBoxController.instance.inChatSequence == false) //done book open tutorial chatbox
                {
                    InitializedGame();
                    currentPart = TutorialPart.CompleteTutorial;
                }
                break;
            case TutorialPart.CompleteTutorial:
                if (ChatBoxController.instance.inChatSequence == false) //done book open tutorial chatbox
                {
                    bool markForEnding = false;

                    markForEnding = allCGInStage[0].transform.childCount + allCGInStage[1].transform.childCount == 0;

                    if (markForEnding)
                    {
                        StartEndGameSequence();
                    }
                }
                break;
            case TutorialPart.GameEnd:
                if (ChatBoxController.instance.inChatSequence == false) //done book open tutorial chatbox
                {
                    BookController.instance.OpenBook();
                    BookController.instance.StartCreditSequence();
                    currentPart = TutorialPart.CreditSequence;
                }
                break;
            case TutorialPart.CreditSequence:
                if (ChatBoxController.instance.inChatSequence == false) //done book open tutorial chatbox
                {
                    if(!BookController.instance.isInCreditSequence)
                    {
                        currentPart = TutorialPart.RestartSequence;
                    }
                }
                break;
            case TutorialPart.RestartSequence:
                if (ChatBoxController.instance.inChatSequence == false) //done book open tutorial chatbox
                {
                    isReadyToRestart = true;
                }
                break;
        }
    }
}
