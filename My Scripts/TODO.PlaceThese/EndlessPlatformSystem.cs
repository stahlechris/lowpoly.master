using System.Collections;
using UnityEngine;

public class EndlessPlatformSystem : MonoBehaviour
{
    [SerializeField] GameObject NearestTwo;
    [SerializeField] GameObject NextNearestTwo;
    [SerializeField] GameObject FurthestTwo;
    [SerializeField] GameObject NextFurthestTwo;

    [SerializeField] NearestTwo NearestTwoScript;
    [SerializeField] FurthestTwo FurthestTwoScript;


    public bool CompletedDescent { get; set; }
    public bool CompletedMove { get; set; }
    public bool CompletedRise { get; set; }
    public bool CompletedReparenting { get; set; }

    public void Inform()
    {
        MovePlatforms();
    }

    void MovePlatforms()
    {
        StartCoroutine(SpawnPlatformsAheadOfPlayer());
    }
    IEnumerator SpawnPlatformsAheadOfPlayer()
    {
        Transform FurthestTwoTransform = FurthestTwo.transform;
        //1. Lower the FurthestTwo GO on the Y by -5
        StartCoroutine(LerpDescentOrAscent(false));
        yield return new WaitUntil(() => CompletedDescent);
        //* 2. Move the FurthestTwo GO +40m on the X
        FurthestTwoTransform.localPosition = new Vector3(FurthestTwoTransform.localPosition.x + 40,
                                                         FurthestTwoTransform.localPosition.y,
                                                         FurthestTwoTransform.localPosition.z);
        //* 3. Raise the FurthestTwo GO on the Y by +5
        StartCoroutine(LerpDescentOrAscent(true));
        yield return new WaitUntil(() => CompletedRise);

        //* 5. Swap Elements:
        //*  a. FurthestTwo GO => NearestTwo
        //*  b. NextFurthestTwo GO => FurthestTwo
        //*  c. NearestTWo GO => NextNearestTwo
        //*  d. NextNearestTwo => NextFurthestTwo
        ReparentPlatforms();
        yield return new WaitUntil(() => CompletedReparenting);

        ShiftTriggerColliders();
        ResetBoolFlags();
    }
    IEnumerator LerpDescentOrAscent(bool command)
    {
        float timeItTakesToLerp = 1.5f;
        float timeItTakesToLerpDown = 0.75f; //can't see it go down anyway...speed it up a lil bit bro
        Vector3 startingPosition = FurthestTwo.transform.localPosition;

        Vector3 endingAscentPosition = FurthestTwo.transform.localPosition + new Vector3(0, 5, 0);
        Vector3 endingDescentPosition = FurthestTwo.transform.localPosition + new Vector3(0, -5, 0);

        float elapsedTime = 0f;
        bool startedFX = false;

        //Relative to current position, lower or raise GO by |5| on the y
        if(command) //raise platform
        {
            while(elapsedTime < timeItTakesToLerp)
            {
                FurthestTwo.transform.localPosition = Vector3.Lerp(startingPosition, endingAscentPosition, (elapsedTime / timeItTakesToLerp));
                elapsedTime += Time.deltaTime;
                yield return null;
                if(!startedFX && elapsedTime > 1)
                {
                    startedFX = true;
                    FurthestTwo.GetComponent<FurthestTwo>().PlayFX();
                }
            }
            //snap it into place because Lerp will be off due to floating point precision.
            FurthestTwo.transform.localPosition = endingAscentPosition;
            CompletedRise = true;
        }
        else //lower platform
        {
            while (elapsedTime < timeItTakesToLerpDown)
            {
                FurthestTwo.transform.localPosition = Vector3.Lerp(startingPosition, endingDescentPosition, (elapsedTime / timeItTakesToLerpDown));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            //snap it into place because Lerp will be off due to floating point precision.
            FurthestTwo.transform.localPosition = endingDescentPosition;
            CompletedDescent = true;
        }
    }

    void ReparentPlatforms() 
    //Experiencing a bug where 3 platforms will be placed into FurthestTwo, 
    //reciprocally NearestTwo only has 1 platform
    {
        //Debug.Log("begin reparenting");

        //Cache-box 20 LOL IM FUNNY
        Transform[] nearestTwoElements = new Transform[2];
        Transform[] nextNearestTwoElements = new Transform[2];
        Transform[] nextFurthestTwoElements = new Transform[2];
        Transform[] furthestTwoElements = new Transform[2];


        //Loop through them to cache child transforms, and swap their parents for reuse
        for (int i = 0; i < 2;i++)
        {
            //Cache children, 0 because they are reparented so the next element will be 0
            nearestTwoElements[i] = NearestTwo.transform.GetChild(0);
            nextNearestTwoElements[i] = NextNearestTwo.transform.GetChild(0);
            nextFurthestTwoElements[i] = NextFurthestTwo.transform.GetChild(0);
            furthestTwoElements[i] = FurthestTwo.transform.GetChild(0);

            //Give the children new parents. (bc new mom and dad will let me play fortnite all night!)
            nearestTwoElements[i].SetParent(NextNearestTwo.transform);
            nextNearestTwoElements[i].SetParent(NextFurthestTwo.transform);
            nextFurthestTwoElements[i].SetParent(FurthestTwo.transform);
            furthestTwoElements[i].SetParent(NearestTwo.transform);
            //Now bc they physically shifted, the box colliders' positions need to move up by X
            //...and the ParticleHolder needs to shift back by -30
        }
        CompletedReparenting = true;

    }
    void ShiftTriggerColliders()
    {
        NearestTwoScript.Reset();
        FurthestTwoScript.Reset();
    }

    void ResetBoolFlags()
    {
        CompletedDescent = false;
        CompletedMove = false;
        CompletedRise = false;
        CompletedReparenting = false;
    }
}
