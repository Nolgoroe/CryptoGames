using System;
using UnityEngine;


namespace GoogleSheetsForUnity
{
    [System.Serializable]
    public class SaveDataContainer
    {
        [Header("General one time data")]
        public string playerID = "Rando"; //No need reset
        public string rangeOfBalls = "null"; //No need reset
        public string allBallSizes = "null"; //No need reset
        public string buildVersion = "null"; //No need reset
        public string gameDate = ""; //No need reset
        public string appLaunchTime = ""; //No need reset
        public string timeUntilAppClose = ""; //No need reset
        public int gamesThisSession = 1; //No need reset

        [Header("End condition data")]
        public string endCondition = "null"; //No need reset

        [Header("Merge data")]
        public int totalMerges = 0; //need reset
        public string timeToFirstMerge = "null"; //need reset
        public string averageTimeBetweenMerges = "null"; //need reset
        public int maxBallIndexReached = 0; //need reset

        [Header("Ball data")]
        public int totalDropCount = 0; //need reset
        public string averageDropTime = "null"; //need reset
        public int consecutiveBallsDroppedWithinXTime = 0; //need reset
        public int deadBalls = 0; //need reset
        public int ball1Probability = 0; //need reset
        public int ball2Probability = 0; //need reset
        public int ball3Probability = 0; //need reset

        [Header("Powerup Data data")]
        public int power1UseCount = 0; //need reset
        public int power2UseCount = 0; //need reset
        public int power3UseCount = 0; //need reset

        [Header("container Fill data")]
        public string timeContainerFilled30percent = "null"; //need reset
        public string timeContainerFilled60percent = "null"; //need reset
        public string timeContainerFilled90percent = "null"; //need reset

        [Header("Score data")]
        public int oldScore = 0; //need reset
        public int newScoreNoMulti = 0; //need reset
        public int NewScoreNormalAll = 0; //need reset

        [Header("Score Times")]
        public string timeGet5000Points = "null"; //need reset
        public string timeGet10000Points = "null"; //need reset
        public string timeGet25000Points = "null"; //need reset
        public string timeGet50000Points = "null"; //need reset
        public string timeGet75000Points = "null"; //need reset
        public string timeGet100000Points = "null"; //need reset
        public string timeGet150000Points = "null"; //need reset
        public string timeGet250000Points = "null"; //need reset
        public string timeGet500000Points = "null"; //need reset
        public string timeGet1000000Points = "null"; //need reset

        [Header("Combo data")]
        public int combo2 = 0; //need reset
        public int combo3 = 0; //need reset
        public int combo4 = 0; //need reset
        public int combo5 = 0; //need reset
        public int combo6 = 0; //need reset
        public int combo7 = 0; //need reset
        public int combo8 = 0; //need reset
        public int combo9 = 0; //need reset
        public int combo10 = 0; //need reset
        public int combo11 = 0; //need reset
        public int combo12 = 0; //need reset
        public int combo13 = 0; //need reset
        public int combo14 = 0; //need reset
        public int combo15 = 0; //need reset

        [Header("Timer")]
        public float timeFromStartOfNewSession;
    }

    public class UnityGoogleSheetsSaveData : MonoBehaviour, ISaveLoadable
    {
        public static UnityGoogleSheetsSaveData Instance { get; private set; }

        // For the table to be created and queried.
        private string _tableName = "Cloud Save Data";

        [Header("Save Data")]
        public SaveDataContainer saveDataContainer;
        //public List<testData> people;

        [Header("Helper Game Data")]
        public float currentGameTime;
        public int[] expectedScores;

        private void Awake()
        {

            DontDestroyOnLoad(this);

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

        }
        private void Start()
        {
            saveDataContainer.playerID = SystemInfo.deviceUniqueIdentifier;
            saveDataContainer.buildVersion = Application.productName + " Build num: " + Application.version;

            if (saveDataContainer.gameDate != DateTime.Now.ToString("yyyy-MM-dd"))
            {
                saveDataContainer.gameDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            saveDataContainer.appLaunchTime = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Update()
        {
            currentGameTime += Time.deltaTime;
            saveDataContainer.timeFromStartOfNewSession += Time.deltaTime;
            UpdateTimeTillAppClose((int)currentGameTime);
        }

        public void CallSaveState()
        {
            SaveState();
        }
        [ContextMenu("Save State Now")]
        private void SaveState()
        {
            // Get the json string of the object.
            string jsonPlayer = JsonUtility.ToJson(saveDataContainer);

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
            string jsonPlayer = JsonUtility.ToJson(saveDataContainer);

            // Look in the 'PlayerInfo' table, for an object of name as specified, and overwrite with the current obj data.
            Drive.UpdateObjects(_tableName, "playerName", saveDataContainer.playerID, jsonPlayer, create, true);
        }

        public void UpdateAllBallSizes(BallBase[] allBalls)
        {
            saveDataContainer.allBallSizes = " ";

            for (int i = 0; i < allBalls.Length; i++)
            {
                saveDataContainer.allBallSizes += " Ball " + i + " Size: " + allBalls[i].transform.localScale.x + " + ";
            }
        }




        public void UpdateRangeOfBalls(int minimum, int maximum)
        {
            saveDataContainer.rangeOfBalls = minimum.ToString() + " - " + maximum.ToString();
        }

        public void UpdateTimeTillAppClose(int time)
        {
            RecordTimeToString(ref saveDataContainer.timeUntilAppClose, time);
        }

        private void RecordTimeToString(ref string recorder, int time)
        {
            int hours = TimeSpan.FromSeconds(time).Hours;
            int minutes = TimeSpan.FromSeconds(time).Minutes;
            int seconds = TimeSpan.FromSeconds(time).Seconds;

            recorder = hours + ":" + minutes + ":" + seconds;
        }
        public void AddToGamesThisSession()
        {
            //every time we lose and restart / just restart
            saveDataContainer.gamesThisSession++;
        }
        public void AddToTotalMerges()
        {
            //every time we lose and restart / just restart
            saveDataContainer.totalMerges++;

            if (saveDataContainer.timeToFirstMerge == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeToFirstMerge, (int)saveDataContainer.timeFromStartOfNewSession);
            }


            saveDataContainer.averageTimeBetweenMerges = (saveDataContainer.timeFromStartOfNewSession / saveDataContainer.totalMerges).ToString();
        }
        public void AddToDeadBalls()
        {
            //every time we lose and restart / just restart
            saveDataContainer.deadBalls++;
        }

        public void AddToConsecutiveBallsDroppedWithinXTime()
        {
            saveDataContainer.consecutiveBallsDroppedWithinXTime++;
        }

        public void AddToPowerupUsage(int powerupID)
        {
            switch (powerupID)
            {
                case 1:
                    saveDataContainer.power1UseCount++;
                    break;
                case 2:
                    saveDataContainer.power2UseCount++;
                    break;
                case 3:
                    saveDataContainer.power3UseCount++;
                    break;
                default:
                    break;
            }
            //every time we lose and restart / just restart
            saveDataContainer.deadBalls++;
        }
        public void UpdateToMaxBallIndexReached(int index)
        {
            //every time we lose and restart / just restart
            if (index > saveDataContainer.maxBallIndexReached && index < GameManager.staticBallDatabase.balls.Length)
                saveDataContainer.maxBallIndexReached = index;
        }

        public void UpdateDataOnDropBall()
        {
            saveDataContainer.totalDropCount++;

            saveDataContainer.averageDropTime = (saveDataContainer.timeFromStartOfNewSession / saveDataContainer.totalDropCount).ToString();
        }
        public void UpdateScores(ScoreType scoreType, int score)
        {
            switch (scoreType)
            {
                case ScoreType.old:
                    saveDataContainer.oldScore = score;
                    break;
                case ScoreType.newNoMulti:
                    saveDataContainer.newScoreNoMulti = score;

                    break;
                case ScoreType.NewAll:
                    saveDataContainer.NewScoreNormalAll = score;
                    UpdateScoreTimes(score);
                    break;
                default:
                    break;
            }
        }
        public void UpdateEndCondition(string endCondition)
        {
            saveDataContainer.endCondition = endCondition;
        }

        private void UpdateScoreTimes(int score)
        {
            if (score > expectedScores[0] && saveDataContainer.timeGet5000Points == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeGet5000Points, (int)saveDataContainer.timeFromStartOfNewSession);
            }
            else if (score > expectedScores[1] && saveDataContainer.timeGet10000Points == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeGet10000Points, (int)saveDataContainer.timeFromStartOfNewSession);

            }
            else if (score > expectedScores[2] && saveDataContainer.timeGet25000Points == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeGet25000Points, (int)saveDataContainer.timeFromStartOfNewSession);

            }
            else if (score > expectedScores[3] && saveDataContainer.timeGet50000Points == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeGet50000Points, (int)saveDataContainer.timeFromStartOfNewSession);

            }
            else if (score > expectedScores[4] && saveDataContainer.timeGet75000Points == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeGet75000Points, (int)saveDataContainer.timeFromStartOfNewSession);

            }
            else if (score > expectedScores[5] && saveDataContainer.timeGet100000Points == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeGet100000Points, (int)saveDataContainer.timeFromStartOfNewSession);

            }
            else if (score > expectedScores[6] && saveDataContainer.timeGet150000Points == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeGet150000Points, (int)saveDataContainer.timeFromStartOfNewSession);

            }
            else if (score > expectedScores[7] && saveDataContainer.timeGet250000Points == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeGet250000Points, (int)saveDataContainer.timeFromStartOfNewSession);

            }
            else if (score > expectedScores[8] && saveDataContainer.timeGet500000Points == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeGet500000Points, (int)saveDataContainer.timeFromStartOfNewSession);

            }
            else if (score > expectedScores[9] && saveDataContainer.timeGet1000000Points == "null")
            {
                RecordTimeToString(ref saveDataContainer.timeGet1000000Points, (int)saveDataContainer.timeFromStartOfNewSession);

            }

        }
        public void UpdateContainerFilledTimes(FillAmount fillAmount)
        {
            switch (fillAmount)
            {
                case FillAmount.fill30:
                    if (saveDataContainer.timeContainerFilled30percent == "null")
                        RecordTimeToString(ref saveDataContainer.timeContainerFilled30percent, (int)saveDataContainer.timeFromStartOfNewSession);
                    break;
                case FillAmount.fill60:
                    if (saveDataContainer.timeContainerFilled60percent == "null")
                        RecordTimeToString(ref saveDataContainer.timeContainerFilled60percent, (int)saveDataContainer.timeFromStartOfNewSession);

                    break;
                case FillAmount.fill90:
                    if (saveDataContainer.timeContainerFilled90percent == "null")
                        RecordTimeToString(ref saveDataContainer.timeContainerFilled90percent, (int)saveDataContainer.timeFromStartOfNewSession);

                    break;
                default:
                    break;
            }
        }
        public void UpdateBallProbabilities(LimitedSpawnBalls[] ballsCanSpawn)
        {
            saveDataContainer.ball1Probability = ballsCanSpawn[0].percentageToSpawn;
            saveDataContainer.ball2Probability = ballsCanSpawn[1].percentageToSpawn;
            saveDataContainer.ball3Probability = ballsCanSpawn[2].percentageToSpawn;
        }

        public void TranslateComboListToSpecificData(int comboCount)
        {
            switch (comboCount)
            {
                case 2:
                    saveDataContainer.combo2++;
                    break;
                case 3:
                    saveDataContainer.combo3++;
                    break;
                case 4:
                    saveDataContainer.combo4++;
                    break;
                case 5:
                    saveDataContainer.combo5++;
                    break;
                case 6:
                    saveDataContainer.combo6++;
                    break;
                case 7:
                    saveDataContainer.combo7++;
                    break;
                case 8:
                    saveDataContainer.combo8++;
                    break;
                case 9:
                    saveDataContainer.combo9++;
                    break;
                case 10:
                    saveDataContainer.combo10++;
                    break;
                case 11:
                    saveDataContainer.combo11++;
                    break;
                case 12:
                    saveDataContainer.combo12++;
                    break;
                case 13:
                    saveDataContainer.combo13++;
                    break;
                case 14:
                    saveDataContainer.combo14++;
                    break;
                case 15:
                    saveDataContainer.combo15++;
                    break;
                default:
                    break;
            }

        }
        public void DataReset()
        {
            saveDataContainer.timeFromStartOfNewSession = 0;

            saveDataContainer.totalMerges = 0;
            saveDataContainer.maxBallIndexReached = 0;
            saveDataContainer.timeToFirstMerge = "null";
            saveDataContainer.averageTimeBetweenMerges = "null";


            saveDataContainer.endCondition = "null";


            saveDataContainer.totalDropCount = 0;
            saveDataContainer.averageDropTime = "null";
            saveDataContainer.consecutiveBallsDroppedWithinXTime = 0;
            saveDataContainer.deadBalls = 0;


            saveDataContainer.power1UseCount = 0;
            saveDataContainer.power2UseCount = 0;
            saveDataContainer.power3UseCount = 0;


            saveDataContainer.timeContainerFilled30percent = "null";
            saveDataContainer.timeContainerFilled60percent = "null";
            saveDataContainer.timeContainerFilled90percent = "null";



            saveDataContainer.oldScore = 0;
            saveDataContainer.newScoreNoMulti = 0;
            saveDataContainer.NewScoreNormalAll = 0;



            saveDataContainer.timeGet5000Points = "null";
            saveDataContainer.timeGet10000Points = "null";
            saveDataContainer.timeGet25000Points = "null";
            saveDataContainer.timeGet50000Points = "null";
            saveDataContainer.timeGet75000Points = "null";
            saveDataContainer.timeGet100000Points = "null";
            saveDataContainer.timeGet150000Points = "null";
            saveDataContainer.timeGet250000Points = "null";
            saveDataContainer.timeGet500000Points = "null";
            saveDataContainer.timeGet1000000Points = "null";


            saveDataContainer.combo2 = 0;
            saveDataContainer.combo3 = 0;
            saveDataContainer.combo4 = 0;
            saveDataContainer.combo5 = 0;
            saveDataContainer.combo6 = 0;
            saveDataContainer.combo7 = 0;
            saveDataContainer.combo8 = 0;
            saveDataContainer.combo9 = 0;
            saveDataContainer.combo10 = 0;
            saveDataContainer.combo11 = 0;
            saveDataContainer.combo12 = 0;
            saveDataContainer.combo13 = 0;
            saveDataContainer.combo14 = 0;
            saveDataContainer.combo15 = 0;

        }




        public void LoadData(GameData data)
        {
            saveDataContainer = data.googleSheetData;

            ForceResetData();
        }

        public void SaveData(GameData data)
        {
            data.googleSheetData = saveDataContainer;
        }

        private void ForceResetData()
        {
            saveDataContainer.gamesThisSession = 0;
        }
    }
}



