﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class Calendar : MonoBehaviour {

    //in the Background

    Day[] days = new Day[42];
    UIGrid daysgrid, memogrid;
    GameObject memobg, eventmenu;
    Transform topmonth, lastmonth, nextmonth, newevent, back;
    Color thismonth = new Color(0f, 0f, 0f, 1f), othermonth = new Color(.7f, .7f, .7f, 1f), selectedday = new Color(0f, 0f, 1f, 1f), eventday = new Color(.8f, .8f, .4f, 1f), noteventday = new Color(1f, 1f, 1f, 1f);
    
    //int[] nowdate = new int[3];
    int nowfirstday, nextfirstday;

    void Awake()
    {
        //to get component
        memobg = transform.Find("MemoBg").gameObject;
        memogrid = memobg.transform.Find("MemoGrid").GetComponent<UIGrid>();

        newevent = transform.Find("NewEvent");
        UIEventListener.Get(newevent.gameObject).onClick += NeweventClick;

        daysgrid = transform.Find("Days").GetComponent<UIGrid>();

        topmonth = transform.Find("TopMonth");
        lastmonth = topmonth.Find("LastMonth");
        nextmonth = topmonth.Find("NextMonth");
        UIEventListener.Get(lastmonth.gameObject).onClick += LastmonthClick;
        UIEventListener.Get(nextmonth.gameObject).onClick += NextmonthClick;

        eventmenu = transform.Find("EventMenu").gameObject;

        //to create the 42 days (buttons)
        for (int l = 0; l<42; ++l)
        {

            days[l] = new Day (days[0] != null ? ((GameObject)Instantiate(days[0].day.gameObject, Vector3.zero, Quaternion.identity)).transform : ((GameObject)Instantiate(Resources.Load("Caramel/Calendar/Day"), Vector3.zero, Quaternion.identity)).transform );
            days[l].day.parent = daysgrid.transform;
            days[l].day.localScale = Vector3.one;

            //to give the event
            UIEventListener.Get(days[l].day.gameObject).onClick += DayClick;
        }

        //to align
        daysgrid.enabled = true;
        
        memobg.SetActive(false);
        eventmenu.SetActive(false);
    }

    void Start()
    {
        FillDays();
        SetEvent();
    }

    void FillDays()
    {
        //to fill the label
        int daysinmonth = GetDaysInMonth(Common.now), nowofweek = (int)new DateTime(Common.now.Year, Common.now.Month, 1).DayOfWeek, i;

        topmonth.Find("TopMonthLabel").GetComponent<UILabel>().text = Common.now.Month.ToString();

        //to fill the label on days
        nowfirstday = nowofweek;
        for (i = 0; i < daysinmonth; ++i)
        {
            days[nowofweek + i].label.text = (i + 1).ToString();
            days[nowofweek + i].label.color = thismonth;
            days[nowofweek + i].button.defaultColor = noteventday;
        }

        //after
        int k = 1;
        nextfirstday = i + nowofweek;
        for (int j = i + nowofweek; j < 42; ++j)
        {
            days[j].label.text = (k++).ToString();
            days[j].label.color = othermonth;
            days[j].button.defaultColor = noteventday;
        }

        k = Common.now.Month != 1 ? GetDaysInMonth(new DateTime(Common.now.Year, Common.now.Month - 1, 1)) + 1 : GetDaysInMonth(new DateTime(Common.now.Year - 1, 12, 1)) + 1;

        //before
        for (i = 0; i < nowofweek; ++i)
        {
            days[i].label.text = (k + i - nowofweek).ToString();
            days[i].label.color = othermonth;
            days[i].button.defaultColor = noteventday;
        }

        days[nowofweek + Common.now.Day - 1].label.color = selectedday;
    }

    public void SetEvent()
    {
        int d;

        //to set dayevent
        for (int i = 0; i < Common.events[0].Count; ++i)
        {
            if (Common.events[0][i].date[0] == Common.now.Year && Common.events[0][i].date[1] == Common.now.Month && Common.events[0][i].date[2] <= int.Parse(days[nextfirstday - 1].label.text))
            {
                //this month
                days[nowfirstday + Common.events[0][i].date[2] - 1].button.defaultColor = eventday;
                days[nowfirstday + Common.events[0][i].date[2] - 1].e[0] = i;
            }
            else if (Common.events[0][i].date[1] == Common.now.Month - 1 && (d = Common.events[0][i].date[2] - int.Parse(days[0].label.text)) >= 0)
            {
                days[d].button.defaultColor = eventday;
                days[d].e[0] = i;
            }
            else if (Common.events[0][i].date[1] == Common.now.Month + 1 && ( d = int.Parse(days[41].label.text) - Common.events[0][i].date[2]) >= 0)
            {
                days[41 - d].button.defaultColor = eventday;
                days[41 - d].e[0] = i;
            }
        }
        
        //pme
        for (int i = 0; i< Common.events[1].Count; ++i)
        {
            if( (int.Parse(days[nextfirstday - 1].label.text) >= Common.events[1][i].date[2]) )
            {
                //this month
                days[nowfirstday + Common.events[1][i].date[2] - 1].button.defaultColor = eventday;
                days[nowfirstday + Common.events[1][i].date[2] - 1].e[1] = i;
            }

            if(  ( d = Common.events[1][i].date[2] - int.Parse(days[0].label.text) ) >= 0  && nowfirstday!= 0 && Common.events[1][i].date[2] <= int.Parse(days[nowfirstday - 1].label.text))
            {
                //last month
                days[d].button.defaultColor = eventday;
                days[d].e[1] = i;
            }
            else if(Common.events[1][i].date[2] <= int.Parse(days[41].label.text))
            {
                d = nextfirstday + Common.events[1][i].date[2] - 1;
                days[d].button.defaultColor = eventday;
                days[d].e[1] = i;
            }
        }

        //pye
        for (int i = 0; i < Common.events[2].Count; ++i)
        { 
            if(Common.events[2][i].date[1] == Common.now.Month && Common.events[2][i].date[2] <= int.Parse(days[nextfirstday - 1].label.text))
            {
                days[nowfirstday + Common.events[2][i].date[2] - 1].button.defaultColor = eventday;
                days[nowfirstday + Common.events[2][i].date[2] - 1].e[2] = i;
            }
            else if (Common.events[2][i].date[1] == Common.now.Month - 1 && (d = Common.events[2][i].date[2] - int.Parse(days[0].label.text)) >= 0)
            {
                days[d].button.defaultColor = eventday;
                days[d].e[2] = i;
            }
            else if (Common.events[2][i].date[1] == Common.now.Month + 1 && (d = int.Parse(days[41].label.text) - Common.events[2][i].date[2]) >= 0)
            {
                days[41 - d].button.defaultColor = eventday;
                days[41 - d].e[2] = i;
            }
        }
    }

    void LastmonthClick(GameObject go)
    {
        Common.now = Common.now.Month != 1 ? new DateTime(Common.now.Year, Common.now.Month - 1, 1) : new DateTime(Common.now.Year - 1, 12, 1);
        refreshdaye();
        Start();
    }

    void NextmonthClick(GameObject go)
    {
        Common.now = Common.now.Month != 12 ? new DateTime(Common.now.Year, Common.now.Month + 1, 1) : new DateTime(Common.now.Year + 1, 1, 1);
        refreshdaye();
        Start();
    }

    void NeweventClick(GameObject go)
    {
        eventmenu.SetActive(true);
        eventmenu.SendMessage("Init");
    }

    void DayClick(GameObject go)
    {
        //to call the memo
        if (memobg.activeSelf == false)
            memobg.SetActive(true);

        //to restore the daylabel color
        days[nowfirstday + Common.now.Day - 1].label.color = thismonth;

        UILabel l = go.transform.Find("DayLabel").GetComponent<UILabel>();

        while(memogrid.transform.childCount > 0)
        {
            DestroyImmediate(memogrid.transform.GetChild(0).gameObject);
        }

        //to get the month
        if (l.color == thismonth || l.color == selectedday)
        {
            Common.now = new DateTime(Common.now.Year, Common.now.Month, int.Parse(l.text));
            l.color = selectedday;
        }
        else if(l.color == othermonth)
        {
            int da = int.Parse(l.text), m;

            if(da >= 1 && da <= 14)
            {
                //next month
                m = Common.now.Month + 1;
                Common.now = m != 13 ? new DateTime(Common.now.Year, m, da) : new DateTime(Common.now.Year + 1, 1, da);
            }
            else if(da >= 24 && da <= 31)
            {
                m = Common.now.Month - 1;
                Common.now = m != 0 ? new DateTime(Common.now.Year, m, da) : new DateTime(Common.now.Year - 1, 12, da);
            }
            Start();
        }

        Transform t;

        //to set memo
        for(int j = 0; j<3; ++j)
        {
            if (days[nowfirstday + Common.now.Day - 1].e[j] != -1)
            {
                Events d = null;

                try
                {
                    d = Common.events[j][days[nowfirstday + Common.now.Day - 1].e[j]];
                }
                catch
                {
                    days[nowfirstday + Common.now.Day - 1].e[j] = -1;
                    days[nowfirstday + Common.now.Day - 1].button.defaultColor = noteventday;
                    EventMenu.Writefile(j);
                    break;
                }

                for (int i = 0; i < d.title.Count; ++i)
                {
                    memogrid.AddChild(memogrid.GetChildList().Count == 0 ? t = ((GameObject)Instantiate(Resources.Load("Caramel/Calendar/MemoButton"), new Vector3(370f, 0f, 0f), Quaternion.identity)).transform : t = ((GameObject)Instantiate(memogrid.GetChild(0).gameObject, new Vector3(370f, 0f, 0f), Quaternion.identity)).transform);
                    t.localScale = Vector3.one;
                    t.Find("Memo").GetComponent<UILabel>().text = d.title[i] + '\n' + d.content[i] + "\n時間: " + d.from[i][0] + ':' + d.from[i][1];
                }
            }
        }

        memogrid.enabled = true;
    }

    void refreshdaye()
    {
        for(int i = 0; i<42; ++i)
        {
            days[i].e[0] = -1;
            days[i].e[1] = -1;
            days[i].e[2] = -1;
        }
    }

    int GetDaysInMonth(DateTime dt)
    {
        switch (dt.Month)
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

                if ((dt.Year % 100 == 0 && dt.Year % 400 == 0))
                    return 29;
                else if (dt.Year % 100 == 0)
                    return 28;
                else if (dt.Year % 4 == 0)
                    return 29;
                else
                    return 28;
        }
        return -1;
    }


    /* 哪一天
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
    */

    // Update is called once per frame
    void Update ()
    {
	
	}
}
