using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class Guide : MonoBehaviour
{
    //in the Background
    UILabel uname, gname, mcmonth, mcday;
    GameObject yes;

    void Awake()
    {
        uname = transform.Find("NameInput/NameDetail").GetComponent<UILabel>();
        gname = transform.Find("GNameInput/GNameDetail").GetComponent<UILabel>();
        mcmonth = transform.Find("MonthInput/MonthDetail").GetComponent<UILabel>();
        mcday = transform.Find("DayInput/DayDetail").GetComponent<UILabel>();

        yes = transform.Find("Yes").gameObject;
        UIEventListener.Get(yes).onClick += yesClick;
    }

    void yesClick(GameObject go)
    {
        try
        {
            StreamWriter sw = new StreamWriter(Application.persistentDataPath + '/' + Common.filename[3] + ".txt", false, Encoding.UTF8);

            sw.Write(uname.text + ',' + gname.text + ',' + int.Parse(mcmonth.text) + ',' + int.Parse(mcday.text));
            sw.Close();

            Common.username = uname.text;
            Common.girlname = gname.text;
            Common.mcdate[0] = int.Parse(mcmonth.text);
            Common.mcdate[1] = int.Parse(mcday.text);

            SceneManager.LoadScene("MainMenu");
        }
        catch
        {
            GameObject e = null;

            if ( ! (e = GameObject.Find("UI Root/Background/ErrorWindow(Clone)")) )
            {
                //to create errorwindow
                e = (GameObject)Instantiate(Resources.Load("Caramel/ErrorWindow"), Vector3.zero, Quaternion.identity );
                e.transform.localScale = Vector3.one;
                e.SetActive(true);
            }

            e.SendMessage("ErrorMessage", "內容有誤!!");
        }
    }
        
}
