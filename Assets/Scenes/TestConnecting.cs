using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Diagnostics;

public class TestConnecting : MonoBehaviour
{
    private Socket clientSocket;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {// �׳� R Ű ������ ����
            try
            {
                Process psi = new Process();
                psi.StartInfo.FileName = "C:/Users/user/anaconda3/python.exe";
                // ���̽� ȯ�� ����
                psi.StartInfo.Arguments = "C:/Users/user/Documents/GitHub/AI_Study/For_Unity_Connect.py";
                // ������ ���̽� ����
                psi.StartInfo.CreateNoWindow = true;
                psi.StartInfo.UseShellExecute = false;
                psi.Start();

                UnityEngine.Debug.Log("����Ϸ�");

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                IPEndPoint serverEndPoint = new IPEndPoint(serverIP, 12345);

                try
                {
                    clientSocket.Connect(serverEndPoint);
                    UnityEngine.Debug.Log("Connected to server");
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"Error connecting to server: {e}");
                    return;
                }

                string message = "Hello from Unity!";
                byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
                clientSocket.Send(messageBytes);

                byte[] buffer = new byte[1024];
                int receivedBytes = clientSocket.Receive(buffer);
                string receivedMessage = System.Text.Encoding.UTF8.GetString(buffer, 0, receivedBytes); //receivedMessage ���ڿ��ޱ�
                UnityEngine.Debug.Log($"Received from Python: {receivedMessage}");

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            

            }
            catch (Exception e)
            {
            UnityEngine.Debug.LogError("[�˸�] �����߻�: " + e.Message);
        }
    }

}
}
