using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillChanger : MonoBehaviour
{
    public GameObject skill1;
    public GameObject skill2;
    public GameObject skill3;

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Sprite sprite5;
    public Sprite sprite6;

    public Button skillChanger;
    public GameObject bow;

    public static bool useBow;
    private Animator animator;
    private Timer timer;

    private PlayerData playerData;
    private Enemy targetEnemy;
    private int baseAttack = 10;
    private int skill1Damage;
    private int skill2Damage;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        playerData = FindObjectOfType<PlayerData>();
        animator = playerData.GetComponent<Animator>();
    }

    private void Start()
    {
        useBow = false;

        skill1.GetComponent<Image>().sprite = sprite1;
        skill2.GetComponent<Image>().sprite = sprite2;
        skill3.GetComponent<Image>().sprite = sprite3;
    }

    private void Update()
    {
        if (useBow)
        {
            skill1.GetComponent<Image>().sprite = sprite4;
            skill2.GetComponent<Image>().sprite = sprite5;
            skill3.GetComponent<Image>().sprite = sprite6;
        }
        else
        {
            skill1.GetComponent<Image>().sprite = sprite1;
            skill2.GetComponent<Image>().sprite = sprite2;
            skill3.GetComponent<Image>().sprite = sprite3;
        }
    }

    public void UseBowOnClick()
    {
        useBow = !useBow;
        if (useBow)
        {
            Animator animator = bow.GetComponent<Animator>();
            animator.SetTrigger("ActivateBow");
            StartCoroutine(PauseGameForSeconds(2));
        }
        else
        {
            bow.SetActive(false);
        }
    }

    private IEnumerator PauseGameForSeconds(float seconds)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1f;
    }

    public void Skill1SingleAttack()
    {
        if (!useBow && timer.currentTurnTime >= playerData.cooldown)
        {
            DisableSkillButtons();
            skill1Damage = baseAttack + playerData.attack;
            StartCoroutine(PerformSkillWithDelay(1, skill1Damage));
        }
    }

    public void Skill2TripleAttack()
    {
        if (!useBow && timer.currentTurnTime >= playerData.cooldown)
        {
            DisableSkillButtons();
            skill2Damage = baseAttack + playerData.attack * 3;
            StartCoroutine(PerformTripleAttack(skill2Damage));
        }
    }

    public void Skill3DefenseOnClick()
    {
        if (!useBow && timer.currentTurnTime >= playerData.cooldown)
        {
            DisableSkillButtons();
            StartCoroutine(PerformDefenseWithDelay());
        }
    }

    private IEnumerator PerformSkillWithDelay(int attackType, int damage)
    {
        yield return new WaitForSeconds(playerData.cooldown);
        targetEnemy = FindObjectOfType<Enemy>();
        targetEnemy.TakeDamage(damage);
        animator.SetTrigger("Attack1");
        timer.ResetTimerAndMoveEnemy();
        EnableSkillButtons();
    }

    private IEnumerator PerformTripleAttack(int totalDamage)
    {
        int individualDamage = totalDamage / 3;

        for (int i = 1; i <= 3; i++)
        {
            animator.SetTrigger("Attack" + i);
            yield return new WaitForSeconds(0.5f);
            targetEnemy = FindObjectOfType<Enemy>();
            targetEnemy.TakeDamage(individualDamage);
        }

        timer.ResetTimerAndMoveEnemy();
        EnableSkillButtons();
    }

    private IEnumerator PerformDefenseWithDelay()
    {
        yield return new WaitForSeconds(playerData.cooldown);
        animator.SetTrigger("Block");
        animator.SetBool("IdleBlock", true);
        timer.ResetTimerAndMoveEnemy();
        EnableSkillButtons();
    }

    private void DisableSkillButtons()
    {
        skill1.GetComponent<Button>().interactable = false;
        skill2.GetComponent<Button>().interactable = false;
        skill3.GetComponent<Button>().interactable = false;
    }

    private void EnableSkillButtons()
    {
        skill1.GetComponent<Button>().interactable = true;
        skill2.GetComponent<Button>().interactable = true;
        skill3.GetComponent<Button>().interactable = true;
    }
}
