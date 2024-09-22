using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;

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
        public GridLayoutGroup grid;
        public GameObject[] InstCards;  //keeps track of all Instantiated Cards
        public static bool win;
        public int currlevel;

        [Header("Scoring")]
        public int pairsmade;
        public TextMeshProUGUI timerText;
        public int TotaltimeLeft=60; // To keep track of time left
        


        [Header("Game Settings")]
        public int numberofcards;  //Setting for Game 
        public int numberofSprites;
        [Header("Audio")]
        public AudioClip flipAudio;
        [Header("Animations")]
        public Transform Coin;
        void Start()
        {
            win = false;
            
            instance = this;

            grid = PlayArea.GetComponent<GridLayoutGroup>();
        
        }

      
    //Place Cards
       public void placeCards(int level)
        {
            InstCards = new GameObject[numberofcards];
            numberofSprites = Convert.ToInt32(numberofcards / 2); //number of sprites should be half of total cards(to make all pairs) 
            pairsmade = 0;
            currlevel = level;
            if(numberofcards > 12)
            {
                grid.spacing = new Vector2(100,100);
            }
            for(int i = 0; i < InstCards.Length; i++)
            {
                InstCards[i] = Instantiate(CardPrefab, PlayArea.transform);
                CardFlip cardFlip = InstCards[i].AddComponent<CardFlip>(); 
                InstCards[i].GetComponent<Button>().onClick.AddListener(cardFlip.RotateTo180);
                clickaudio c =InstCards[i].AddComponent<clickaudio>();
                InstCards[i].GetComponent<Button>().onClick.AddListener(c.PlayTapSound);
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
            StartCoroutine(IncrementTimer());
            
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
                   
                    StartCoroutine(TransitionAndDestroyCards(g, previousCard, Coin));
                    FruitsList.Clear();       // Clear the FruitsList
                    CardsList.Clear();        // Clear the CardsList
                          
                    
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

      private IEnumerator TransitionAndDestroyCards(GameObject card1, GameObject card2, Transform target)
        {
            float transitionDuration = 0.5f;  // Duration of the transition
            float elapsedTime = 0f;

            Vector3 startPosition1 = card1.transform.position;
            Vector3 startPosition2 = card2.transform.position;

            Vector3 startScale1 = card1.transform.localScale;
            Vector3 startScale2 = card2.transform.localScale;

            Vector3 targetScale = Vector3.zero;  // The target scale, we want them to shrink to zero

            while (elapsedTime < transitionDuration)
            {
                // Interpolate both cards' positions and scale towards the destination and smaller size
                card1.transform.position = Vector3.Lerp(startPosition1, target.position, elapsedTime / transitionDuration);
                card2.transform.position = Vector3.Lerp(startPosition2, target.position, elapsedTime / transitionDuration);

                // Interpolate the scale (shrinking the cards)
                card1.transform.localScale = Vector3.Lerp(startScale1, targetScale, elapsedTime / transitionDuration);
                card2.transform.localScale = Vector3.Lerp(startScale2, targetScale, elapsedTime / transitionDuration);

                elapsedTime += Time.deltaTime;
                yield return null;  // Wait for the next frame
            }

            // Ensure both cards reach the exact target position and scale at the end
            card1.transform.position = target.position;
            card2.transform.position = target.position;

            card1.transform.localScale = targetScale;  // Final scale to zero
            card2.transform.localScale = targetScale;  // Final scale to zero

            yield return new WaitForEndOfFrame();  // Wait for 1 second before destroying them
            pairsmade++;
            if (pairsmade >= numberofcards / 2)
            {

                Debug.Log("::::::::::"+PlayerPrefs.GetInt("level"));
                Debug.Log("::::::::::" + currlevel);

                if (PlayerPrefs.GetInt("level") == currlevel)
                {
                    PlayerPrefs.SetInt("level", currlevel + 1); // unlock the next level
                }
                win = true;
            }
            Destroy(card1);
            Destroy(card2);
           
        }


        private IEnumerator IncrementTimer()
        {
           

            while (TotaltimeLeft>=0) // Infinite loop to keep the timer running
            {
                yield return new WaitForSeconds(1f); 
                TotaltimeLeft -= 1; 
                timerText.text = TotaltimeLeft.ToString();
                if (TotaltimeLeft>30)
                {
                    timerText.color = Color.green;
                }
                else if(TotaltimeLeft < 30 && TotaltimeLeft>11)
                {
                    timerText.color= new Color(1f, 0.5f, 0f);
                }else if (TotaltimeLeft <= 10)
                {
                    timerText.color = Color.red;
                }
                else if(TotaltimeLeft <=0 && !win)
                {
                    win = false;
                }
            }
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