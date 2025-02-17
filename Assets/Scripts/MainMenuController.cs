using DG.Tweening;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public enum GameMode
    {
        KML = 0,
        RCT = 1
    }

    public GameMode gameMode;

    [SerializeField]
    private Color[] bgColors;

    [SerializeField]
    private Texture2D[] mouseCursor;

    [SerializeField]
    private GameObject[] particleSet;

    [SerializeField]
    private GameObject[] gameEdition;

    [SerializeField]
    private RectTransform gameIcon;

    [SerializeField]
    private RectTransform startText;

    private bool iconIsDoneMoving = false;
    private bool textIsDoneShowing = false;

    [SerializeField]
    private GameObject startGameButton;

    private float sinElapse;

    void Start()
    {
        Camera.main.backgroundColor = bgColors[(int)gameMode];
        Cursor.SetCursor(mouseCursor[(int)gameMode], Vector2.zero, CursorMode.Auto);
        particleSet[(int)gameMode].SetActive(true);
        gameEdition[(int)gameMode].SetActive(true);

        gameIcon.DOAnchorPos(new Vector3(0, 160), 1).From(new Vector3(0, -1000)).OnComplete(() =>
        {
            var cg = startText.GetComponent<CanvasGroup>();
            cg.DOFade(1, 1).OnComplete(() =>
            {
                sinElapse = 0;
                iconIsDoneMoving = true;
                textIsDoneShowing = true;
                startGameButton.SetActive(true);
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        sinElapse += Time.deltaTime;

        if (iconIsDoneMoving)
        {
            float newZvalue = Mathf.Sin(sinElapse);
            gameIcon.transform.eulerAngles = new Vector3(0, 0, newZvalue * 20);
        }
        if (textIsDoneShowing)
        {
            float newScaleValue = Mathf.Sin(sinElapse * 2);
            startText.transform.localScale = new Vector3(1 + (newScaleValue * .1f), 1 + (newScaleValue * .1f), 1);
        }
    }

    private bool startTransitionSequence = false;

    public void ButtonClick()
    {
        if (startTransitionSequence) return;
        startTransitionSequence = true;
        TransitionToNextScene();
    }
    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    [SerializeField]
    private RectTransform[] transitionBase;
    [SerializeField]
    private RectTransform[] transitionBG;

    void TransitionToNextScene()
    {
        RectTransform currentTrans = transitionBase[(int)gameMode];
        RectTransform currentTransBG = transitionBG[(int)gameMode];
        currentTrans.DOScale(new Vector3(1, 1, 1), 1).From(new Vector3(0, 0, 1)).OnComplete(() =>
        {
            currentTransBG.DOScale(new Vector3(15, 15, 1), .5f).From(new Vector3(1.05f, 1.1f, 1)).SetDelay(.25f).OnComplete(() =>
            {
                GoToNextScene();
            });
        });

    }
}
