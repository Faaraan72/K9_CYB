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
        public GameObject HomePanel;
        public GameObject MenuPanel;

        void Start()
        {
            HomePanel.SetActive(true);
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
            HomePanel.SetActive(false);
            GameplayManager.instance.placeCards();
        }
        void OpenMenu()
        {
            MenuPanel.SetActive(true);
           
        }
        void ExitGame()
        {
            GamePanel.SetActive(true);
            HomePanel.SetActive(false);
        }
        void closeMenu()
        {
            MenuPanel.SetActive(false);
        }
    }
}