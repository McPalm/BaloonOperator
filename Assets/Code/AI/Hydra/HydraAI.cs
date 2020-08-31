using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraAI : NetworkBehaviour
{
    [SyncVar]
    NetworkIdentity Target;
    [SyncVar]
    int attackPattern;
    [SyncVar]
    public bool enraged;

    float EF => enraged ? .5f : 1f;


    public bool Attacking { get; private set; } = false;

    public AnimationCurve attackCurve;
    public AnimationCurve retractCurve;

    public Sprite NeutralSprite;
    public Sprite AttackSprite;
    public Sprite StunnedSprite;

    public SpriteRenderer SpriteRenderer;

    public GameObject DamageSource;

    public LayerMask Solid;

    public ProjectileSpawner ProjectileSpawner;

    Coroutine activeLoop;

    Vector3 home;


    private void Start()
    {
        home = transform.position;
        transform.position = transform.position + Vector3.down * 5f;
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        activeLoop = StartCoroutine(LocalLoop());
    }

    private void OnDisable()
    {
        StopCoroutine(activeLoop);
        Attacking = false;
        SpriteRenderer.sprite = NeutralSprite;
        attackPattern = 0;
    }

    public void SetAttack(GameObject target, int type)
    {
        Target = target.GetComponent<NetworkIdentity>();
        attackPattern = type;
    }

    public void UnAttack() => attackPattern = 0;

    IEnumerator LocalLoop()
    {
        yield return null;
        Vector3 wiggleAnchor = home;
        float wiggleTimer = 0f;
        for(; ; )
        {
            switch(attackPattern)
            {
                case 1: // head charge
                    if (Target)
                    {
                        Attacking = true;
                        SpriteRenderer.sprite = AttackSprite;
                        Vector3 end = Target.transform.position;
                        yield return new WaitForSeconds(.5f * EF); // telegraph
                        Vector3 start = transform.position;
                        if (Target)
                            end = Target.transform.position;
                        DamageSource.SetActive(true);

                        var distance = Vector2.Distance(start, end);
                        yield return LerpTo(getChargePoint(end), 6f / EF + distance * .3f, attackCurve);
                        SpriteRenderer.sprite = StunnedSprite;
                        yield return new WaitForSeconds(.1f);
                        DamageSource.SetActive(false);
                        yield return new WaitForSeconds(2.9f * EF);
                        SpriteRenderer.sprite = NeutralSprite;
                        var to = Vector3.Lerp(transform.position + new Vector3(0f, 6f), home, .25f);
                        yield return LerpTo(to, 4f, retractCurve);
                        Attacking = false;
                        wiggleAnchor = transform.position;
                        wiggleTimer = 0f;
                    }
                    break;
                case 2: // fire attack
                    Attacking = true;
                    ProjectileSpawner.Target = Target.transform;
                    SpriteRenderer.sprite = AttackSprite;
                    yield return WiggleWait(.5f * EF);
                    ProjectileSpawner.SpawnProjectile();
                    yield return WiggleWait(.2f * EF);
                    ProjectileSpawner.SpawnProjectile();
                    yield return WiggleWait(.2f * EF);
                    ProjectileSpawner.SpawnProjectile();
                    yield return WiggleWait(.2f * EF);
                    if(enraged)
                    {
                        ProjectileSpawner.SpawnProjectile();
                        yield return WiggleWait(.2f * EF);
                        ProjectileSpawner.SpawnProjectile();
                        yield return WiggleWait(.2f * EF);
                        ProjectileSpawner.SpawnProjectile();
                        yield return WiggleWait(.2f * EF);
                        ProjectileSpawner.SpawnProjectile();
                        yield return WiggleWait(.2f * EF);
                        ProjectileSpawner.SpawnProjectile();
                        yield return WiggleWait(.2f * EF);
                        ProjectileSpawner.SpawnProjectile();
                        yield return WiggleWait(.2f * EF);
                    }
                    SpriteRenderer.sprite = NeutralSprite;
                    Attacking = false;
                    break;
            }
            Wiggle();
            yield return null;
        }

        IEnumerator WiggleWait(float time)
        {
            for (float f = 0; f < time; f += Time.deltaTime)
            {
                Wiggle();
                yield return null;
            }
        }

        void Wiggle()
        {
            transform.position = wiggleAnchor + new Vector3(Mathf.Sin(wiggleTimer * 4f), 0f);
            wiggleTimer += Time.deltaTime;
            wiggleAnchor = Vector3.Lerp(wiggleAnchor, home, .2f * Time.deltaTime);
        }
    }

    IEnumerator LerpTo(Vector3 destination, float speed, AnimationCurve curve)
    {
        var start = transform.position;
        float distance = (start - destination).magnitude;
        for (float f = 0; f < distance; f += Time.deltaTime * speed)
        {
            var relative = f / distance;
            relative = curve.Evaluate(relative);
            transform.position = Vector3.LerpUnclamped(start, destination, relative);
            yield return null;
        }
        transform.position = destination;
    }

    Vector3 getChargePoint(Vector3 direction)
    {
        var hit = Physics2D.Raycast(transform.position, direction - transform.position, 100f, Solid);
        if (hit)
            return hit.point + Vector2.up;
        return direction;
    }

}
