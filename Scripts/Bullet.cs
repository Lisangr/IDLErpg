using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public Transform spawnAOEPoint;
    public float bulletSpeed;
    private Enemy target;
    private Transform targetPosition;

    // UI buttons for skill attacks
    public Button firstSkillButton;
    public Button secondSkillButton;
    public Button aoeSkillButton;

    private Timer timer;
    private PlayerData playerData;

    private void Start()
    {
        EnableAttackButtons(); // Ensure buttons are enabled initially
        timer = FindObjectOfType<Timer>();
        playerData = FindObjectOfType<PlayerData>(); // Fetch PlayerData to use cooldown
    }

    public void FirstSkill()
    {
        if (SkillChanger.useBow)
        {
            DisableAttackButtons(); // Disable all buttons
            target = FindObjectOfType<Enemy>().GetComponent<Enemy>();
            targetPosition = target.transform;
            FireBullet(spawnPoint, targetPosition, 1);
            StartCoroutine(ResetAttackButtonsAfterCooldown());
        }
    }

    public void SecondSkill()
    {
        if (SkillChanger.useBow)
        {
            DisableAttackButtons(); // Disable all buttons
            target = FindObjectOfType<Enemy>().GetComponent<Enemy>();
            targetPosition = target.transform;
            FireBullet(spawnPoint, targetPosition, 3);
            StartCoroutine(ResetAttackButtonsAfterCooldown());
        }
    }

    public void AOESkillAttack()
    {
        if (SkillChanger.useBow)
        {
            DisableAttackButtons(); // Disable all buttons
            target = FindObjectOfType<Enemy>().GetComponent<Enemy>();
            targetPosition = target.transform;
            FireBulletWithRotation(spawnAOEPoint, targetPosition, 1);
            StartCoroutine(ResetAttackButtonsAfterCooldown());
        }
    }

    private void FireBullet(Transform spawn, Transform target, int damageMultiplier)
    {
        GameObject bullet = Instantiate(bulletPrefab, spawn.position, Quaternion.identity);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        bulletBehavior.Initialize(target, bulletSpeed, damageMultiplier, spawn.position);
    }

    private void FireBulletWithRotation(Transform spawn, Transform target, int damageMultiplier)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 270);
        GameObject bullet = Instantiate(bulletPrefab, spawn.position, rotation);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        bulletBehavior.Initialize(target, bulletSpeed, damageMultiplier, spawn.position);
    }

    private void DisableAttackButtons()
    {
        firstSkillButton.interactable = false;
        secondSkillButton.interactable = false;
        aoeSkillButton.interactable = false;
    }

    private void EnableAttackButtons()
    {
        firstSkillButton.interactable = true;
        secondSkillButton.interactable = true;
        aoeSkillButton.interactable = true;
    }

    private IEnumerator ResetAttackButtonsAfterCooldown()
    {
        yield return new WaitForSeconds(playerData.cooldown); // Wait for the cooldown duration
        timer.ResetTimerAndMoveEnemy();
        EnableAttackButtons(); // Re-enable buttons after the cooldown
    }
    /*
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public Transform spawnAOEPoint;
    public float bulletSpeed;
    public Enemy target;
    private Transform targetPosition;

    public void FirstSkill()
    {
        target = FindObjectOfType<Enemy>().GetComponent<Enemy>();
        targetPosition = target.transform;
        if (SkillChanger.useBow)
        {
            FireBullet(spawnPoint, targetPosition, 1);
        }
    }

    public void SecondSkill()
    {
        target = FindObjectOfType<Enemy>().GetComponent<Enemy>();
        targetPosition = target.transform;
        if (SkillChanger.useBow)
        {
            FireBullet(spawnPoint, targetPosition, 3);
        }
    }

    public void AOESkillAttack()
    {
        target = FindObjectOfType<Enemy>().GetComponent<Enemy>();
        targetPosition = target.transform;
        if (SkillChanger.useBow)
        {
            FireBulletWithRotation(spawnAOEPoint, targetPosition, 1);
        }
    }

    private void FireBullet(Transform spawn, Transform target, int damageMultiplier)
    {
        GameObject bullet = Instantiate(bulletPrefab, spawn.position, Quaternion.identity);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        bulletBehavior.Initialize(target, bulletSpeed, damageMultiplier, spawn.position);
    }

    private void FireBulletWithRotation(Transform spawn, Transform target, int damageMultiplier)
    {
        // Create a rotation of 90 degrees around the Y axis
        Quaternion rotation = Quaternion.Euler(0, 0, 270);

        GameObject bullet = Instantiate(bulletPrefab, spawn.position, rotation);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        bulletBehavior.Initialize(target, bulletSpeed, damageMultiplier, spawn.position);
    }*/
}
