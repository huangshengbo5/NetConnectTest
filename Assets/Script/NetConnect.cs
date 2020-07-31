using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;

public class NetConnect : MonoBehaviour
{

    public Button Btn_Connect;

    public Button Btn_Send;

    public InputField Input_Text;

    public Text Out_Text;

    private Socket socket;


    public void Connection()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect("127.0.0.1", 9999);
    }

    public void Send()
    {
        string sendStr = Input_Text.text;
        byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
        socket.Send(sendBytes);

        byte[] readBuff = new byte[1024];
        int count = socket.Receive(readBuff);
        string recvStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
        Out_Text.text = recvStr;
        socket.Close();
    }                                                                                                                                                                                     
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
