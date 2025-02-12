using UnityEngine;

public class TutorialPointer : MonoBehaviour
{

    float timeElapse;
    [SerializeField]
    private Transform pointer;
    [SerializeField]
    private Transform target;
    private void OnEnable()
    {
        timeElapse = 0;
    }
    private void Update()
    {
        transform.up = Camera.main.ScreenToWorldPoint(target.position);
        timeElapse += Time.deltaTime;

        float newScaleValue = Mathf.Sin(timeElapse * 2);
        pointer.transform.localPosition = new Vector3(0, 50 + (newScaleValue * 10), 0);
    }
}
