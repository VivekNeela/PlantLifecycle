using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TMKOC.PlantLifecycle
{
    public class PlantAnimationHandler : MonoBehaviour
    {

        public float scale;

        public void ScalePlant(float duration)
        {
            transform.DOScale(scale, duration);
            
        }

    }
}
