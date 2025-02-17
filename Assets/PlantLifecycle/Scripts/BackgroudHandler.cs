using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMKOC.PlantLifecycle
{
    public class BackgroudHandler : MonoBehaviour
    {
        public SpriteRenderer bgSprite;
        public SpriteRenderer aboveGroundSprite;


        void Start()
        {
            ScaleSprite(bgSprite);
            ScaleSprite(aboveGroundSprite);
        }


        private void ScaleSprite(SpriteRenderer sprite)
        {
            Camera mainCamera = Camera.main;
            float screenHeight = 2f * mainCamera.orthographicSize;
            float screenWidth = screenHeight * mainCamera.aspect;

            Vector3 newScale = transform.localScale;
            newScale.x = (screenWidth / sprite.bounds.size.x) / 2;
            newScale.y = (screenHeight / sprite.bounds.size.y) / 2;

            sprite.transform.localScale = newScale;
        }




    }
}
