using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using TMPro;
// using MySql.Data.MySqlClient;

/* 
    Unsecure...
*/
class AcceptAllCertificates : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}

public class saveData : MonoBehaviour {
    public Transform MyTransform=null;
    public Vector2 LastPos = Vector2.zero;
    public int Roll_Value = 0;

    /*  
        Gets data from MySql table using script w/ dir "uri"
    */
    IEnumerator GetRequest(string uri){
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)){
            webRequest.certificateHandler = new AcceptAllCertificates();
            yield return webRequest.SendWebRequest();
            if(webRequest.result == UnityWebRequest.Result.ConnectionError){
                Debug.Log(webRequest.error);
            } else {
                string dataText = webRequest.downloadHandler.text;
                string[] s = dataText.Split(':');
                string[] s2 = s[2].Split('<');
                // Roll_Value = int.Parse(s[0]);
                Roll_Value = int.Parse(s2[0]);
            }
        }
    }

    IEnumerator SendRequest(string uri){
        WWWForm form = new WWWForm();
        form.AddField("MyTransform", MyTransform.ToString());
        form.AddField("LastPos", LastPos.ToString());
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form)){
            webRequest.certificateHandler = new AcceptAllCertificates();
            yield return webRequest.SendWebRequest();
            if(webRequest.result == UnityWebRequest.Result.ConnectionError){
                Debug.Log(webRequest.error);
            } else {
                Debug.Log("Form upload complete!");
            }
        }
    }
    //SavePosition before going to Pause Menu
    void SaveData()
    {
        StartCoroutine(SendRequest("http://localhost/budget-app/public/send-data.php"));
    
    } 


    void RetreiveData()
    {
        // Runs data collection script
        StartCoroutine(GetRequest("http://localhost/budget-app/public/get-data.php"));
    }

    

    void Awake () {
        MyTransform = GetComponent<Transform> ();
    }

    void Start()
    {
        
        
    }

    // It is called when the scene is cut off
    void OnDestroy () {
        
    }

    public TMP_Text RollText;
    void Update(){
        RetreiveData();
    }

    
}

