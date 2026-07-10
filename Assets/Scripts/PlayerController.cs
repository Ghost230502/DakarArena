using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 8f;
    public float attackRangeNormal = 1.5f;
    public float attackRangeSpecial = 3f;
    public float hp = 100f;
    public Slider hpSlider;
    public AudioClip sonPoing;
    public AudioClip sonEnchainement;
    public AudioClip sonSpecial;
    private AudioSource audioSource;
    private Animator animator;
    private Rigidbody rb;
    private bool isGrounded = true;
    private float specialCooldown = 0f;
    private bool enTrainDeParer = false;
    private bool combatTermine = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        MettreAJourBarreVie();
    }

    void Update()
    {
        if (combatTermine) return;

        float moveX = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * moveX * speed * Time.deltaTime;

        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(moveX));

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (specialCooldown > 0)
            specialCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attaquer(8f, "ATTAQUE LEGERE", sonPoing, attackRangeNormal);
            if (animator != null) animator.SetTrigger("Poing");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Attaquer(15f, "ATTAQUE LOURDE", sonEnchainement, attackRangeNormal);
            if (animator != null) animator.SetTrigger("Enchainement");
        }

        if (Input.GetKeyDown(KeyCode.L) && specialCooldown <= 0)
        {
            Attaquer(25f, "COUP SPECIAL", sonSpecial, attackRangeSpecial);
            if (animator != null) animator.SetTrigger("Special");
            specialCooldown = 10f;
        }

        if (Input.GetKey(KeyCode.I))
        {
            enTrainDeParer = true;
            if (animator != null) animator.SetBool("EnParade", true);
        }
        else
        {
            enTrainDeParer = false;
            if (animator != null) animator.SetBool("EnParade", false);
        }

        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;
    }

    public void RecevoirDegats(float degats)
    {
        if (combatTermine) return;

        if (enTrainDeParer)
            degats *= 0.3f;

        hp -= degats;
        MettreAJourBarreVie();

        if (hp <= 0)
        {
            Debug.Log("DJIBRIL EST KO !");
            CombatManager cm = FindAnyObjectByType<CombatManager>();
            if (cm != null)
                cm.IAGagne();
        }
    }

    void MettreAJourBarreVie()
    {
        if (hpSlider == null) return;
        hpSlider.value = hp;

        Image fillImage = hpSlider.fillRect.GetComponent<Image>();
        if (fillImage != null)
        {
            if (hp > 60)
                fillImage.color = Color.green;
            else if (hp > 30)
                fillImage.color = new Color(1f, 0.6f, 0f);
            else
                fillImage.color = Color.red;
        }
    }

    public void ArreterCombat()
    {
        combatTermine = true;
    }

    void Attaquer(float degats, string nomAttaque, AudioClip son, float range)
    {
        if (combatTermine) return;

        Debug.Log(nomAttaque);

        if (son != null && audioSource != null)
            audioSource.PlayOneShot(son);

        if (nomAttaque == "COUP SPECIAL")
            StartCoroutine(FlashCouleur());

        GameObject ennemi = GameObject.FindWithTag("Enemy");
        if (ennemi != null)
        {
            float distance = Vector3.Distance(
                transform.position, ennemi.transform.position);

            if (distance <= range)
            {
                EnnemiStats stats = ennemi.GetComponent<EnnemiStats>();
                if (stats != null)
                    stats.RecevoirDegats(degats);
            }
        }
    }

    System.Collections.IEnumerator FlashCouleur()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend == null) yield break;

        Color couleurOriginale = rend.material.color;
        rend.material.color = Color.yellow;
        yield return new WaitForSeconds(0.3f);
        rend.material.color = couleurOriginale;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
}