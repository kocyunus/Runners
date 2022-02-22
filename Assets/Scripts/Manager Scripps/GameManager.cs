using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    #region instance
    private static GameManager _myInstance;
    public static GameManager _MyInstance
    {
        get
        {
            return _myInstance;
        }
    }
    #endregion
    #region RankSystem
    private RealTimeLeaderboard _realTimeLeaderboard;
    GameObject[] _runners;
    List<RankingSystem> _sortArray = new List<RankingSystem>();
    public int pass;
    public string firstRunner, secondRunner, thirdRunner;
    #endregion
    #region LevelSystem
    public bool finish;
    private bool _win;
    public bool start;
    public int level;
    #endregion

    #region UI
    public GameObject nextLevelObj, againLevel;
    public GameObject gamePlayUI, FinishGameUI;
    public Image fill;
    public Sprite orange, gray;
    public Text currentLevel, nextLevel;
    public Text countText;
    #endregion

    #endregion
    #region Unity Funcs
    private void Awake()
    {
        if (_myInstance == null)
        {
            _myInstance = this;
        }

        _runners = GameObject.FindGameObjectsWithTag("Runner");
        _realTimeLeaderboard = FindObjectOfType<RealTimeLeaderboard>();
        StartCoroutine(GameStartCount());
    }

    private void Start()
    {
        finish = false;
        for (int i = 0; i < _runners.Length; i++)
        {
            _sortArray.Add(_runners[i].GetComponent<RankingSystem>());
        }
    }
    private void Update()
    {
        CalculatingRank();
    }

    #endregion
    #region Start Count
    IEnumerator GameStartCount()
    {
        countText.text = 3.ToString();
        yield return new WaitForSeconds(1);
        countText.color = Color.cyan;
        countText.text = 2.ToString();
        yield return new WaitForSeconds(1);
        countText.color = Color.green;
        countText.text = 1.ToString();
        yield return new WaitForSeconds(1);
        countText.color = Color.yellow;
        countText.text = "GO";
        start = true;

        yield return new WaitForSeconds(0.5f);

        countText.gameObject.SetActive(false);

    }
    #endregion
    #region RankingSystem
    void CalculatingRank()
    {
        _sortArray = _sortArray.OrderBy(t => t.counter).ToList();
        switch (_sortArray.Count)
        {
            case 3:
                _sortArray[0].rank = 3;
                _sortArray[1].rank = 2;
                _sortArray[2].rank = 1;

                _realTimeLeaderboard.runnerNamethirth = _sortArray[0].name;
                _realTimeLeaderboard.runnerNameSecond = _sortArray[1].name;
                _realTimeLeaderboard.runnerNameFirst = _sortArray[2].name;
                break;
            case 2:
                _sortArray[0].rank = 2;
                _sortArray[1].rank = 1;

                _realTimeLeaderboard.runnerNameSecond = _sortArray[0].name;
                _realTimeLeaderboard.runnerNameFirst = _sortArray[1].name;
                break;
            case 1:
                _sortArray[0].rank = 1;
                _realTimeLeaderboard.runnerNameFirst = _sortArray[0].name;
                if (firstRunner == "")
                {
                    firstRunner = _sortArray[0].name;
                    _win = true;
                    FinishLevel();
                }
                break;
            default:
                break;
        }

        if (pass >= (float)_runners.Length / 2)
        {
            pass = 0;
            _sortArray = _sortArray.OrderBy(x => x.counter).ToList();
            foreach (RankingSystem rs in _sortArray)
            {
                if (rs.rank == _sortArray.Count)
                {
                    rs.gameObject.SetActive(false);

                    if (rs.gameObject.name == "Player")
                    {
                        _win = false;
                        FinishLevel();
                    }

                    if (thirdRunner == "")
                    {
                        thirdRunner = rs.gameObject.name;
                    }
                    else if (secondRunner == "")
                    {
                        secondRunner = rs.gameObject.name;
                    }
                }
            }
            _runners = GameObject.FindGameObjectsWithTag("Runner");
            _sortArray.Clear();
            for (int i = 0; i < _runners.Length; i++)
            {
                _sortArray.Add(_runners[i].GetComponent<RankingSystem>());

            }

            if (_runners.Length < 2)
            {
                finish = true;
                if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("Level"))
                {
                    PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                }

            }
        }
    }

    #endregion
    #region LevelManagment and  UI progress
    void FinishLevel()
    {
        gamePlayUI.SetActive(false);
        FinishGameUI.SetActive(true);

        if (_win)
        {
            currentLevel.text = PlayerPrefs.GetInt("Level", 1).ToString();
            level = PlayerPrefs.GetInt("Level", level);
            nextLevel.text = PlayerPrefs.GetInt("Level", 1) + 1 + "";
            nextLevelObj.SetActive(true);
            againLevel.SetActive(false);
            fill.sprite = orange;

        }
        else
        {
            currentLevel.text = PlayerPrefs.GetInt("Level", 1) - 1 + "";
            nextLevel.text = PlayerPrefs.GetInt("Level", 1).ToString();
            nextLevelObj.SetActive(false);
            againLevel.SetActive(true);
            fill.sprite = gray;
        }
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel() 
    {
        SceneManager.LoadScene(level );
    }
    public void AgainLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
