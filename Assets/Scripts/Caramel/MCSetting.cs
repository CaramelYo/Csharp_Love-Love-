using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using System.Collections;

public class MCSetting : MonoBehaviour
{
    //in the MC

    UILabel month, day;
    GameObject yes;
    Transform mcsetting;

    void Awake()
    {
        mcsetting = transform.Find("MCSetting");
        mcsetting.gameObject.SetActive(false);

        month = mcsetting.Find("MonthInput/MonthDetail").GetComponent<UILabel>();
        day = mcsetting.Find("DayInput/DayDetail").GetComponent<UILabel>();

        yes = mcsetting.Find("Yes").gameObject;

        UIEventListener.Get(yes).onClick += yesClick;
        UIEventListener.Get(gameObject).onClick += MCClick;
    }

    void yesClick(GameObject go)
    {
        try
        {
            StreamWriter sw = new StreamWriter(Application.persistentDataPath + '/' + Common.filename[3] + ".txt", false, Encoding.UTF8);

            sw.Write(Common.username + ',' + Common.girlname + ',' + int.Parse(month.text) + ',' + int.Parse(day.text));
            sw.Close();

            Common.mcdate[0] = int.Parse(month.text);
            Common.mcdate[1] = int.Parse(day.text);

            mcsetting.gameObject.SetActive(false);
        }
        catch
        {
            GameObject e = null;

            if (!(e = GameObject.Find("UI Root/Background/Option/OptionWindow/MC/ErrorWindow(Clone)")))
            {
                //to create errorwindow
                e = (GameObject)Instantiate(Resources.Load("Caramel/ErrorWindow"), Vector3.zero, Quaternion.identity);
                e.transform.localScale = Vector3.one;
                e.SetActive(true);
            }

            e.SendMessage("ErrorMessage", "內容有誤!!");
        }
    }

    void MCClick(GameObject go)
    {
        mcsetting.gameObject.SetActive(true);
    }
}
