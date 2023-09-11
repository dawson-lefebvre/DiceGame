using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBehaviour : MonoBehaviour
{
    public int DieValue { get; set; }
    [SerializeField] Sprite OneSprite;
    [SerializeField] Sprite TwoSprite;
    [SerializeField] Sprite ThreeSprite;
    [SerializeField] Sprite FourSprite;
    [SerializeField] Sprite FiveSprite;
    [SerializeField] Sprite SixSprite;
    Sprite[] Sprites = new Sprite[6];
    SpriteRenderer sr;
    bool rolling = false;
    int rollTimer = 0;
    public int maxRollFrames = 20, rollInterval = 2;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = OneSprite;
        Sprites[0] = OneSprite;
        Sprites[1] = TwoSprite;
        Sprites[2] = ThreeSprite;
        Sprites[3] = FourSprite;
        Sprites[4] = FiveSprite;
        Sprites[5] = SixSprite;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rolling)
        {
            if(rollTimer % rollInterval == 0)
            {
                DieValue = Random.Range(1, 7);
                sr.sprite = Sprites[DieValue - 1];
            }
            rollTimer--;
            if(rollTimer == 0)
            {
                rolling = false;
            }
        }
    }

    public void Roll()
    {
        rolling = true;
        rollTimer = maxRollFrames;
    }
}