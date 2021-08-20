using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = -3.0f;
    [SerializeField] //0=triple shot 1=speed 2=shields
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;
    
    void Start()
    {
        
    }

    void Update()
    {
        powerupMovement();
    }

    void powerupMovement(){
        transform.Translate(new Vector3(0, 1, 0) * _speed * Time.deltaTime);
        // transform.position = new Vector3(Random.Range(-9.2f, 9.2f), 8f, 0);
        if(transform.position.y <= -6.0f){
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if(player != null){
                switch(_powerupID){
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedPowerupActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
