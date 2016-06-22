using UnityEngine;
using System.Collections;

public class GameFunctionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowNotification(){
		using (AndroidJavaClass unity = new AndroidJavaClass("com.example.diclab_cheng.unity_notification_plugin.LocalNotificationActivity"))
		{
			unity.CallStatic("Show", 5, "這是標題", "這是內文", "black_heart96");
		}
	}
}
