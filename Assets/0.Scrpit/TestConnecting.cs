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

    private void Start()
    {
        Process psi = new Process();
        psi.StartInfo.FileName = "C:/Users/user/anaconda3/python.exe";
        // 파이썬 환경 연결
        psi.StartInfo.Arguments = "C:/Users/user/Documents/GitHub/AI_Study/For_Unity_Connect.py";
        // 실행할 파이썬 파일
        psi.StartInfo.CreateNoWindow = true;
        psi.StartInfo.UseShellExecute = false;
        psi.Start();
        UnityEngine.Debug.Log("실행완료");

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
    }


    void Update()
    {
        try
        {
            //유니티에 값보내기 
            string message = "Hello from Unity!";
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            clientSocket.Send(messageBytes);


            //파이썬에서 값 받기
            byte[] buffer = new byte[1024];
            int receivedBytes = clientSocket.Receive(buffer);
            string receivedMessage = System.Text.Encoding.UTF8.GetString(buffer, 0, receivedBytes); //receivedMessage 문자열받기
            UnityEngine.Debug.Log($"Received from Python: {receivedMessage}");

        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("[알림] 에러발생: " + e.Message);
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }


    private void OnDisable()
    {
        // 게임 오브젝트가 비활성화될 때 실행되는 코드
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}