using UnityEngine;
using UnityEngine.UI;

namespace cyb
{
    public class GameplayManager : MonoBehaviour
    {
       public static GameplayManager instance;
        [Header("Game Mech")]
        public GameObject PlayArea;
        public GameObject CardPrefab;
        public GameObject[] Sprites;

        public GameObject[] InstCards = new GameObject[12];
   
        void Start()
        {
            instance = this;
            
            
        }

        void Update()
        {
          
        }

       public void placeCards()
        {
            for(int i = 0; i < InstCards.Length; i++)
            {
                InstCards[i] = Instantiate(CardPrefab, PlayArea.transform);
                CardFlip cardFlip = InstCards[i].AddComponent<CardFlip>(); 
                InstCards[i].GetComponent<Button>().onClick.AddListener(cardFlip.RotateTo180);


            }
        }
       
    }
}