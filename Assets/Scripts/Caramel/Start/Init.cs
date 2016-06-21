using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class Init : MonoBehaviour {
    //in the Background

    
    void Awake()
    {
        UIEventListener.Get(gameObject).onClick = BClick;
    }

    void BClick(GameObject go)
    {
        //to load dayfile
        StreamReader sr = new StreamReader(Common.filename[0]);
        string[] split = sr.ReadToEnd().Split(',');

        Common.events[0] = new List<Events>();
        
        for(int i = 0; i<split.Length; )
        {
            //Common.events[0].Add(new Events(y, m, d, emtitle.text, emcontent.text, emap.text == "AM" ? int.Parse(emhour.text) : int.Parse(emhour.text) + 12, int.Parse(emminute.text)));
        }


        SceneManager.LoadScene("Interface");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
