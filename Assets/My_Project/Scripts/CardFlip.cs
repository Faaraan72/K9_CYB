using UnityEngine;

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

                // Perform the rotation using Lerp
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, flipProgress);

                // Check if the flip is complete
                if (flipProgress >= 1f)
                {
                    isFlipping = false; // Stop flipping
                    flipProgress = 0f;  // Reset progress for future flips
                }
            }
        }

        // Function to rotate to 180 degrees on Y-axis
        public void RotateTo180()
        {
            if (!isFlipping)
            {
                targetRotation = Quaternion.Euler(0, 180, 0);
                isFlipping = true;
            }
        }

        // Function to rotate back to 0 degrees on Y-axis
        public void RotateBackTo0()
        {
            if (!isFlipping)
            {
                targetRotation = startRotation;
                isFlipping = true;
            }
        }
    }
}