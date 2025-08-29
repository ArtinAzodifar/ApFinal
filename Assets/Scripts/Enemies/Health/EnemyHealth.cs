using System;
using UnityEngine;
using Unity.Netcode;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyHealth : NetworkBehaviour, Damagable
{
    [SerializeField] private GameObject[] collectibles;
    [SerializeField] private int initialHealth;
    private NetworkVariable<int> health = new NetworkVariable<int>();
    [SerializeField] private EnemyHB healthBar;
    public bool isDead = false;

    private SoundPlayer soundPlayer;
    [SerializeField] private string deathSoundName;

    private DamageFlash _damageFlash;
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameManager.Instance;
    }


    public void Start()
    {
        if (gameManager.IsLocalMode()) health.Value = initialHealth;
        if (healthBar != null) healthBar.SetMaxHealth(initialHealth);
        soundPlayer = GetComponent<SoundPlayer>();
        _damageFlash = GetComponent<DamageFlash>();
        //if (!gameManager.IsLocalMode() && IsServer && GetComponent<NetworkObject>() != null && !GetComponent<NetworkObject>().IsSpawned) GetComponent<NetworkObject>().Spawn();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkDespawn();
        if (IsServer) health.Value = initialHealth;
    }

    public void Damage(int amount)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        health.Value -= amount;

        //damage animation
        if (_damageFlash != null)
        {
            if (gameManager.IsLocalMode()) _damageFlash.CallDamageFlash();
            else if (IsServer) damageFlashClientRpc();
        }

        //health bar update
        if (healthBar != null)
        {
            if (gameManager.IsLocalMode()) healthBar.SetHealth(health.Value);
            else if (IsServer) updateHealthBarClientRpc(health.Value);
        }

        //death check
        if (health.Value <= 0 && !isDead)
        {
            isDead = true;
            Animator animator = gameObject.GetComponent<Animator>();

            //sound
            if (soundPlayer != null)
            {
                if (gameManager.IsLocalMode()) playTheSound();
                else if (IsServer) PlaySoundClientRpc();
            }

            //random collectible create
            //random < 25 -> healthPoint 25%
            //random < 50 -> damage booster 25%
            //random < 60 -> max mana 10%
            //random > 60 -> nothing 40%
            int random = UnityEngine.Random.Range(0, 100);
            if (random < 25)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                var collectible = Instantiate(collectibles[0], position, Quaternion.identity);//health

                //for online mode
                var netObject = collectible.GetComponent<NetworkObject>();
                if (!gameManager.IsLocalMode() && IsServer && netObject != null) netObject.Spawn();
            }
            else if (random < 50)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                var collectible = Instantiate(collectibles[1], position, Quaternion.identity);//damage booster

                //for online mode
                var netObject = collectible.GetComponent<NetworkObject>();
                if (!gameManager.IsLocalMode() && IsServer && netObject != null) netObject.Spawn();
            }
            else if (random < 60)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                var collectible = Instantiate(collectibles[2], position, Quaternion.identity);//max mana

                //for online mode
                var netObject = collectible.GetComponent<NetworkObject>();
                if (!gameManager.IsLocalMode() && IsServer && netObject != null) netObject.Spawn();
            }

            if (animator != null && HasTrigger(animator, "Death"))
            {
                animator.SetTrigger("Death");

                //save in localmode
                if (gameManager.IsLocalMode() && GetComponent<PersistentObject>() != null) GetComponent<PersistentObject>().OnProcessed();

                //delete object
                if (gameManager.IsLocalMode()) Destroy(gameObject, !SceneManager.GetActiveScene().name.Equals("LevelThree") ? 1.8f : 0.8f);
                else if (IsServer && GetComponent<NetworkObject>() != null) StartCoroutine(Despawn());
                else if (IsServer) DestroyClientRpc(!SceneManager.GetActiveScene().name.Equals("LevelThree") ? 1.8f : 0.8f);
            }
            else
            {
                //save in local mode
                if (gameManager.IsLocalMode() && GetComponent<PersistentObject>() != null) GetComponent<PersistentObject>().OnProcessed();

                //delete object
                if (gameManager.IsLocalMode()) Destroy(gameObject);
                else if (IsServer && GetComponent<NetworkObject>() != null)
                {
                    if ((bool)GetComponent<NetworkObject>().IsSceneObject) DestroyClientRpc(0);
                    gameObject.GetComponent<NetworkObject>().Despawn();
                }
                else if (IsServer) DestroyClientRpc(0f);
            }
        }
    }

    private void playTheSound()
    {
        AudioClip clipToPlay = soundPlayer.GetClipByName(deathSoundName);

        if (clipToPlay != null)
        {
            float volume = AudioController.Instance.sfxVolume;
            AudioSource.PlayClipAtPoint(clipToPlay, transform.position, volume);
        }
        else Debug.LogError($"SOUND NOT FOUND! The name '{deathSoundName}' does not match any clip in the SoundPlayer component on {gameObject.name}.");
    }

    private bool HasTrigger(Animator animator, string triggerName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger && param.name == triggerName) return true;
        }
        return false;
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(!SceneManager.GetActiveScene().name.Equals("LevelThree") ? 1.8f : 0.8f);
        if ((bool)GetComponent<NetworkObject>().IsSceneObject) DestroyClientRpc(0);
        gameObject.GetComponent<NetworkObject>().Despawn();
    }

    //getter
    public bool IsDead() { return isDead; }

    //ClientRpc
    [ClientRpc]
    private void updateHealthBarClientRpc(int amount) { healthBar.SetHealth(amount); }

    [ClientRpc]
    private void PlaySoundClientRpc() { playTheSound(); }

    [ClientRpc]
    private void damageFlashClientRpc() { _damageFlash.CallDamageFlash(); }

    [ClientRpc]
    private void DestroyClientRpc(float time) { Destroy(gameObject, time); }
}