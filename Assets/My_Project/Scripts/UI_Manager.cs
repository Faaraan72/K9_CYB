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

        [Header("Panels")]
        public GameObject GamePanel;
        public GameObject GameMenu;
        public GameObject SettingsPanel;
        public GameObject WinPanel;
        public GameObject GameOverPanel;

        [Header("Scoring")]
        public TextMeshProUGUI pairsmade;
        public TextMeshProUGUI totalscoretxt;
        public TextMeshProUGUI timeleft;
        public TextMeshProUGUI timetaken;
        void Start()
        {
            instance = this;
            GameMenu.SetActive(true);
            WinPanel.SetActive(false);
            GameOverPanel.SetActive(false);
            SettingsPanel.SetActive(false);

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
            }else if (!GameplayManager.win && GameplayManager.instance.Totaltime <=0)
            {
                OpenGameOverPanel();
            }
            else
            {
                UpdateScore();
            }
            
        }
       public void  UpdateScore()
        {
            timeleft.text = "" + GameplayManager.instance.Totaltime;
            totalscoretxt.text = "" + Convert.ToInt32(timeleft.text) * Convert.ToInt32(pairsmade.text);
            timetaken.text =""+ (60 - GameplayManager.instance.Totaltime);
        }
         void StartGame()
        {
            GamePanel.SetActive(true);
            GameMenu.SetActive(false);
            GameplayManager.instance.placeCards();
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
            GamePanel.SetActive(true);
            GameMenu.SetActive(false);
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