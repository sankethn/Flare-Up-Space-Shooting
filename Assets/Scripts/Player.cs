using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private float _powerupSpeedMultiplier = 2f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private UIManager _UIManager;

    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;


    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    void Start()
    {
        transform.position = new Vector3(0,0,0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if(_spawnManager == null){
            Debug.LogError("Spwan Manager is NULL");
        }

        if(_UIManager == null){
            Debug.LogError("UI Manager is NULL");
        }

        if(_audioSource == null){
            Debug.LogError("AudioSource on the player is NULL");
        }
        else{
            _audioSource.clip = _laserSoundClip;
        }
    }

    void Update()
    {
        calculateMovement();
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire){
            fireLaser();
        }
    }

    void calculateMovement(){
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.7f, 5.8f), 0);

        if(transform.position.x >= 11.5f){
            transform.position = new Vector3(-11.5f, transform.position.y, 0);
        }
        else if(transform.position.x <= -11.5f){
            transform.position = new Vector3(11.5f, transform.position.y, 0);
        }
    }

    void fireLaser(){
        _canFire = Time.time + _fireRate;
        if(_isTripleShotActive == true){
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else{
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage(){
        if(_isShieldActive == true){
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }
        else{
            _lives--;
            _UIManager.UpdateLives(_lives);
        }

        if(_lives == 2){
            _leftEngine.SetActive(true);
        }
        else if(_lives == 1){
            _rightEngine.SetActive(true);
        }

        if(_lives<1){
            // _UIManager.CheckForHighScore();
            _spawnManager.onPlayerDeath();
            _UIManager.GameOver();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive(){
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine(){
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void SpeedPowerupActive(){
        _speed*=_powerupSpeedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine(){
        yield return new WaitForSeconds(5f);
        _speed/=_powerupSpeedMultiplier;
    }

    public void ShieldActive(){
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void AddScore(){
        _UIManager.UpdateScore();
    }

}
