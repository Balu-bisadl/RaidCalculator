using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class WindowController : MonoBehaviour
{
    [SerializeField] GameObject[] windows;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            for (int i = 0; i<windows.Count(); i++)
            {
                if (windows[i] != null)
                {
                    if (windows[i].active)
                    {
                        if (touch.phase == TouchPhase.Began && !ispointeroverUI(touch.position,i))
                        {
                            if (!istouchinsidewindow(touch.position, i))
                            {
                                windows[i].SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }
    bool ispointeroverUI(Vector2 Touchposition, int windowindex)
    {
        PointerEventData eventdata = new PointerEventData(EventSystem.current);
        eventdata.position = Touchposition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventdata, results);
        foreach (var r in results)
        {
            if (windows[windowindex]==r.gameObject)
            {
                return true;
            }
        }


        return false;
    }
    bool istouchinsidewindow(Vector2 Touchposition,int windowindex)
    {
        RectTransform windowrect = windows[windowindex].GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(windowrect, Touchposition);
    }
}
