using UnityEngine;
using UnityEngine.UI;


namespace cyb
{


    public class CardFlip : MonoBehaviour
    {
        
        private bool isFlipping = false;
        private Quaternion targetRotation;
        private Quaternion startRotation;
        private float flipProgress = 0f;
        public float flipDuration = 0.5f;
        public float flipSpeed = 500f;

        void Start()
        {
            startRotation = transform.rotation;
            targetRotation = startRotation;
        }

        void Update()
        {
            if (isFlipping)
            {
                // Increase the flip progress over time
                flipProgress += Time.deltaTime / flipDuration;

                //  rotating using Lerp
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, flipProgress);

                // Check if the flip is complete
                if (flipProgress >= 1f)
                {
                    isFlipping = false; // Stop flipping
                    flipProgress = 0f;  // Reset progress for future flips
                    
                }
            }
        }

        // Function to rotate 
        public void RotateTo180()
        {
            if (!isFlipping)
            {
                targetRotation = Quaternion.Euler(0, 180, 0);
                isFlipping = true;
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            GameplayManager.instance.CheckFruits (gameObject.transform.GetChild(1).name, gameObject); //call for check  or ADD in list
            gameObject.GetComponent<Button>().enabled = false;
           
        }

        // Function to rotate back 
        public void RotateBackTo0()
        {
            if (!isFlipping)
            {
                targetRotation = startRotation;
                isFlipping = true;
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            gameObject.GetComponent<Button>().enabled = true;

        }
    }
}