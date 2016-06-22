using UnityEngine;
using System.Collections;

public class OptionButton : MonoBehaviour
{
    //in the Option

    Transform optionwindow;

    void Awake()
    {
        UIEventListener.Get(gameObject).onClick = OptionClick;
        optionwindow = transform.Find("OptionWindow");
    }

    void OptionClick(GameObject go)
    {
        if(!optionwindow.gameObject.activeSelf)
        {
            optionwindow.gameObject.SetActive(true);
        }
    }
}
