
using UnityEngine;
using System.Collections;

public class UVAnimation : MonoBehaviour
{
    //public Vector2 textureMatrix = Vector2.zero;
    public float timeBetweenFrames = 0.3f;
    public float minStartDelay = 0.05f;
    public float maxStartDelay = 0.5f;
    public bool animate = false;
 //   public float minAnimTime = 5.0f;
   // public float maxAnimTime = 10f;

    //private float _xOffset = 0f;
    //private float _yOffset = 0f;
  //  private float _timer = 0f;
    private Renderer _renderer = null;
	public bool animateCrowd;

    IEnumerator Start()
    {
        _renderer = GetComponent<Renderer>();
        float delay = Random.Range(minStartDelay, maxStartDelay);
        yield return new WaitForSeconds(delay);
        animate = true;
        //_xOffset = 1f / textureMatrix.x;
        //_yOffset = 1f / textureMatrix.y;

        StartCoroutine("Animate");
    }

    void Update()
    {
        if(animate == false && animateCrowd == true)
        {
            StartCoroutine("Animate");
            animate = true;
        }

    }

//    IEnumerator Animate()
//    {
//        float delay = Random.Range(minStartDelay, maxStartDelay);
//        yield return new WaitForSeconds(delay);
////        if (GameMechanics.Instance)
////            GameMechanics.Instance.EnableCrowdCameraFlashes(true);
//        while (_renderer && animateCrowd)
//        {
//            float x = 0f;
//            float y = 0f;
//            for (int i = 0; i < textureMatrix.y; i++)
//            {
//                for (int j = 0; j < textureMatrix.x; j++)
//                {
//                    _renderer.material.mainTextureOffset = new Vector2(x, y);
//                    yield return new WaitForSeconds(timeBetweenFrames);
//                    x += _xOffset;
//                    if (x >= 1) x = 0;
//                }

//                y -= _yOffset;
//                if (y <= -1) y = 0;
//            }
//        }
////        if (GameMechanics.Instance)
////            GameMechanics.Instance.EnableCrowdCameraFlashes(false);
//        animate = false;
//    }

    IEnumerator Animate()
    {
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0f));
        yield return new WaitForSeconds(timeBetweenFrames);
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(0f, 0f));
        yield return new WaitForSeconds(timeBetweenFrames);

        animate = false;
    }
}