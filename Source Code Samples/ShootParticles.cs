using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootParticles : MonoBehaviour
{
    private ParticleSystem particles;
    public Transform firingPoint;
    public int numberOfParticles = 10;
    public float spreadAngle = 20f, minSpeed = 7.5f, maxSpeed = 10.1f, spawnRadius = 0.05f;
    private bool fired = false;
    public AudioManager am;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !fired)
        {
            StartCoroutine(JustFired());
            
            am.Play("gun");

            particles.transform.position = firingPoint.position;

            ParticleSystem.ShapeModule shapeModule = particles.shape;
            shapeModule.shapeType = ParticleSystemShapeType.Box;
            shapeModule.scale = new Vector3(spawnRadius, spawnRadius, 0f);

            for (int i = 0; i < numberOfParticles; i++)
            {
                float randomSpread = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
                Quaternion spreadRotation = Quaternion.Euler(0f, 0f, randomSpread);
                float randomSpeed = Random.Range(minSpeed, maxSpeed);

                float randomSize = Random.Range(0.5f, 1f);
                Color randomColor = new(1f, Random.Range(.9f, .99f), Random.Range(.9f, .99f));

                ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
                {
                    velocity = spreadRotation * firingPoint.right * randomSpeed,
                    startSize = randomSize,
                    startColor = randomColor
                };

                particles.Emit(emitParams, 1);
            }

            particles.Play();
        }
    }

    private IEnumerator JustFired()
    {
        fired = true;
        yield return new WaitForSecondsRealtime(2.05f);
        fired = false;
        yield return null;
    }
}
