using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DieBehaviour : MonoBehaviour
{
    //Die value property
    public int DieValue { get; set; }
    //Dice sprites and array
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

    //Frames for rolling "animation"
    public int maxRollFrames = 20, rollInterval = 2;
    public bool locked;

    // Start is called before the first frame update
    void Start()
    {
        //Get sprite renderer and populate sprite array
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
        if (rolling)
        {
            //Changes sprite every rollInterval frames and sets rolling to false once timer frames reache 0
            if (rollTimer % rollInterval == 0)
            {
                DieValue = Random.Range(1, 7); //Gets random number and sets sprite to that number
                sr.sprite = Sprites[DieValue - 1];
            }
            rollTimer--;
            if (rollTimer == 0)
            {
                rolling = false;
                Debug.Log(DieValue);
            }
        }
    }

    public void Roll()
    {
        //Sets roll to true and resets timer to maxFrames
        rolling = true;
        rollTimer = maxRollFrames;
    }

    [SerializeField] TextMeshProUGUI buttonText;
    public void LockUnlock()
    {
        //Locks and changes button text
        if (locked) { locked = false; buttonText.text = "LOCK IN"; }
        else { locked = true; buttonText.text = "UNLOCK"; }
    }

    public void SetValue(int value)
    {
        //Sets dice value and sprite
        DieValue = value;
        sr.sprite = Sprites[DieValue - 1];
    }
}
