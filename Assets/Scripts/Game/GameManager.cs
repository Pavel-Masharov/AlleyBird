using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Text textLevel, textLevelBest, textMonets, textMonetsTotal;

    [SerializeField] GameObject popupLost;

    public static GameManager Instance;

    public int level { get; private set; }
    private int levelBest;

    private int monets;
    private int monetsTotal;
    public bool gameOver { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        level = 0;
        monets = 0;
        gameOver = false;

        Load();

        Player.DeathEvent += GameOver;
        Player.ColectedMonetEvent += ColectedMonets;
        Player.NewStepEvent += NewLevel;

        popupLost.SetActive(false);   
    }

    private void Update()
    {
        OutputInfo();
    }

    private void GameOver()
    {
        Save();
        gameOver = true;
        popupLost.SetActive(true);
    }

    private void ColectedMonets()
    {
        monets++;
        monetsTotal += monets;
    }

    private void NewLevel()
    {
        level++;

        if(levelBest < level)
        {
            levelBest = level;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
        popupLost.SetActive(false);
        gameOver = false;
    }

    private void OutputInfo()
    {
        textLevel.text = level.ToString();
        textLevelBest.text = "BEST: " + levelBest.ToString();

        textMonets.text = monets.ToString();
        textMonetsTotal.text = "Total: " + monetsTotal.ToString();
    }

    private void OnDisable()
    {
        Player.DeathEvent -= GameOver;
        Player.ColectedMonetEvent -= ColectedMonets;
        Player.NewStepEvent -= NewLevel;
    }


    [System.Serializable]
    class SaveData
    {
        public int levelBestData;
        public int monetsTotalData;
    }

    public void Save()
    {
        SaveData data = new SaveData();
        data.levelBestData = levelBest;
        data.monetsTotalData = monetsTotal;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            levelBest = data.levelBestData;
            monetsTotal = data.monetsTotalData;
        }
    }
}
