using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DayEvent
{
    public DayEvent(int ny, int nm, int nd)
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

    public void AddEvent(string t, string c, int fh, int fm)
    {
        title.Add(t);
        content.Add(c);

        from.Add(new int[2]);
        from[from.Count - 1][0] = fh;
        from[from.Count - 1][1] = fm;
    }

    public int[] date = new int[3];
    public List<string> title = new List<string>(), content = new List<string>();
    public List<int[]> from = new List<int[]>(), to = new List<int[]>();
}

public class Calendar : MonoBehaviour {

    //in the Background

    UIButton[] days = new UIButton[42];
    DateTime now;
    UIGrid grid;
    GameObject memobg, eventmenu, topmonth, lastmonth, nextmonth, newevent, selectedday, emyear, emmonth, emday, emnew, emcancel;
    UILabel memo, emtitle, emcontent, emap, emhour, emminute;
    Color thismonth = new Color(0f, 0f, 0f, 1f), othermonth = new Color(.7f, .7f, .7f, 1f), selected = new Color(0f, 0f, 1f, 1f), eventday = new Color(.8f, .8f, .4f, 1f), noteventday = new Color(1f, 1f, 1f, 1f);
    UIInput emtinput, emcinput;
    List<int>[] tempdayevent = new List<int>[42];

    int[] nowdate = new int[3];
    int nowfirstday, nextfirstday;
    
    List<DayEvent> de = new List<DayEvent>();
    
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
        emtinput = eventmenu.transform.Find("TitleInput").GetComponent<UIInput>();
        emtitle = emtinput.transform.Find("TitleDetail").GetComponent<UILabel>();
        emcinput = eventmenu.transform.Find("ContentInput").GetComponent<UIInput>();
        emcontent = emcinput.transform.Find("ContentDetail").GetComponent<UILabel>();
        emap = eventmenu.transform.Find("apPopupList/apPopupListLabel").GetComponent<UILabel>();
        emhour = eventmenu.transform.Find("HourPopupList/HourPopupListLabel").GetComponent<UILabel>();
        emminute = eventmenu.transform.Find("MinutePopupList/MinutePopupListLabel").GetComponent<UILabel>();

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
        SetEvent();
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

        nowfirstday = nowofweek;
        //to fill the label on days
        for (i = 0; i < daysinmonth; ++i)
        {
            days[nowofweek + i].transform.Find("DayLabel").GetComponent<UILabel>().text = (i + 1).ToString();
            days[nowofweek + i].transform.Find("DayLabel").GetComponent<UILabel>().color = thismonth;
            days[nowofweek + i].GetComponent<UITexture>().color = noteventday;
        }

        //after
        int k = 1;
        nextfirstday = i + nowofweek;
        for (int j = i + nowofweek; j < 42; ++j)
        {
            days[j].transform.Find("DayLabel").GetComponent<UILabel>().text = (k++).ToString();
            days[j].transform.Find("DayLabel").GetComponent<UILabel>().color = othermonth;
            days[j].GetComponent<UITexture>().color = noteventday;
        }

        k = now.Month != 1 ? GetDaysInMonth(new DateTime(now.Year, now.Month - 1, 1)) + 1 : GetDaysInMonth(new DateTime(now.Year - 1, 12, 1)) + 1;

        //before
        for (i = 0; i < nowofweek; ++i)
        {
            days[i].transform.Find("DayLabel").GetComponent<UILabel>().text = (k + i - nowofweek).ToString();
            days[i].transform.Find("DayLabel").GetComponent<UILabel>().color = othermonth;
            days[i].GetComponent<UITexture>().color = noteventday;
        }

        days[nowofweek + now.Day - 1].transform.Find("DayLabel").GetComponent<UILabel>().color = selected;
    }

    void SetEvent()
    {
        int d;

        if(de.Count == 0)
        {
            return;
        }

        for (int i = 0; i <de.Count; ++i)
        {
            Debug.Log(de[i].date);
            if (de[i].date[1] == now.Month)
            {
                //this month
                days[nowfirstday + de[i].date[2]].GetComponent<UITexture>().color = eventday;

                /*
                if(tempdayevent[nowfirstday + de[i].date[2]].Count == 0)
                {
                    Debug.Log("null");
                    
                }
                */

                tempdayevent[nowfirstday + de[i].date[2]].Add(i);
            }
            else if (de[i].date[1] == now.Month - 1 && (d = de[i].date[2] - int.Parse(days[0].transform.Find("DayLabel").GetComponent<UILabel>().text)) >= 0)
            {
                days[d].GetComponent<UITexture>().color = eventday;
                tempdayevent[d].Add(i);
            }
            else if (de[i].date[1] == now.Month + 1 && ( d = int.Parse(days[41].transform.Find("DayLabel").GetComponent<UILabel>().text) - de[i].date[2]) >= 0)
            {
                days[41 - d].GetComponent<UITexture>().color = eventday;
                tempdayevent[41 - d].Add(i);
            }
        }
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
        int y = int.Parse(emyear.transform.Find("YearLabel").GetComponent<UILabel>().text);
        int m = int.Parse(emmonth.transform.Find("MonthLabel").GetComponent<UILabel>().text);
        int d = int.Parse(emday.transform.Find("DayLabel").GetComponent<UILabel>().text);
        int i;

        if (de.Count == 0)
        {
            de.Add(new DayEvent(y, m, d));
        }

        for(i = 0; i<de.Count; ++i)
        {
            if (de[i].date[0] == y && de[i].date[1] == m && de[i].date[2] == d)
            {
                //found
                de[i].AddEvent(emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text));
            }
        }

        if(i == de.Count)
        {
            //to create new dayevent
            de.Add(new DayEvent(y, m, d));
            de[de.Count - 1].AddEvent(emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text));
        }

        emCancelClick(null);
    }

    void emCancelClick(GameObject go)
    {
        emtinput.value = null;
        emcinput.value = null;

        eventmenu.SetActive(false);
    }

    void DayClick(GameObject go)
    {
        //to call the memo
        if (memobg.activeSelf == false)
            memobg.SetActive(true);

        days[nowfirstday + now.Day - 1].transform.Find("DayLabel").GetComponent<UILabel>().color = thismonth;

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
        SetEvent();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
