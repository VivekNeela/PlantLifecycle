using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
// using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMKOC.PlantLifecycle
{
    public class OnTapGasoline : OnTapObject
    {

        public ShakeRotation shakeRotation;

        public override void OnMouseDown()
        {
            //answer is wrong so just doshake and play wrong answer audio....
            if (shakeRotation.Rotating) return;
            shakeRotation.ShakeRotate();
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
        }






    }
}
