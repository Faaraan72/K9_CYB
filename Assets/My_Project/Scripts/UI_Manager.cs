using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using System;

namespace cyb
{


    public class UI_Manager : MonoBehaviour
    {
        public static UI_Manager instance;
        [Header("Buttons")]
        public Button PlayButton;
        public Button ExitButton;
        public Button MenuButton;
        public Button CloseMenuButton;
        public Button[] HomeButtons;
        public Button[] Levels;

        [Header("Panels")]
        public GameObject GamePanel;
        public GameObject GameMenu;
        public GameObject SettingsPanel;
        public GameObject WinPanel;
        public GameObject GameOverPanel;
        public GameObject LevelPanel;

        [Header("Sprites")]
        public Sprite lockedlevel;
        public Sprite unlockedlevel;
        public Sprite playedLevel;

        [Header("Scoring")]
        public TextMeshProUGUI pairsmade;
        public TextMeshProUGUI totalscoretxt;
        public TextMeshProUGUI timeleft;
        public TextMeshProUGUI timetaken;
        [Header("Mech")]
        private int TotalTime=60;
        public int levelreached;
        private bool Gameovercalled = false;

        void Start()
        {
            Gameovercalled = false;
            instance = this;
            GameMenu.SetActive(true);
            WinPanel.SetActive(false);
            GameOverPanel.SetActive(false);
            SettingsPanel.SetActive(false);
            LevelPanel.SetActive(false);
            GameObject btns = LevelPanel.transform.GetChild(1).gameObject;
            
            for(int i = 0; i < btns.transform.childCount; i++)
            {
                Levels[i] = btns.transform.GetChild(i).GetComponent<Button>();

                levelreached = PlayerPrefs.GetInt("level");
                if (levelreached == 0)
                {
                    PlayerPrefs.SetInt("level", 1);
                    levelreached = PlayerPrefs.GetInt("level");
                }
               //since i is index it starts from 0 so we need to match  (levelreached-1)
                if (i < levelreached-1)
                {
                    Levels[i].transform.GetComponent<Image>().sprite = playedLevel;
                }else if(i == levelreached-1)
                {
                    Levels[i].transform.GetComponent<Image>().sprite = unlockedlevel;
                }
                else
                {
                    Levels[i].transform.GetComponent<Image>().sprite = lockedlevel;
                    Levels[i].enabled = false;
                }
                
            }
            PlayButton.onClick.AddListener(StartGame);
            ExitButton.onClick.AddListener(ExitGame);
            MenuButton.onClick.AddListener(OpenSettings);
            CloseMenuButton.onClick.AddListener(closeMenu);
            foreach(Button b in HomeButtons)
            {
                b.onClick.AddListener(restart);
            }
        }


        void Update()
        {
            pairsmade.text = "" + GameplayManager.instance.pairsmade;
            if (GameplayManager.win)
            {
                OpenWinPanel();
            }else if (!GameplayManager.win && GameplayManager.instance.TotaltimeLeft <=0 && !Gameovercalled)
            {
                Gameovercalled = true;
                GameplayManager.instance.audiosource.PlayOneShot(GameplayManager.instance.gameOver);
                OpenGameOverPanel();
            }
            else
            {
                UpdateScore();
            }
            
        }
       public void  UpdateScore()
        {
            timeleft.text = "" + GameplayManager.instance.TotaltimeLeft;
            totalscoretxt.text = "" + Convert.ToInt32(timeleft.text) * Convert.ToInt32(pairsmade.text);
            timetaken.text =""+ (TotalTime - GameplayManager.instance.TotaltimeLeft);
        }
         void StartGame()
        {
            // GamePanel.SetActive(true);
            LevelPanel.SetActive(true);
            GameMenu.SetActive(false);
            // GameplayManager.instance.placeCards();
            
        }

        //first level 1 -> 6 cards and 30 seconds
        //seconds level 2-> 8 cards and 60 seconds
        //third level 3-> 8 cards and 30 seconds
        //third level 4-> 10 cards and 60 seconds
        //third level 5-> 10 cards and 30 seconds

        // and so on
        public void SelectLevel(int level)
        {
            Debug.Log("level" + level);
            
            if (level % 2 == 0)
            {
                GameplayManager.instance.numberofcards = level + 6;
                GameplayManager.instance.TotaltimeLeft = 60;
                TotalTime = 60;
            }
            else
            {
                GameplayManager.instance.numberofcards = level + 5;
                GameplayManager.instance.TotaltimeLeft = 30;
                TotalTime = 30;

            }
            LevelPanel.SetActive(false);
            GamePanel.SetActive(true);
            
            GameplayManager.instance.placeCards( level);
        }
        void OpenSettings()
        {
            SettingsPanel.SetActive(true);
           
        }
        public void OpenWinPanel()
        {
            WinPanel.SetActive(true);
        }
        public void OpenGameOverPanel()
        {
            GameOverPanel.SetActive(true);
        }
        void ExitGame()
        {
            
                Debug.Log("Game is quitting...");
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;  // Stop playing in the Editor
#endif
            
        }
        void closeMenu()
        {
            SettingsPanel.SetActive(false);
        }

        void restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}