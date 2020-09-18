//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

//You don't have to worry about this script too much.
//This is just a utility I created to expose some functionality
//that Unity normally does not, so that we could play with our own FSM design.
[RequireComponent(typeof(SpriteRenderer))]
public class SimpleSpriteAnimator : MonoBehaviour
{
    public struct Clip
    {
        public int startIndex;
    }

    public SpriteRenderer spriteRenderer { get; set; }

    //Unity fights you every step of the way to do this the "low-level" way.
    //The only reason I'm doing this with a public list of sprites is to be able to
    //show you simple animations decoupled from Unity's Animator system.
    //We'll be playing with Unity's system in a later lecture.
    public Sprite[] sprites;

    public int framerate = 12;
    private float frametime;

    //How many frames each animation is in our sprite array.
    public int animLength = 10;

    private Clip[] clips;
    private Clip current;

    private bool loop = false;
    private bool isPlaying = false;
    private float timer = 0.0f;
    private int frame = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        int rows = sprites.Length / animLength;
        clips = new Clip[rows];

        for(int i = 0; i < rows; ++i)
        {
            clips[i] = new Clip { startIndex = i * animLength };
        }

        frametime = 1.0f / (float)framerate;
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        timer += Time.deltaTime;

        //Simple sprite frame update.
        while(timer > frametime)
        {
            ++frame;
            
            if(frame >= animLength)
            {
                if (!loop)
                {
                    isPlaying = false;
                    break;
                }

                frame = 0;
            }

            spriteRenderer.sprite = sprites[current.startIndex + frame];
            timer -= frametime;
        }
    }

    public void PlayLoop(int clipID)
    {
        loop = true;
        Play(clipID);
    }

    public void PlayOnce(int clipID)
    {
        loop = false;
        Play(clipID);
    }

    public bool IsDone()
    {
        return !isPlaying;
    }

    private void Play(int clipID)
    {
        clipID = Mathf.Clamp(clipID, 0, clips.Length - 1);
        current = clips[clipID];

        timer = 0.0f;
        frame = 0;
        isPlaying = true;
    }
}
