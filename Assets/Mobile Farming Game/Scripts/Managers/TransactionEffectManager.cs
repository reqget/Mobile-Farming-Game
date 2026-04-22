using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionEffectManager : MonoBehaviour
{
    public static TransactionEffectManager instance;

    [Header("Elements")]
    [SerializeField] private ParticleSystem coinPS;
    [SerializeField] private RectTransform coinRectTransform;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    private int coinAmount;
    private Camera Camera;

    private void Start()
    {
            Camera = Camera.main;
    }

    private void Awake()
    {
            if (instance == null)
            instance = this;
            else
            Destroy(gameObject);
    }

    [NaughtyAttributes.Button]
    private void PlayCoinParticlesTest()
    {
        PlayCoinParticles(100);
    }
    public void PlayCoinParticles(int amount)
    {
        if(coinPS.isPlaying)
            return;

        ParticleSystem.Burst burst = coinPS.emission.GetBurst(0);
        burst.count = amount;
        coinPS.emission.SetBurst(0, burst);

        coinAmount = amount;

        ParticleSystem.MainModule main = coinPS.main;
        main.gravityModifier = 2;

        coinPS.Play();

        StartCoroutine(PlayCoinParticlesCoroutine());
    }

    IEnumerator PlayCoinParticlesCoroutine()
    {
        yield return new WaitForSeconds(1);

        ParticleSystem.MainModule main = coinPS.main;
        main.gravityModifier = 0;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[coinAmount];
        coinPS.GetParticles(particles);

        Vector3 direction = (coinRectTransform.position - Camera.transform.position).normalized;

        while (coinPS.isPlaying)
        {
            coinPS.GetParticles(particles);

             Vector3 targetPosition = Camera.transform.position+direction * Vector3.Distance(Camera.transform.position,coinPS.transform.position);
            for (int i = 0; i < particles.Length; i++)
            {
               if(particles[i].remainingLifetime <= 0)
                    continue;
                    
            

                particles[i].position = Vector3.MoveTowards(particles[i].position, targetPosition, moveSpeed* Time.deltaTime);

                if (Vector3.Distance(particles[i].position, targetPosition) < 0.1f) 
                {
                    particles[i].position += Vector3.up * 100000;
                    //particles[i].remainingLifetime = 0;
                    CashManager.instance.AddCoins(1);
                }
            }
            coinPS.SetParticles(particles);

            yield return null;

        }

       
    }
}
