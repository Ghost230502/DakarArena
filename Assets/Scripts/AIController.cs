using UnityEngine;

public class AIController : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 2f;
    public float retreatDistance = 4f;
    private GameObject joueur;
    private EnnemiStats myStats;
    private Animator animator;
    private float attackCooldown = 0f;
    private float retreatTimer = 0f;

    enum EtatIA { Idle, Approach, Attack, Retreat }
    EtatIA etat = EtatIA.Idle;

    void Start()
    {
        joueur = GameObject.FindWithTag("Player");
        myStats = GetComponent<EnnemiStats>();
        animator = GetComponent<Animator>();
    }

    private float attackDuration = 0.8f;
    private float attackTimer = 0f;

    void Update()
    {
        if (joueur == null) return;
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
        if (retreatTimer > 0) retreatTimer -= Time.deltaTime;
        if (attackTimer > 0) attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(
            transform.position, joueur.transform.position);

        switch (etat)
        {
            case EtatIA.Idle:
                etat = EtatIA.Approach;
                break;

            case EtatIA.Approach:
                if (animator != null)
                    animator.SetFloat("Speed", 1f);

                if (distance <= attackRange)
                    etat = EtatIA.Attack;
                else
                    MoveToward(joueur.transform.position);
                break;

            case EtatIA.Attack:
                if (animator != null)
                    animator.SetFloat("Speed", 0f);

                if (attackCooldown <= 0 && attackTimer <= 0)
                {
                    Attaquer();
                    attackCooldown = 1.5f;
                    attackTimer = attackDuration;
                }

                if (attackTimer <= 0)
                {
                    retreatTimer = 1f;
                    etat = EtatIA.Retreat;
                }
                break;

            case EtatIA.Retreat:
                if (animator != null)
                    animator.SetFloat("Speed", 1f);

                if (retreatTimer <= 0)
                    etat = EtatIA.Approach;
                else
                    MoveAway(joueur.transform.position);
                break;
        }

        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;
    }

    void MoveToward(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        dir.y = 0;
        dir.z = 0;
        transform.position += dir * speed * Time.deltaTime;
    }

    void MoveAway(Vector3 target)
    {
        Vector3 dir = (transform.position - target).normalized;
        dir.y = 0;
        dir.z = 0;
        transform.position += dir * speed * Time.deltaTime;
    }

    void Attaquer()
    {
        float degats = Random.Range(0, 3) == 0 ? 15f : 8f;
        Debug.Log("IA attaque ! -" + degats + " HP");

        if (animator != null)
            animator.SetTrigger("Poing");

        PlayerController joueurStats = joueur.GetComponent<PlayerController>();
        if (joueurStats != null)
            joueurStats.RecevoirDegats(degats);
    }
}