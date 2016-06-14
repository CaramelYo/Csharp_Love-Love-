using UnityEngine;
using System.Collections;
using System;

public class Calendar : MonoBehaviour {

    //in the Grid

    UIButton[] days = new UIButton[35];
    DateTime iMonth, now;
    
    //Transform grid;

    void Awake()
    {

        //to create the 35 days (buttons)
        for(int l = 0; l<35; ++l)
        {
            days[l] = days[0] != null ?((GameObject)Instantiate(days[0].gameObject, Vector3.zero, Quaternion.identity)).GetComponent<UIButton>() : ((GameObject)Instantiate(Resources.Load("Caramel/Calendar/Day"), Vector3.zero, Quaternion.identity)).GetComponent<UIButton>();
            days[l].transform.parent = transform; //grid
            days[l].transform.localScale = Vector3.one;
            UIEventListener.Get(days[l].gameObject).onClick = DayClick;
        }

        GetComponent<UIGrid>().enabled = true;

        //to fill the label on days

        //now = DateTime.Now;
        now = new DateTime(2014, 3, 15);
        int daysinmonth = GetDaysInMonth(now), nowofweek = (int)new DateTime(now.Year, now.Month, 1).DayOfWeek, i;

        for (i = 0; i < daysinmonth; ++i)
        {
            days[nowofweek + i].transform.Find("DayLabel").GetComponent<UILabel>().text = (i + 1).ToString();
        }
        
        //after
        int k = 1;
        for(int j = i + nowofweek; j<35; ++j)
        {
            days[j].transform.Find("DayLabel").GetComponent<UILabel>().text = (k++).ToString();
        }

        k = GetDaysInMonth(new DateTime(now.Year, now.Month-1, now.Day)) + 1;

        Debug.Log(k);

        //before
        for(i = 0; i < nowofweek; ++i)
        {
            days[i].transform.Find("DayLabel").GetComponent<UILabel>().text = (k + i - nowofweek).ToString();
        }



        switch ((int)new DateTime(now.Year, now.Month, 1).DayOfWeek)
        {
            case 0:
                Debug.Log("日");
                break;
            case 1:
                Debug.Log("一");
                break;
            case 2:
                Debug.Log("二");
                break;
            case 3:
                Debug.Log("三");
                break;
            case 4:
                Debug.Log("四");
                break;
            case 5:
                Debug.Log("五");
                break;
            case 6:
                Debug.Log("六");
                break;
        }
    }
    
	// Use this for initialization
	void Start ()
    {
	
	}

    int GetDaysInMonth(DateTime dt)
    {
        switch(dt.Month)
        {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                return 31;

            case 4:
            case 6:
            case 9:
            case 11:
                return 30;

            case 2:

                if((dt.Year % 100 == 0 && dt.Year % 400 == 0))
                {
                    return 29;
                }
                else if(dt.Year % 100 == 0)
                {
                    return 28;
                }
                else if(dt.Year % 4 == 0)
                {
                    return 29;
                }
                else
                {
                    return 28;
                }
        }

        return -1;
    }

    void DayClick(GameObject go)
    {
        Debug.Log("day click");
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
