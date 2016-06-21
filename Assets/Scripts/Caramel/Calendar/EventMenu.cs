using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public static class Common
{
    public static DateTime now = DateTime.Now;
    public static List<Events>[] events = new List<Events>[2];
    public static string[] filename = new string[] { "DayEvents", "pme" };
}

public class EventMenu : MonoBehaviour
{
    //in the EventMenu
    GameObject emnew, emcancel, calendar;
    UILabel emtitle, emcontent, emap, emhour, emminute, emyear, emmonth, emday, emmode;
    UIInput emtinput, emcinput;

    void Awake ()
    {
        calendar = GameObject.Find("UI Root/Background");
        emyear = transform.Find("Year/YearLabel").GetComponent<UILabel>();
        emmonth = transform.Find("Month/MonthLabel").GetComponent<UILabel>();
        emday = transform.Find("Day/DayLabel").GetComponent<UILabel>();
        emnew = transform.Find("New").gameObject;
        emcancel = transform.Find("Cancel").gameObject;
        emtinput = transform.Find("TitleInput").GetComponent<UIInput>();
        emtitle = emtinput.transform.Find("TitleDetail").GetComponent<UILabel>();
        emcinput = transform.Find("ContentInput").GetComponent<UIInput>();
        emcontent = emcinput.transform.Find("ContentDetail").GetComponent<UILabel>();
        emap = transform.Find("apPopupList/apPopupListLabel").GetComponent<UILabel>();
        emhour = transform.Find("HourPopupList/HourPopupListLabel").GetComponent<UILabel>();
        emminute = transform.Find("MinutePopupList/MinutePopupListLabel").GetComponent<UILabel>();
        emmode = transform.Find("ModePopupList/ModePopupListLabel").GetComponent<UILabel>();

        //UIEventListener.Get(emyear).onClick = emYearClick;
        //UIEventListener.Get(emmonth).onClick = emMonthClick;
        //UIEventListener.Get(emday).onClick = emDayClick;
        UIEventListener.Get(emnew).onClick = emNewClick;
        UIEventListener.Get(emcancel).onClick = emCancelClick;
    }

    void emNewClick(GameObject go)
    {
        int y = int.Parse(emyear.text);
        int m = int.Parse(emmonth.text);
        int d = int.Parse(emday.text);
        int i= 0, j = -1;
        //Common.events[0]
        switch (emmode.text)
        {
            case "今日事件":
                /*
                for (i = 0; i < Events.de.Count; ++i)
                {
                    if (Events.de[i].date[0] == y && Events.de[i].date[1] == m && Events.de[i].date[2] == d)
                    {
                        //found
                        Events.de[i].AddEvent(emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text));
                        break;
                    }
                }

                if (i == Events.de.Count)
                {
                    //to create new dayevent
                    Events.de.Add(new DayEvent(y, m, d, emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text)));
                }

                StreamWriter sw = new StreamWriter(Application.dataPath + '/' + dayfile + ".txt");
                for (i = 0; i < Events.de.Count; ++i)
                {
                    sw.Write("date:\nyear: " + Events.de[i].date[0] + " month: " + Events.de[i].date[1] + " day: " + Events.de[i].date[2] + '\n');
                    for (int j = 0; j < Events.de[i].title.Count; ++j)
                    {
                        sw.Write(Events.de[i].title[j] + ',' + Events.de[i].content[j] + ", from: " + Events.de[i].from[j][0] + Events.de[i].from[j][1] + '\n');
                        sw.Flush();
                    }
                }
                sw.Close();
                */
                j = 0;
                break;
            case "每月事件":
                /*
                for (i = 0; i < Events.pme.Count; ++i)
                {
                    if (Events.pme[i].date == d)
                    {
                        //found
                        Events.pme[i].AddEvent(emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text));
                        break;
                    }
                }

                if (i == Events.pme.Count)
                {
                    //to create new pme
                    Events.pme.Add(new PerMonthEvent(d, emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text)));
                }

                sw = new StreamWriter(Application.dataPath + '/' + pmefile + ".txt");
                for (i = 0; i < Events.pme.Count; ++i)
                {
                    sw.Write(Events.pme[i].date + '\n');
                    for (int j = 0; j < Events.pme[i].title.Count; ++j)
                    {
                        sw.Write(Events.pme[i].title[j] + ',' + Events.pme[i].content[j] + ", from: " + Events.pme[i].from[j][0] + Events.pme[i].from[j][1] + '\n');
                        sw.Flush();
                    }
                }
                sw.Close();
                */
                j = 1;
                break;
        }


        switch (j)
        {
            case 0:
                for (i = 0; i < Common.events[j].Count; ++i)
                {
                    if (Common.events[j][i].date[0] == y && Common.events[j][i].date[1] == m && Common.events[j][i].date[2] == d)
                    {
                        //found
                        Common.events[j][i].AddEvent(emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text));
                        Debug.Log("event " + j + " count" + Common.events[j].Count);
                        break;
                    }
                }

                break;

            case 1:
                for (i = 0; i < Common.events[j].Count; ++i)
                {
                    if (Common.events[j][i].date[2] == d)
                    {
                        //found
                        Common.events[j][i].AddEvent(emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text));
                        Debug.Log("event " + j + " count" + Common.events[j].Count);
                        break;
                    }
                }
                break;
        }

        if (i == Common.events[j].Count)
        {
            //to create new dayevent
            switch(j)
            {
                case 0:
                    Common.events[j].Add(new Events(y, m, d, emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text)));
                    break;
                case 1:
                    Common.events[j].Add(new Events(d, emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text)));
                    break;
            }
        }

        //to save file

        for (j = 0; j < 2; ++j)
        {
            StreamWriter sw = new StreamWriter(Application.dataPath + '/' + Common.filename[j] + ".txt");

            for (i = 0; i < Common.events[j].Count; ++i)
            {
                sw.Write(Common.events[j][i].date[0] + "," + Common.events[j][i].date[1] + "," + Common.events[j][i].date[2] + ',');

                for (int k = 0; k < Common.events[j][i].title.Count; ++k)
                {
                    sw.Write(Common.events[j][i].title[k] + "," + Common.events[j][i].content[k] + "," + Common.events[j][i].from[k][0] + "," + Common.events[j][i].from[k][1] + '\n');
                    sw.Flush();
                }
            }
            sw.Close();
        }

        calendar.SendMessage("SetEvent");
        emCancelClick(null);
    }

    void emCancelClick(GameObject go)
    {
        emtinput.value = emtinput.defaultText;
        emcinput.value = emcinput.defaultText;

        gameObject.SetActive(false);
    }

    void Init()
    {
        emyear.text = Common.now.Year.ToString();
        emmonth.text = Common.now.Month.ToString();
        emday.text = Common.now.Day.ToString();
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
