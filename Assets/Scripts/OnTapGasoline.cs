using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMKOC.PlantLifecycle
{
    public class OnTapGasoline : OnTapObject
    {
        public override void OnMouseDown()
        {
            //answer is wrong so just doshake and play wrong answer audio....
            transform.DOShakeRotation(1, 30, 2, 10);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
        }

    }
}
