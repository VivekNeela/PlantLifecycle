using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TMKOC.PlantLifecycle
{
    public class SaveSprite : MonoBehaviour
    {

        // public Sprite sprite;
        public SpriteRenderer spriteRenderer;
        public Texture2D modifiedTexture;
        public Color color;


        [Button]
        void ChangeSpriteColor()
        {
            Texture2D originalTexture = spriteRenderer.sprite.texture;
            modifiedTexture = new Texture2D(originalTexture.width, originalTexture.height);

            Color[] pixels = originalTexture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color(color.r, color.g, color.b, pixels[i].a);
                // pixels[i] = color;
            }

            modifiedTexture.SetPixels(pixels);
            modifiedTexture.Apply();

            Sprite newSprite = Sprite.Create(modifiedTexture, spriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f));
            spriteRenderer.sprite = newSprite;
        }



        [Button]
        void SaveSpriteAsPNG()
        {
            byte[] bytes = modifiedTexture.EncodeToPNG();
            string filePath = Path.Combine(Application.persistentDataPath, "hello.png");
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("Saved sprite to: " + filePath);
        }
    }
}
