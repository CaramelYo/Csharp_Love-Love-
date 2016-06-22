using UnityEngine;
using System.Collections;

public class ErrorWindow : MonoBehaviour
{
    //in the ErrorWindow

    UILabel content;

    void Awake()
    {
        content = transform.Find("Content").GetComponent<UILabel>();
    }

    public void ErrorMessage(string s)
    {
        content.text = s;
    }
}
