using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    //in the Back
    void Awake()
    {
        UIEventListener.Get(gameObject).onClick = BackClick;
    }

    void BackClick(GameObject go)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
