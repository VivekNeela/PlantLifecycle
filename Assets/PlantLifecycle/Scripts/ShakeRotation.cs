using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMKOC.PlantLifecycle
{
    public class ShakeRotation : MonoBehaviour   //put on any object you want to rotate and call 
    {
        private Quaternion originalRotation;
        [SerializeField] private int numberOfRotations;
        [SerializeField] private int rotationAngle;
        [SerializeField] private int rotationSpeed;
        [SerializeField] private bool rotating;
        public bool Rotating { get => rotating; set => rotating = value; }

        void Start() => originalRotation = transform.rotation;

        public void ShakeRotate() => StartCoroutine(RotateLeftAndRight());

        private IEnumerator RotateLeftAndRight()
        {
            for (int i = 0; i < numberOfRotations; i++)
            {
                // Rotate to the right
                yield return RotateToAngle(rotationAngle, rotationSpeed);

                // Rotate to the left
                yield return RotateToAngle(-rotationAngle, rotationSpeed);
            }

            // Return to the original rotation
            yield return RotateToAngle(0, rotationSpeed);
        }

        private IEnumerator RotateToAngle(float targetAngle, float speed)
        {
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0, 0, targetAngle);

            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
            {
                // Smoothly rotate towards the target rotation
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime * 100);
                Rotating = true;
                yield return null;
            }

            // Ensure the final rotation is exact
            transform.rotation = targetRotation;
            Rotating = false;
        }





    }
}
