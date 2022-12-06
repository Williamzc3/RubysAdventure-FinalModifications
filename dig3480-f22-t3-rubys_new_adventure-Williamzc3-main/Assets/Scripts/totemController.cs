using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class totemController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    AudioSource audioSource;
    public ParticleSystem smokeEffect;
    public AudioClip Break;
    public bool Broken = false;
    private RubyController rubyController;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController");
        rubyController = rubyControllerObject.GetComponent<RubyController>();
        smokeEffect.Stop();
    }

    // Update is called once per frame
    /*void FixedUpdate()
    {
        if (!Broken)
        {
            return;
        }
    }*/

    public void Destroyed()
    {
        
        Broken = true;
        smokeEffect.Play();

        if (rubyController != null)
        {
            rubyController.DestroyedTotems(1);
        }
    }
}
