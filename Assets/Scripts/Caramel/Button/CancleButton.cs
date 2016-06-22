using UnityEngine;
using System.Collections;

public class CancleButton : MonoBehaviour
{
    Transform errorwindow;

    void Awake()
    {
        UIEventListener.Get(gameObject).onClick = CancleClick;
        errorwindow = transform.parent;
    }

    void CancleClick(GameObject go)
    {
        if(errorwindow.gameObject.activeSelf)
        {
            errorwindow.gameObject.SetActive(false);
        }
    }
}
