using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class MainMenu : MonoBehaviour {
    //in the Background

    Transform calendar;
    UILabel title;
    GameObject tipwindow;

    void Awake()
    {
        calendar = transform.Find("Calendar");
        UIEventListener.Get(calendar.gameObject).onClick += calendarClick;

        title = transform.Find("Title").GetComponent<UILabel>();
        tipwindow = transform.Find("TipWindow").gameObject;
    }

    void Start()
    {
        if(Common.username != null)
        {
            title.text = "您好~" + Common.username;
        }
        else
        {
            //dont have user data
            SceneManager.LoadScene("Guide");
        }

        Common.now = DateTime.Now;

        string tip = null;

        //mc
        if (Common.mcdate[0] == Common.now.Month && (Common.now.Day - Common.mcdate[1]) > 25)
        {
            tip += "MC快來囉~1\n";
        }
        else if(Common.mcdate[0] == Common.now.Month-1 && (Common.now.Day - Common.mcdate[1] + 30) > 25)
        {
            tip += "MC快來囉~2\n";
        }

        //dayevent
        for(int i = 0; i<Common.events[0].Count; ++i)
        {
            Events e = Common.events[0][i];

            if (e.date[0] == Common.now.Year && e.date[1] == Common.now.Month && e.date[2] == Common.now.Day)
            {
                for (int j = 0; (j<Common.events[0][i].title.Count) && !Common.events[0][i].print[j]; ++j)
                {
                    Debug.Log("print " + Common.events[0][i].print[j]);
                    //the day
                    int d = e.from[j][0] - Common.now.Hour;

                    if(d >= 0)
                    {
                        tip += e.title[j] + '\n' + e.content[j] + '\n' + "在" + e.from[j][0].ToString() + " 點 " + e.from[j][1].ToString() + " 分 開始喔!!\n";
                        
                        if (d <= 1 )
                        {
                            //to show the event
                            tip += e.title[j] + " 即將開始喔!!\n";
                        }

                        tip += '\n';
                        Common.events[0][i].print[j] = true;
                    }
                }
            }
        }

        //pme
        for(int i = 0; i<Common.events[1].Count; ++i)
        {
            Events e = Common.events[1][i];
            if(e.date[2] == Common.now.Day)
            {
                for(int j = 0; (j<Common.events[1][i].title.Count) && !Common.events[1][i].print[j]; ++j)
                {
                    //the day
                    int d = e.from[j][0] - Common.now.Hour;

                    if(d >= 0)
                    {
                        tip += "今天是一個月一次的\n" + e.title[j] + '\n' + e.content[j] + '\n' + "在" + e.from[j][0].ToString() + " 點 " + e.from[j][1].ToString() + " 分 開始喔!!\n";

                        if (d <= 1)
                        {
                            //to show the event
                            tip += e.title[j] + " 即將開始喔!!\n";
                        }

                        tip += '\n';
                        Common.events[1][i].print[j] = true;
                    }
                }
            }
        }

        //pye
        for (int i = 0; i < Common.events[2].Count; ++i)
        {
            Debug.Log("pye11");
            Events e = Common.events[2][i];
            if (e.date[1] == Common.now.Month && e.date[2] == Common.now.Day)
            {
                for (int j = 0; (j < Common.events[2][i].title.Count) && !Common.events[2][i].print[j]; ++j)
                {
                    Debug.Log("pye");
                    //the day
                    int d = e.from[j][0] - Common.now.Hour;

                    if (d >= 0)
                    {
                        tip += "今天是一年一次的\n" + e.title[j] + '\n' + e.content[j] + '\n' + "在" + e.from[j][0].ToString() + " 點 " + e.from[j][1].ToString() + " 分 開始喔!!\n";

                        if (d <= 1)
                        {
                            //to show the event
                            tip += e.title[j] + " 即將開始喔!!\n";
                        }

                        tip += '\n';
                        Common.events[2][i].print[j] = true;
                    }
                }
            }
        }

        if (tip != null)
        {
            tipwindow.SetActive(true);
            tipwindow.SendMessage("ErrorMessage", tip);
        }

    }

    void calendarClick(GameObject go)
    {
        SceneManager.LoadScene("Calendar");
    }
}
