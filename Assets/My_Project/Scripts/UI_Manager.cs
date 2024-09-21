using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace cyb
{


    public class UI_Manager : MonoBehaviour
    {
        [Header("Buttons")]
        public Button PlayButton;
        public Button ExitButton;
        public Button MenuButton;
        public Button CloseMenuButton;

        [Header("Panels")]
        public GameObject GamePanel;
        public GameObject GameMenu;
        public GameObject SettingsPanel;

        void Start()
        {
            GameMenu.SetActive(true);
            SettingsPanel.SetActive(false);
            PlayButton.onClick.AddListener(StartGame);
            ExitButton.onClick.AddListener(ExitGame);
            MenuButton.onClick.AddListener(OpenMenu);
            CloseMenuButton.onClick.AddListener(closeMenu);

        }


        void Update()
        {

        }

         void StartGame()
        {
            GamePanel.SetActive(true);
            GameMenu.SetActive(false);
            GameplayManager.instance.placeCards();
        }
        void OpenMenu()
        {
            SettingsPanel.SetActive(true);
           
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

        }
    }
}