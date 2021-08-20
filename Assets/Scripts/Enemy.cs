using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = -4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private GameObject _leftThruster;
    private GameObject _rightThruster;

    void Start()
    {
      _player = GameObject.Find("Player").GetComponent<Player>();  
      _audioSource = GetComponent<AudioSource>();
      _anim = GetComponent<Animator>();
      _leftThruster = this.gameObject.transform.GetChild(0).gameObject;
      _rightThruster = this.gameObject.transform.GetChild(1).gameObject;
      if(_player == null){
        Debug.LogError("Player object is NULL");
      }

      if(_anim == null){
        Debug.LogError("Animator is NULL");
      }

      if(_audioSource == null){
          Debug.LogError("AudioSource on the enemy is NULL");
      }
    }

    void Update()
    {
        enemyMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other){ 

        if(other.tag == "Player"){
            Player player = other.transform.GetComponent<Player>();
            if(player != null){
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _leftThruster.SetActive(false);
            _rightThruster.SetActive(false);
            Destroy(this.gameObject, 2.4f);
        }

        if(other.tag == "Laser"){
            Laser laser = other.GetComponent<Laser>();
            bool enemyLaser = laser.isEnemyLaser();
            if(!enemyLaser)
            {
                Destroy(other.gameObject);
                if(_player != null){
                    _player.AddScore();
                }

                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _audioSource.Play();
                _leftThruster.SetActive(false);
                _rightThruster.SetActive(false);
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.4f);
            }
        }
    }

    void enemyMovement(){

        if(transform.position.y <= -6.0f){
            transform.position = new Vector3(Random.Range(-9f, 9.9f), 8.0f, 0);
        }

        transform.Translate(new Vector3(0, 1, 0) * _speed * Time.deltaTime);
    }
}
