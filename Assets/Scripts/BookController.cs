using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BookController : MonoBehaviour
{
    public static BookController instance;
    public List<PageController> pages = new List<PageController>();
    public List<Button> pageButtons = new List<Button>();

    private void Awake()
    {
        if (!instance) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        currentPage = 0;
    }

    public RectTransform bookButtonRT;
    public RectTransform markRT;
    public RectTransform centerRT;
    public RectTransform bookButtonRTForScaling;
    public CanvasGroup bookButtonCG;
    public RectTransform bookGroupRT;
    public CanvasGroup bookGroupCG;
    public RectTransform coverRT;
    public RectTransform contentRT;
    public CanvasGroup frontCoverCG;
    public CanvasGroup bgCG;

    public void OpenBook()
    {
        bookButtonRT.GetComponent<Animator>().enabled = false;
        bookButtonRT.DOScale(Vector3.zero, .5f);
        bgCG
            .DOFade(1, .25f)
            .From(0)
            .OnComplete(() =>
        {
            bookGroupRT.localScale = new Vector3(.1f, .1f, 1);
            bookGroupCG.alpha = 1;
            bookGroupCG.interactable = true;
            bookGroupCG.blocksRaycasts = true;

            bookGroupRT
            .DOScale(Vector3.one, .5f);
            bookGroupRT
            .DOLocalMove(Vector3.zero, .5f)
            .From(bookButtonRT.localPosition)
            .OnComplete(() =>
            {
                //frontCoverCG.DOFade(1,0).SetDelay(.5f);
                coverRT
                .DORotate(new Vector3(0, 180, 0), 1f)
                .From(new Vector3(0, 0, 0))
                .OnUpdate(() =>
                {
                    if (coverRT.localEulerAngles.y >= 90) frontCoverCG.alpha = 1;
                })
                .OnComplete(() =>
                {
                    bgCG.interactable = true;
                    bgCG.blocksRaycasts = true;
                    contentRT.SetAsLastSibling();
                    if (!TinyTimTimTheTutorialMaster.TinyTimTim.firstBookOpen)
                        TinyTimTimTheTutorialMaster.TinyTimTim.StartBookOpenTutorial();
                });
            });
        });
    }

    public void CloseBook()
    {
        if (!TinyTimTimTheTutorialMaster.TinyTimTim.readBookTutorial) return;
        bgCG.interactable = false;
        bgCG.blocksRaycasts = false;

        PageController.FlipSide side = PageController.FlipSide.Right;

        for (int i = currentPage - 1; i > -1; i--)
        {
            pages[i].FlipPage(.5f, 0, 0, side);
        }

        coverRT
            .DORotate(new Vector3(0, 0, 0), .5f)
            .From(new Vector3(0, 180, 0))
            .OnUpdate(() =>
        {
            if (coverRT.localEulerAngles.y <= 90)
            {
                frontCoverCG.alpha = 0;
                coverRT.SetAsLastSibling();
            }
        })
            .OnComplete(() =>
        {
            bookGroupRT
            .DOScale(new Vector3(0, 0, 1), .25f);
            bookGroupRT
            .DOLocalMove(bookButtonRT.localPosition, .25f)
            .From(Vector3.zero)
            .OnComplete(() =>
            {
                bookGroupCG.alpha = 0;
                bookGroupCG.interactable = false;
                bookGroupCG.blocksRaycasts = false;

                bgCG
                .DOFade(0, .25f)
                .From(1)
                .OnComplete(() =>
                {
                    bookButtonRT
                    .DOScale(Vector3.one, .25f)
                    .OnComplete(() =>
                    {
                        bookButtonRT.GetComponent<Animator>().enabled = true;
                        if(!TinyTimTimTheTutorialMaster.TinyTimTim.fullGameIsInit)
                            TinyTimTimTheTutorialMaster.TinyTimTim.FinishTutorial();
                    });
                });
            });
        });
        currentPage = 0;
    }

    public void ShowBookButton(Sprite sprite, string contentOwner, string[] contentText, Sprite[] contentImg)
    {
        bookButtonCG.alpha = 1;
        bookButtonCG.interactable = true;
        bookButtonRTForScaling
            .DOScale(Vector3.one * 1.1f, 1f)
            .From(Vector3.zero)
            .OnComplete(() =>
        {
            bookButtonRTForScaling
            .DOScale(Vector3.one, .5f)
            .From(Vector3.one * 1.1f)
            .OnComplete(() =>
            {
                bookButtonCG.interactable = true;
                bookButtonCG.blocksRaycasts = true;


                ChatBoxController.instance.ShowChat(sprite, contentOwner, contentText, contentImg);
            });
        });
    }

    private int currentPage = 0;

    public void FlipBookToPage(int pageNum)
    {
        PageController.FlipSide side = PageController.FlipSide.Left;
        float time = 1f;
        float delayIncrement = .1f;
        float delay = 0;
        if (pageNum == currentPage) return;
        if (pageNum > currentPage)
        {
            side = PageController.FlipSide.Left;

            for (int i = currentPage; i < pageNum; i++)
            {
                pages[i].FlipPage(time, delay,i, side);
                delay += delayIncrement;
            }
        }
        else if (pageNum < currentPage)
        {
            side = PageController.FlipSide.Right;

            for (int i = currentPage - 1; i > pageNum - 1; i--)
            {
                pages[i].FlipPage(time, delay, 0, side);
                delay += delayIncrement;
            }
        }

        currentPage = pageNum;
    }

    [SerializeField]
    private GameObject star;

    public void SpawnStar(CharacterInteraction chara, UnityEvent OnComplete)
    {
        RectTransform charaRT = chara.GetComponent<RectTransform>();
        
        Vector3 spawnPos = charaRT.position;
        spawnPos.z = 0f;
        Vector3 targetPos = markRT.position;
        targetPos.z = 0f;
        Vector3 camCenter = centerRT.position;
        camCenter.z = 0f;

        Animator starAnim = star.GetComponent<Animator>();
        starAnim.enabled = false;

        Image starIMG = star.GetComponent<Image>();
        //starIMG.color = chara.particleColor;

        Button charaButton = chara.GetComponent<Button>();
        charaButton.interactable = false;

        Animator charaAnim = chara.GetComponent<Animator>();
        charaAnim.enabled = false;

        RectTransform starRT = star.GetComponent<RectTransform>();


        starRT.position = spawnPos;
        starRT.localScale = Vector3.zero;
        star.SetActive(true);

        float randSpeed = Random.Range(.1f, 1f);

        int randValue = Random.Range(0, 2) * 2 - 1;
        float echoPulse = 0;
        float pulseTimer = .05f;

        charaRT
            .DOScale(Vector3.zero, 1)
            .SetDelay(1)
            .OnUpdate(() =>
            {
                chara.ParticleActivation(false);
            });
        //starRT
        //    .DOMove(camCenter, 1).SetDelay(1);
        starRT
            .DOScale(Vector3.one * 1.1f, .75f)
            .From(Vector3.zero)
            .SetDelay(1)
            .OnUpdate(() =>
            {
                starRT.Rotate(new Vector3(0f, 0f, randSpeed * randValue));
            })
            .OnComplete(() =>
            {
                starRT
                .DOScale(Vector3.one, .25f)
                .From(Vector3.one * 1.1f)
                .OnUpdate(() =>
                {
                    starRT.Rotate(new Vector3(0f, 0f, randSpeed * randValue));
                })
                .OnComplete(() =>
                {
                    starAnim.enabled = true;
                    starRT
                    .DOScale(Vector3.one * .1f, 1f)
                    .From(Vector3.one);
                    starRT
                    .DOMove(targetPos, 1f)
                    .From(spawnPos)
                    .OnUpdate(() =>
                    {
                        if(echoPulse >= pulseTimer)
                        {
                            RectTransform newStar = Instantiate(star, starRT.position, starRT.rotation).GetComponent<RectTransform>();
                            newStar.localScale = starRT.localScale;
                            newStar.SetParent(centerRT);
                            newStar.GetComponent<Animator>().enabled = false;
                            newStar.GetComponent<Image>().sprite = star.GetComponent<Image>().sprite;
                            newStar
                            .DOScale(Vector3.zero, .5f)
                            .SetDelay(.1f)
                            .OnComplete(() =>
                            {
                                Destroy(newStar.gameObject);
                            });

                            echoPulse = 0;
                        }
                        Debug.Log(echoPulse);
                        echoPulse += Time.deltaTime;
                        starRT.Rotate(new Vector3(0f, 0f, randSpeed * randValue));
                    })
                    .OnComplete(() =>
                    {
                        chara.gameObject.SetActive(false);
                        star.SetActive(false);
                        UpdateButtonInBook(chara.bookIndex);
                        OnComplete.Invoke();
                    });
                });
            });
    }

    public void UpdateButtonInBook(int index)
    {
        pageButtons[index].interactable = true;
    }
}
