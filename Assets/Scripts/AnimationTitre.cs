using UnityEngine;

public class AnimationTitre : MonoBehaviour
{
    public float vitesse = 2f;
    public float amplitude = 0.05f;
    private Vector3 tailleOriginale;

    void Start()
    {
        tailleOriginale = transform.localScale;
    }

    void Update()
    {
        float scale = 1f + Mathf.Sin(Time.time * vitesse) * amplitude;
        transform.localScale = tailleOriginale * scale;
    }
}