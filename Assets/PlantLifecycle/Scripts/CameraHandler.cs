using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TMKOC.PlantLifecycle
{
    public class CameraHandler : MonoBehaviour   //only for orthographic cameras...
    {
        public CinemachineVirtualCamera virtualCamera;
        public float zoomDuration;
        public float targetSize;
        public Transform target;
        private float ogSize;

        private void Start()
        {
            ogSize = virtualCamera.m_Lens.OrthographicSize;
        }


        [Button]
        public void ZoomToPlant(Action callback)
        {
            var pos = new Vector3(target.transform.position.x, target.transform.position.y, -10);
            ZoomAndMove(targetSize, pos, callback);

        }

        [Button]
        public void ResetCamera(Action callback = null)
        {
            var pos = new Vector3(0, 0, -10);
            ZoomAndMove(ogSize, pos, callback);
        }

        //lets just call this 
        public void ZoomAndMove(float size, Vector3 _pos, Action callback = null)
        {
            virtualCamera.transform.DOMove(_pos, zoomDuration);   //move to target..
            StartCoroutine(ZoomOrthographic(size, zoomDuration, callback));  //zoom to size..
        }


        // Coroutine for 2D Orthographic Zoom
        private IEnumerator ZoomOrthographic(float targetSize, float duration, Action callback = null)
        {
            float startSize = virtualCamera.m_Lens.OrthographicSize;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // Smooth interpolation using Lerp
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);

                yield return null; // Wait for the next frame
            }

            // Ensure final zoom is exactly the target size
            virtualCamera.m_Lens.OrthographicSize = targetSize;
            callback?.Invoke();
        }




    }
}
