using MySql.Data.MySqlClient;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UnitManager : GameManager 
{
    public string heroname = "Hualand"; 
    
    private string dburl = "projectmwd.pro";
    private string dbname = "mywaydefence";
    private string dbid = "root";
    private string dbpw = "Gnrhkdtkfkd!2";
    protected string conStr = string.Empty;

    protected override void Start()
    {
        base.Start();
        // if(PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        // {
        //     heroname = PhotonNetwork.CurrentRoom.CustomProperties["Champion"].ToString();
        // }
        // else
        // {
            
        // }
    }

    protected override void Update()
    {
        base.Update();
        if(life == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        
    }

    protected virtual void ConnectDB()
    {
        conStr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", dburl, dbname, dbid, dbpw);
    }

    protected bool isConnectDB()
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(conStr))
            {
                conn.Open();
                Debug.Log("Connection True");
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Connection Error: " + e.Message);
            return false;
        }
    }
}
