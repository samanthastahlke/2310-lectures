//(c) Samantha Stahlke 2020
//Created for INFR 2310.
//Please don't fire me.
//Also, yes, this code is incredibly janky.
//We did it for the meme.
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ColinsDoctor : MonoBehaviour
{
    //Obligatory PSA: This code is SUPER MESSY and doesn't take
    //advantage of Unity's transform animators.
    //This is just to get stuff working as quickly as possible,
    //and sticking to vanilla poking-at-the-Inspector to tweak things.
    public SkinnedMeshRenderer drHogueRenderer;
    public Image background;
    public Text textName;
    public Text textCredits;

    //Phase 1: Walk (Orangecat)
    public float walkTimestamp = 1.0f;
    public Transform walkStart;
    public Transform walkEnd;

    //Phase 2: Dancing (Vibing)
    public float danceTimestamp = 3.0f;
    public Transform dancePos;

    //Phase 3: Hula (Hoguealicious)
    public float hulaTimestamp = 5.0f;
    public Transform hulaPos;

    //Phase 4: Poggers
    public float pogTimestamp = 8.0f;
    public Sprite pogBG;
    public float pogSwitch = 0.5f;
    public Transform pogLeft;
    public Transform pogRight;

    //Phase 5: Dr. Hogue has entered the exosphere
    public float spaceTimestamp = 10.5f;
    public Sprite spaceBG;
    public float spaceSwitch = 0.3f;
    public Transform space1;  
    public Transform space2;
    public Transform space3;

    public float endTimestamp = 11.5f;

    private Animator anim;
    private ColinState state;
    private float timer = 0.0f;

    //We're basically doing a terrible horrible FSM for this.
    enum ColinState
    {
        INVISIBLE,
        WALK,
        DANCE,
        HULA,
        POG,
        SPACE,
        END
    };

    void Start()
    {
        anim = GetComponent<Animator>();
        state = ColinState.INVISIBLE;
        drHogueRenderer.enabled = false;
        background.enabled = false;
        textName.enabled = false;
        textCredits.enabled = false;      
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch(state)
        {
            case ColinState.INVISIBLE:

                if (timer >= walkTimestamp)
                    ChangeState(ColinState.WALK);
                break;

            case ColinState.WALK:
           
                if (timer >= danceTimestamp)
                {
                    ChangeState(ColinState.DANCE);
                    break;
                }

                float localTime = timer - walkTimestamp;
                float localTotal = danceTimestamp - walkTimestamp;
                float t = localTime / localTotal;

                transform.position = Vector3.Lerp(walkStart.position,
                    walkEnd.position, t);
                
                break;

            case ColinState.DANCE:

                if (timer >= hulaTimestamp)
                {
                    ChangeState(ColinState.HULA);
                    break;
                }
                break;

            case ColinState.HULA:

                if (timer >= pogTimestamp)
                {
                    ChangeState(ColinState.POG);
                    break;
                }
                break;

            case ColinState.POG:

                if (timer >= spaceTimestamp)
                {
                    ChangeState(ColinState.SPACE);
                    break;
                }

                int pogIndex = (int)((timer - pogTimestamp) / pogSwitch);
                Transform pogPose = (pogIndex == 1) ? pogLeft : pogRight;

                transform.position = pogPose.position;
                transform.rotation = pogPose.rotation;

                break;

            case ColinState.SPACE:

                if (timer >= endTimestamp)
                {
                    ChangeState(ColinState.END);
                    break;
                }

                int spaceIndex = (int)((timer - spaceTimestamp) / spaceSwitch);
                Transform spacePose = (spaceIndex == 0) ? space1 
                    : (spaceIndex == 1) ? space2 : space3;

                transform.position = spacePose.position;
                transform.rotation = spacePose.rotation;

                break;

            default:
                break;
        }
    }

    void ChangeState(ColinState newState)
    {
        state = newState;

        switch (state)
        {
            case ColinState.INVISIBLE:

                break;

            case ColinState.WALK:

                drHogueRenderer.enabled = true;
                transform.position = walkStart.position;
                transform.LookAt(walkEnd.position);
                break;

            case ColinState.DANCE:

                anim.SetTrigger("vibecheck");
                transform.position = dancePos.position;
                transform.rotation = dancePos.rotation;
                break;

            case ColinState.HULA:

                background.enabled = true;
                anim.SetTrigger("biggerVibecheck");
                transform.position = hulaPos.position;
                transform.rotation = hulaPos.rotation;
                break;

            case ColinState.POG:

                anim.SetTrigger("poggers");
                background.sprite = pogBG;
                break;

            case ColinState.SPACE:

                background.sprite = spaceBG;
                break;

            case ColinState.END:

                drHogueRenderer.enabled = false;
                background.enabled = false;
                textName.enabled = true;
                textCredits.enabled = true;
                break;
        }
    }
}
