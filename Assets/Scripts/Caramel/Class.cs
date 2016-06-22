using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public static class Common
{
    public static DateTime now = DateTime.Now;
    public static List<Events>[] events = new List<Events>[3];
    public static string[] filename = new string[] { "DayEvents", "pme", "User", "pye" };
    public static string username = null, girlname = null;
    public static int[] mcdate = new int[2];
}

public class Day
{
    public Day(Transform d)
    {
        day = d;
        label = day.Find("DayLabel").GetComponent<UILabel>();
        button = day.GetComponent<UIButton>();
        e[0] = -1;
        e[1] = -1;
    }

    public Transform day;
    public UILabel label;
    public UIButton button;
    public int[] e = new int[2];
}

public class Events
{
    public Events(int ny, int nm, int nd, string t, string c, int fh, int fm)
    {
        date[0] = ny;
        date[1] = nm;
        date[2] = nd;
        AddEvent(t, c, fh, fm);
    }

    public Events(int d, string t, string c, int fh, int fm)
    {
        date[0] = -1;
        date[1] = -1;
        date[2] = d;
        AddEvent(t, c, fh, fm);
    }

    public Events(int m, int d, string t, string c, int fh, int fm)
    {
        date[0] = -1;
        date[1] = m;
        date[2] = d;
        AddEvent(t, c, fh, fm);
    }

    public void AddEvent(string t, string c, int fh, int fm)
    {
        title.Add(t);
        content.Add(c);

        from.Add(new int[2]);
        from[from.Count - 1][0] = fh;
        from[from.Count - 1][1] = fm;

        print.Add(false);
    }

    public int[] date = new int[3];
    public List<string> title = new List<string>(), content = new List<string>();
    public List<int[]> from = new List<int[]>();
    public List<bool> print = new List<bool>();
}


