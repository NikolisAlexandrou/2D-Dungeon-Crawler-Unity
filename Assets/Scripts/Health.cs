using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class Health : MonoBehaviour
{
    public float currentHealth, maxHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }
    public GameObject damageParticle;
    public void GetHit(int amount, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;
        GameManager.i.gameObject.GetComponent<CameraShake>().ShakeIt();

        currentHealth -= amount;
        GameObject ob = Instantiate(GameManager.i.DamageIndicator, transform.position, Quaternion.identity);
        ob.GetComponent<TextMeshPro>().text = "-" + amount;
        ob.transform.DOMoveY(ob.transform.position.y + 0.5f, 1);
        Destroy(ob, 1);
        int i = Random.Range(0, 2);
        GameObject swin = Instantiate(damageParticle, transform.position, transform.rotation);
        //swin.GetComponent<SpriteRenderer>().color = Color.red;
        swin.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        Destroy(swin, 0.2f);
        switch (i)
        {
            case 0:
                SoundManager.i.AttackSound0.Play();
                break;
            case 1:
                SoundManager.i.AttackSound1.Play();
                break;
            case 2:
                SoundManager.i.AttackSound2.Play();
                break;
        }
        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
            if (gameObject.GetComponent<EnemyAI>() != null)
            {
                gameObject.GetComponent<EnemyAI>().IsStunned = true;
                StartCoroutine(removeStun(gameObject));
               
                SpriteRenderer rb1 = gameObject.GetComponent<EnemyAI>().AllSprites[0];
                SpriteRenderer rb2 = gameObject.GetComponent<EnemyAI>().AllSprites[1];
                SpriteRenderer rb3 = gameObject.GetComponent<EnemyAI>().AllSprites[2];

                playHurtAnimation(rb1);
                playHurtAnimation(rb2);
                playHurtAnimation(rb3);

                if (sender.GetComponent<PlayerController>() == null)
                    return;

                GetComponent<Rigidbody2D>().AddForce(sender.GetComponent<PlayerController>().wp.transform.right * 2000, ForceMode2D.Impulse);

            }
        }
        else
        {
            if (gameObject.GetComponent<EnemyAI>() != null)
            {
                if (GameManager.getInstance().enemies.Contains(gameObject))
                {
                    GameManager.getInstance().enemies.Remove(gameObject);
                }
            }
            OnDeathWithReference?.Invoke(sender);
            isDead = true;
            Destroy(gameObject);
        }
    }
    [SerializeField] float stunTime;
    public IEnumerator removeStun(GameObject ob)
    {
        yield return new WaitForSeconds(stunTime);
        ob.GetComponent<EnemyAI>().IsStunned = false;
    }


    public void playHurtAnimation(SpriteRenderer ob)
    {
        var sequence = DOTween.Sequence();
        sequence.Append(ob.DOColor(Color.red, 0.15f));
        sequence.Append(ob.DOColor(ob.GetComponentInParent<EnemyAI>().originalColour, 0.15f));
    }

}
