using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameFunctionScripts : MonoBehaviour
{
    public static GameFunctionScripts Instance;
    public NetworkView netView; //必須先建立物件，才能使用RPC
    public string IP = "140.116.245.71";
    public int Port = 1000;

    public UILabel MessageText;
    public UIInput Input;
    //public UIInput Name;
    public UIButton ConnectButton;
    private bool once1, once2, once3;
    // Use this for initialization
    void Start()
    {
        Instance = this;
        once1 = once2 = once3 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (once1)
        {
            MessageText.text += "\nREADY...\n";
            once1 = false;
        }
        if (Network.peerType != NetworkPeerType.Disconnected)
        {
            if (once2)
            {
                MessageText.text += "connecting\n";
                once2 = false;
            }
            /* for server
            if (Network.peerType == NetworkPeerType.Server)
            {
                textInfo.text = "Server\n" + "Connections: " + Network.connections.Length + "\n";
                if (Network.connections.Length > 0) Debug.Log(Network.connections[0]);
            }
            */
            if (Network.peerType == NetworkPeerType.Client)
            {
                if (once3)
                {
                    MessageText.text += "Client\n";
                    once3 = false;
                }
            }
        }
    }

    public void StartClient()
    {
        Network.Connect(IP, Port);
        MessageText.text += "START CLIENT...\n";
    }

    /* for server
    public void StartServer()
    {
        Network.InitializeServer(10, Port, true);
        MessageText.text += "START SERVER...\n";
    }
    */

    public void Logout()
    {
        Network.Disconnect(250);
        MessageText.text += "LOG OUT!!\n";
    }

    public void _SendMessage()
    {
        netView.RPC("MessageCtoS", RPCMode.Server,  Common.username + " : "+Input.value);
        Input.value = "";
    }

    [RPC]
    private void MessageCtoS(string str) // client to server
    {

    }

    [RPC]
    private void MessageStoC(string str) // server to clients
    {
        MessageText.text += str + "\n";
    }

}