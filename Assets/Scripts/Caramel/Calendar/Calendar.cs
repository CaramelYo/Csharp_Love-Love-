using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DayEvent
{
    DayEvent(int ny, int nm, int nd)
    {
        date[0] = ny;
        date[1] = nm;
        date[2] = nd;
    }

    public void AddEvent(string t, string c, int fh, int fm, int th, int tm)
    {
        title.Add(t);
        content.Add(c);

        from.Add(new int[2]);
        from[from.Count - 1][0] = fh;
        from[from.Count - 1][1] = fm;

        to.Add(new int[2]);
        to[to.Count - 1][0] = th;
        to[to.Count - 1][1] = tm;
    }

    int[] date = new int[3];
    List<string> title = new List<string>(), content = new List<string>();
    List<int[]> from = new List<int[]>(), to = new List<int[]>();
}

public class Calendar : MonoBehaviour {

    //in the Background

    UIButton[] days = new UIButton[42];
    DateTime now;
    UIGrid grid;
    GameObject memobg, eventmenu, topmonth, lastmonth, nextmonth, newevent, selectedday, emyear, emmonth, emday, emnew, emcancel;
    UILabel memo, emtitle, emcontent;
    int[] nowdate = new int[3];
    Color thismonth = new Color(0f, 0f, 0f, 1f), othermonth = new Color(.7f, .7f, .7f, 1f), selected = new Color(0f, 0f, 1f,1f);

    List<DayEvent> de;
    
    //Transform grid;

    void Awake()
    {
        //to get component
        memobg = transform.Find("MemoBg").gameObject;
        memo = memobg.transform.Find("Memo").GetComponent<UILabel>();

        newevent = transform.Find("NewEvent").gameObject;
        UIEventListener.Get(newevent.gameObject).onClick = NeweventClick;

        grid = transform.Find("Days").GetComponent<UIGrid>();

        topmonth = transform.Find("TopMonth").gameObject;
        lastmonth = topmonth.transform.Find("LastMonth").gameObject;
        nextmonth = topmonth.transform.Find("NextMonth").gameObject;
        UIEventListener.Get(lastmonth).onClick = LastmonthClick;
        UIEventListener.Get(nextmonth).onClick = NextmonthClick;

        eventmenu = transform.Find("EventMenu").gameObject;
        emyear = eventmenu.transform.Find("Year").gameObject;
        emmonth = eventmenu.transform.Find("Month").gameObject;
        emday = eventmenu.transform.Find("Day").gameObject;
        emnew = eventmenu.transform.Find("New").gameObject;
        emcancel = eventmenu.transform.Find("Cancel").gameObject;
        //UIEventListener.Get(emyear).onClick = emYearClick;
        //UIEventListener.Get(emmonth).onClick = emMonthClick;
        //UIEventListener.Get(emday).onClick = emDayClick;
        UIEventListener.Get(emnew).onClick = emNewClick;
        //UIEventListener.Get(emcancel).onClick = emCancelClick;

        //to create the 42 days (buttons)
        for (int l = 0; l<42; ++l)
        {
            days[l] = days[0] != null ?((GameObject)Instantiate(days[0].gameObject, Vector3.zero, Quaternion.identity)).GetComponent<UIButton>() : ((GameObject)Instantiate(Resources.Load("Caramel/Calendar/Day"), Vector3.zero, Quaternion.identity)).GetComponent<UIButton>();
            days[l].transform.parent = grid.transform;
            days[l].transform.localScale = Vector3.one;
            //to give the event
            UIEventListener.Get(days[l].gameObject).onClick = DayClick;
        }


        //to align
        grid.enabled = true;
        
        now = DateTime.Now;

        memobg.SetActive(false);
        eventmenu.SetActive(false);
    }

    void Start()
    {
        FillDays();
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
    }

    void FillDays()
    {
        //to fill the label
        int daysinmonth = GetDaysInMonth(now), nowofweek = (int)new DateTime(now.Year, now.Month, 1).DayOfWeek, i;

        topmonth.transform.Find("TopMonthLabel").GetComponent<UILabel>().text = now.Month.ToString();

        //to fill the label on days
        for (i = 0; i < daysinmonth; ++i)
        {
            days[nowofweek + i].transform.Find("DayLabel").GetComponent<UILabel>().text = (i + 1).ToString();
            days[nowofweek + i].transform.Find("DayLabel").GetComponent<UILabel>().color = thismonth;
        }

        //after
        int k = 1;
        for (int j = i + nowofweek; j < 42; ++j)
        {
            days[j].transform.Find("DayLabel").GetComponent<UILabel>().text = (k++).ToString();
            days[j].transform.Find("DayLabel").GetComponent<UILabel>().color = othermonth;
        }

        k = now.Month != 1 ? GetDaysInMonth(new DateTime(now.Year, now.Month - 1, 1)) + 1 : GetDaysInMonth(new DateTime(now.Year - 1, 12, 1)) + 1;

        //before
        for (i = 0; i < nowofweek; ++i)
        {
            days[i].transform.Find("DayLabel").GetComponent<UILabel>().text = (k + i - nowofweek).ToString();
            days[i].transform.Find("DayLabel").GetComponent<UILabel>().color = othermonth;
        }

        days[nowofweek + now.Day - 1].transform.Find("DayLabel").GetComponent<UILabel>().color = selected;
    }

    /*
    void Setselectedday()
    {
        days[(int)new DateTime(now.Year, now.Month, 1).DayOfWeek + now.Day - 1].transform.Find("DayLabel").GetComponent<UILabel>().color = selected;
    }
    */

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

    void LastmonthClick(GameObject go)
    {
        now = now.Month != 1 ? new DateTime(now.Year, now.Month - 1, 1) : new DateTime(now.Year - 1, 12, 1);

        Start();
    }

    void NextmonthClick(GameObject go)
    {
        now = now.Month != 12 ? new DateTime(now.Year, now.Month + 1, 1) : new DateTime(now.Year + 1, 1, 1);

        Start();
    }

    void NeweventClick(GameObject go)
    {
        //to init
        emyear.transform.Find("YearLabel").GetComponent<UILabel>().text = now.Year.ToString();
        emmonth.transform.Find("MonthLabel").GetComponent<UILabel>().text = now.Month.ToString();
        emday.transform.Find("DayLabel").GetComponent<UILabel>().text = now.Day.ToString();

        eventmenu.SetActive(true);
    }

    void emNewClick(GameObject go)
    {

    }

    void DayClick(GameObject go)
    {
        //to call the memo
        if (memobg.activeSelf == false)
            memobg.SetActive(true);

        UILabel l = go.transform.Find("DayLabel").GetComponent<UILabel>();
        int m = -1;     //-1 => error

        //to get the month
        if (l.color == thismonth || l.color == selected)
        {
            Debug.Log("this month");
            m = now.Month;

            now = new DateTime(now.Year, now.Month, int.Parse(l.text));

            //selectedday.transform.Find("DayLabel").GetComponent<UILabel>().color = thismonth;
            //selectedday = go.GetComponent<UIButton>();

            l.color = selected;
        }
        else if(l.color == othermonth)
        {
            Debug.Log("other month");

            int d = int.Parse(l.text);

            if(d >= 1 && d <= 14)
            {
                //next month
                m = now.Month + 1;
                now = m != 13 ? new DateTime(now.Year, m, d) : new DateTime(now.Year + 1, 1, d);
            }
            else if(d >= 24 && d <= 31)
            {
                m = now.Month - 1;
                now = m != 0 ? new DateTime(now.Year, m, d) : new DateTime(now.Year - 1, 12, d);
            }

            Start();
        }

        Debug.Log("month = " + m.ToString());
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
