using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class DBManager : MonoBehaviour
{
    [SerializeField] private string dburl = "projectmwd.pro";
    [SerializeField] private string dbname = "mywaydefence";
    [SerializeField] private string dbid = "root";
    [SerializeField] private string dbpw = "Gnrhkdtkfkd!2";
    protected string conStr = string.Empty;

    #region["Start is called before the first frame update"] 
    private void Start()
    {
        conStr = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", dburl, dbname, dbid, dbpw);
    }
    #endregion


}
