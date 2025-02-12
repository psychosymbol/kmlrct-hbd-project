using DG.Tweening;
using UnityEngine;

public class TinyTimTimTheTutorialMaster : MonoBehaviour
{
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

    public GameObject TheTutorialMasterHimself;

    bool fullGameIsInit = false;
    private enum TutorialPart
    {
        Start,
        Zoom,
        Move,
        Done
    };

    private TutorialPart currentPart;

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
        ChatBoxController.instance.ShowChat(tinytimtimSprite, tinytimtimName, moveTutorial, moveTutorialIMG);
    }

    public void FinishTutorial()
    {
        currentPart = TutorialPart.Done;
    }

    public void InitializedGame()
    {
        if (fullGameIsInit) return;
        control.moveable = false;
        control.zoomable = false;
        TheTutorialMasterHimself.SetActive(false);
        var cm = Camera.main;
        cm.DOOrthoSize(2500, 1).OnComplete(() =>
        {
            cm.transform.DOMove(new Vector3(0, 0, -10), 1).OnComplete(() =>
            {
                control.moveable = true;
                control.zoomable = true;
                fullGameIsInit = true;
                ShowStage();
            });
        });
    }

    public void ShowStage()
    {

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
            case TutorialPart.Done:
                if (ChatBoxController.instance.inChatSequence == false) //done zoom tutorial chatbox
                {
                    InitializedGame();
                }
                break;
        }
    }
}
