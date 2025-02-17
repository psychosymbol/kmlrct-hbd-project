using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;
    public Sprite[] cursors;

    public Image mouseCursor;

    private void Awake()
    {
        if (!Instance) Instance = this;
        else if (Instance != this) Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    public void SetCursor(int index)
    {
        mouseCursor.sprite = cursors[index];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        //mousePosition = Camera.main.ScreenToViewportPoint(mousePosition);
        mousePosition.z = 0;
        mouseCursor.transform.position = mousePosition;
    }
}
