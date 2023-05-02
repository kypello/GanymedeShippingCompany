using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{
    public GameObject starPrefab;
    public int starCount;
    public Transform starParent;

    /*
    void Start() {
        for (int i = 0; i < starCount; i++) {
            GameObject newStar = Instantiate(starPrefab, new Vector3(Random.Range(-80f, 80f), Random.Range(-40f, 40f), Random.Range(40f, 60f)), Quaternion.identity);
            newStar.transform.localScale = Vector2.one * Random.Range(0.2f, 1f);
            newStar.transform.SetParent(starParent);
        }
    }
    */

    /*
    void Start() {
        Color[] pixels = new Color[1000000];
        for (int y = 0; y < 1000; y++) {
            for (int x = 0; x < 1000; x++) {
                float perlinValue = Mathf.PerlinNoise(x / 2f, y / 2f);
                Color color = new Color(perlinValue, perlinValue, perlinValue, 1f);
                pixels[y * 1000 + x] = color;
            }
        }

        Texture2D texture = new Texture2D(1000, 1000);
        texture.SetPixels(pixels);
        
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/stars.png", bytes);
    }
    */
}
