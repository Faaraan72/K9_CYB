using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

namespace cyb
{
    public class GameplayManager : MonoBehaviour
    {
       public static GameplayManager instance; //static instance 
        [Header("Game Mech")]
        public GameObject PlayArea;
        public GameObject CardPrefab;
        public Sprite[] Sprites;        //total availabe Sprites can be added
        public List<string> FruitsList; //to maintan previous and curr selected name
        public List<GameObject> CardsList; //to maintan previous and curr selected card
        private GridLayoutGroup grid;
        public GameObject[] InstCards;  //keeps track of all Instantiated Cards
        [Header("Game Settings")]
        public int numberofcards;  //Setting for Game 
        public int numberofSprites;
        [Header("Audio")]
        public AudioClip flipAudio;
        void Start()
        {
            instance = this;
            InstCards = new GameObject[numberofcards];
            grid = PlayArea.GetComponent<GridLayoutGroup>();
            if (numberofcards > 12 && numberofcards <=16)
            {
                grid.cellSize =  new Vector2(75f,75f);
                grid.spacing = new Vector2(100f, 100f);
            }
           
            numberofSprites = Convert.ToInt32( numberofcards / 2); //number of sprites should be half of total cards(to make all pairs)
        }

      
    //Place Cards
       public void placeCards()
        {
            for(int i = 0; i < InstCards.Length; i++)
            {
                InstCards[i] = Instantiate(CardPrefab, PlayArea.transform);
                CardFlip cardFlip = InstCards[i].AddComponent<CardFlip>(); 
                InstCards[i].GetComponent<Button>().onClick.AddListener(cardFlip.RotateTo180);
                clickaudio c =InstCards[i].AddComponent<clickaudio>();
                c.tapSound = flipAudio;

            }
            PlaceRandomFruit();
            
        }

        //set Random Fruit Images 
      public void PlaceRandomFruit()
        {
            //initializing Index for Sprites 
            int[] SpritesIndex = new int[numberofSprites];
            for(int i = 0; i < numberofSprites; i++)
            {
                SpritesIndex[i] = i;
            }
            //initializing Index for Cards 
            int[] CardsIndex = new int[numberofcards];
            for(int i = 0; i < numberofcards; i++)
            {
                CardsIndex[i] = i;
            }
            //Randomizing these Array so we get Random positions
            int[] SelectedSprites = SelectRandomndexs(SpritesIndex, numberofSprites);
            int[] RandomCardsIndex = SelectRandomndexs(CardsIndex, numberofcards);

            //check
            for(int i = 0; i < SelectedSprites.Length; i++)
            {
                Debug.Log("Sprite::"+SelectedSprites[i]);
            }
            for (int i = 0; i < RandomCardsIndex.Length; i++)
            {
                Debug.Log("Cards::" + RandomCardsIndex[i]);
            }

            //for every Random Card Index,  place the random Sprite
            int j = 0;
            foreach(int i in RandomCardsIndex) {
                if (j > numberofSprites-1)
                {
                    j = 0;
                }
                InstCards[i].transform.GetChild(1).GetComponent<Image>().sprite = Sprites[SelectedSprites[j]];
                InstCards[i].transform.GetChild(1).name = Sprites[SelectedSprites[j]].name; //set the name of card same as the sprite so we can compare it
               j++;
                
            }
        }

        // Logic is Implemented as per the document: Not to wait for matching ,just keep on matching cards as the user clicks it
      public bool CheckFruits(string str, GameObject g)
        {
            // If there is already one item in the list, compare it with the new one
            if (FruitsList.Count == 1 && CardsList.Count == 1)
            {
                string previousName = FruitsList[0];
                GameObject previousCard = CardsList[0];

                if (previousName == str)
                {
                    // If the names match, remove both Cards and clear both lists
                    Destroy(g,1f);               // Destroy the current Card
                    Destroy(previousCard,1f);    // Destroy the previous Card
                    
                    FruitsList.Clear();       // Clear the FruitsList
                    CardsList.Clear();        // Clear the CardsList
                    //Debug.Log("Both cards match, deleted both and cleared lists.");

                    return true;              // true since both matched
                }
                else
                {
                    // If the names don't match, remove the previous one
                    FruitsList.RemoveAt(0);
                    CardsList.RemoveAt(0);
                    Debug.Log("Removed previous card: " + previousCard.transform.GetChild(1).name);
                    StartCoroutine(flipback(previousCard));
                   
                }
            }

            // Add the new name and card to the lists
            FruitsList.Add(str);
            CardsList.Add(g);
            Debug.Log("Added: " + str);
            Debug.Log("Added card: " + g.transform.GetChild(1).name);

            return false;  //false since cards didn't match
        }

      IEnumerator flipback(GameObject g)
        {
            yield return new WaitForSeconds(0.5f);
            g.GetComponent<CardFlip>().RotateBackTo0();
        }


        //Random selection of n elements in array
      public int[] SelectRandomndexs(int[] array, int numberOfElements)
        {
           

            int[] selectedElements = new int[numberOfElements];
            
            bool[] selectedIndices = new bool[array.Length]; 

            int selectedCount = 0;
            while (selectedCount < numberOfElements)
            {
                int randomIndex = UnityEngine.Random.Range(0, array.Length);

                if (!selectedIndices[randomIndex])
                {
                    selectedElements[selectedCount] = array[randomIndex];
                    selectedIndices[randomIndex] = true; 
                    selectedCount++;
                }
            }

            return selectedElements;
        }
    }
}