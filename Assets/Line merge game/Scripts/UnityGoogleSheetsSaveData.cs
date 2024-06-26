using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GoogleSheetsForUnity
{
    /* 
        This example will create a number of buttons on the scene, with self describing actions.
        It introduces to basic operations to handle spreadsheets with the API to make CRUD operations on:
        tables (worksheets) with fields (column headers), as well as objects (rows) on those tables.
    */
    public class UnityGoogleSheetsSaveData : MonoBehaviour
    {
        public static UnityGoogleSheetsSaveData Instance { get; private set; }
        [System.Serializable]
        public class testData
        {
            public string playerName = "Avishy";
            public int level = 2;
            public float health = 7.89f;
            public string role = "prog";
            public float test1 = 1;
            public float test2 = 2;
            public float test3 = 3;
            public string gameDate = "";
            public string gameTimeStart = "";

        }

        public testData testDataPost;
        //public List<testData> people;


        private void Start()
        {
            DontDestroyOnLoad(this);

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            testDataPost.gameDate = DateTime.Now.ToString("yyyy-MM-dd");
            testDataPost.gameTimeStart = DateTime.Now.ToString("HH:mm:ss");
        }
        // For the table to be created and queried.
        private string _tableName = "bla";

        

        [ContextMenu("Save State Now")]
        private void SaveState()
        {
            // Get the json string of the object.
            string jsonPlayer = JsonUtility.ToJson(testDataPost);

            // Save the object on the cloud, in a table called like the object type.
            Drive.CreateObject(jsonPlayer, _tableName, true);
        }

        //private void SaveStateList()
        //{
        //    // Get the json string of the object.
        //    string jsonData = JsonHelper.ToJson(people.ToArray());

        //    Debug.Log("<color=yellow>Sending following player to the cloud: \n</color>" + jsonData);

        //    // Save the object on the cloud, in a table called like the object type.
        //    Drive.CreateObjects(jsonData, _tableName, true);
        //}


        [ContextMenu("Update State Now")]
        private void CallUpdateState()
        {
            UpdateState(false);
        }
        private void UpdateState(bool create)
        {
            // Get the json string of the object.
            string jsonPlayer = JsonUtility.ToJson(testDataPost);

            // Look in the 'PlayerInfo' table, for an object of name as specified, and overwrite with the current obj data.
            Drive.UpdateObjects(_tableName, "playerName", testDataPost.playerName, jsonPlayer, create, true);
        }
    }
}



