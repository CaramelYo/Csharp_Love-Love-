using UnityEngine;
using System.Collections;

public class Delete : MonoBehaviour {
    //in the MemoButton

    UILabel l;
    GameObject calendar;
    
    void Awake()
    {
        UIEventListener.Get(gameObject).onClick += DeleteClick;
        l = transform.Find("Memo").GetComponent<UILabel>();
        calendar = GameObject.Find("UI Root/Background");
    }

    void DeleteClick(GameObject go)
    {
        string t = l.text.Split('\n')[0];
        Debug.Log("Click");
        Debug.Log("l = " + l.text);
        Debug.Log("t = " + t);
        for (int i = 0; i<3; ++i)
        {
            for(int j = 0; j<Common.events[i].Count; ++j)
            {
                Events e = Common.events[i][j];
                for(int k = 0; k<e.title.Count; ++k)
                {
                    if(e.title[k].Equals(t))
                    {
                        Debug.Log("Delete");
                        Common.events[i].RemoveAt(j);
                        Destroy(gameObject);
                        calendar.SendMessage("SetEvent");
                        return;
                    }
                }
            }
        }
    }
}
