using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{

    private float floating_speed = 0.01f;
    private float max_lifetime = 2;
    private float fadeSpeed = 0.02f;

    public float lifetime = 0;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(0, floating_speed));
        lifetime += Time.deltaTime;

        if(lifetime > max_lifetime/2)
        {
            TextMesh tm = GetComponent<TextMesh>();

            tm.color = new Color(
                tm.color.r,
                tm.color.g,
                tm.color.b,
                tm.color.a -fadeSpeed
                );
        }
        if(lifetime > max_lifetime)
        {
            Destroy(this.gameObject);
        }
    }
    public void setText(string text)
    {
        GetComponent<TextMesh>().text = text;
    }
}
