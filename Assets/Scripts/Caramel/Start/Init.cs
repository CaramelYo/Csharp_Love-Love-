using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;

public class Init : MonoBehaviour {
    //in the Background
    
    void Awake()
    {
        UIEventListener.Get(gameObject).onClick = BClick;
    }

    void BClick(GameObject go)
    {
        FileInfo f = new FileInfo(Application.dataPath + '/' + Common.filename[0] + ".txt");
        StreamReader sr = null;
        StreamWriter sw = null;
        string[] split = null;

        //to load dayfile
        if (f.Exists)
        {
            sr = f.OpenText();
            split = sr.ReadToEnd().Split(',');

            Common.events[0] = new List<Events>();

            for (int i = 0; i < split.Length - 1;)
            {
                Common.events[0].Add(new Events(int.Parse(split[i++]), int.Parse(split[i++]), int.Parse(split[i++]), split[i++], split[i++], int.Parse(split[i++]), int.Parse(split[i++])));

                int id = Common.events[0].Count;
                while (split[i][0] != '\n')
                {
                    Common.events[0][id - 1].AddEvent(split[i++], split[i++], int.Parse(split[i++]), int.Parse(split[i++]));
                }
            }

            sr.Close();
        }
        else
        {
            sw = new StreamWriter(Application.dataPath + '/' + Common.filename[0] + ".txt", false, Encoding.UTF8);
            Common.events[0] = new List<Events>();
            sw.Close();
        }

        //to load pme
        f = new FileInfo(Application.dataPath + '/' + Common.filename[1] + ".txt");

        if(f.Exists)
        {
            sr = f.OpenText();
            split = sr.ReadToEnd().Split(',');

            Common.events[1] = new List<Events>();

            for(int i = 0; i<split.Length - 1;)
            {
                Common.events[1].Add(new Events(int.Parse(split[i++]), split[i++], split[i++], int.Parse(split[i++]), int.Parse(split[i++])));

                int id = Common.events[1].Count;
                while(split[i][0] != '\n')
                {
                    Common.events[1][id - 1].AddEvent(split[i++], split[i++], int.Parse(split[i++]), int.Parse(split[i++]));
                }
            }

            sr.Close();
        }
        else
        {
            sw = new StreamWriter(Application.dataPath + '/' + Common.filename[1] + ".txt", false, Encoding.UTF8);
            Common.events[1] = new List<Events>();
            sw.Close();
        }

        
        //to load pye
        f = new FileInfo(Application.dataPath + '/' + Common.filename[3] + ".txt");

        if (f.Exists)
        {
            sr = f.OpenText();
            split = sr.ReadToEnd().Split(',');

            Common.events[2] = new List<Events>();

            for (int i = 0; i < split.Length - 1;)
            {
                Common.events[2].Add(new Events(int.Parse(split[i++]), int.Parse(split[i++]), split[i++], split[i++], int.Parse(split[i++]), int.Parse(split[i++])));

                int id = Common.events[1].Count;
                while (split[i][0] != '\n')
                {
                    Common.events[1][id - 1].AddEvent(split[i++], split[i++], int.Parse(split[i++]), int.Parse(split[i++]));
                }
            }

            sr.Close();
        }
        else
        {
            sw = new StreamWriter(Application.dataPath + '/' + Common.filename[3] + ".txt", false, Encoding.UTF8);
            Common.events[2] = new List<Events>();
            sw.Close();
        }
        

        //to load user
        f = new FileInfo(Application.dataPath + '/' + Common.filename[2] + ".txt");

        if(f.Exists)
        {
            sr = f.OpenText();
            split = sr.ReadToEnd().Split(',');
            Common.username = split[0];
            Common.girlname = split[1];
            Common.mcdate[0] = int.Parse(split[2]);
            Common.mcdate[1] = int.Parse(split[3]);

            sr.Close();
        }
        else
        {
            //first use
            SceneManager.LoadScene("Guide");
        }
        
        SceneManager.LoadScene("MainMenu");
    }
}
