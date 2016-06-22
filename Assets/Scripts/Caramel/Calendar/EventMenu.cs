using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

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
        UIEventListener.Get(emnew).onClick += emNewClick;
        UIEventListener.Get(emcancel).onClick += emCancelClick;
    }

    void emNewClick(GameObject go)
    {
        int y = int.Parse(emyear.text);
        int m = int.Parse(emmonth.text);
        int d = int.Parse(emday.text);
        int i= 0, j = -1;

        switch (emmode.text)
        {
            case "今日事件":
                j = 0;
                break;
            case "每月事件":
                j = 1;
                break;
            case "每年事件":
                j = 2;
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
                        break;
                    }
                }
                break;

            case 2:
                for(i = 0; i<Common.events[j].Count; ++i)
                {
                    if(Common.events[j][i].date[1] == m && Common.events[j][i].date[2] == d)
                    {
                        //found
                        Common.events[j][i].AddEvent(emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text));
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
                case 2:
                    Common.events[j].Add(new Events(m, d, emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text)));
                    break;
            }
        }

        StreamWriter sw = new StreamWriter(Application.dataPath + '/' + Common.filename[j] + ".txt", false,  Encoding.UTF8);

        for (i = 0; i < Common.events[j].Count; ++i)
        {
            switch(j)
            {
                case 0:
                    sw.Write(Common.events[0][i].date[0] + "," + Common.events[0][i].date[1] + "," + Common.events[0][i].date[2] + ',');
                    break;
                case 1:
                    sw.Write(Common.events[1][i].date[2] + ",");
                    break;
                case 2:
                    sw.Write(Common.events[2][i].date[1] + "," + Common.events[2][i].date[2] + ",");
                    break;
            }

            for (int k = 0; k < Common.events[j][i].title.Count; ++k)
            {
                sw.Write(Common.events[j][i].title[k] + "," + Common.events[j][i].content[k] + "," + Common.events[j][i].from[k][0] + "," + Common.events[j][i].from[k][1] + ',');
            }
            sw.Write('\n');
            sw.Flush();
        }
        sw.Close();

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
}
