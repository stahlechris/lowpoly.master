//using System.Collections;
//using UnityEngine;

//public class CameraShake : MonoBehaviour 
//{
//    public IEnumerator Shake(float duration, float magnitude)
//    {
//        Vector3 origin = transform.localPosition;

//        float elapsed = 0.0f;

//        while(elapsed < duration)
//        {
//            float x = Random.Range(-1f, 1f) * magnitude;
//            float y = Random.Range(-1f, 1f) * magnitude;

//            transform.localPosition = new Vector3(x, y, origin.z);

//            elapsed += Time.deltaTime;
//            //return null waits until end of frame
//            yield return null;
//        }
//        transform.localPosition = origin;
//    }
//}
