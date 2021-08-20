﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
  
    void Update()
    {
        if(_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp(){
        transform.Translate(new Vector3(0, 1, 0) * _speed * Time.deltaTime);

        if(transform.position.y >= 8.0f){
            if(transform.parent != null){
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

   void MoveDown(){
        transform.Translate(new Vector3(0, 1, 0) * -_speed * Time.deltaTime);

        if(transform.position.y < -5.1f){
            if(transform.parent != null){
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
        }
    }

    public bool isEnemyLaser()
    {
        return _isEnemyLaser;
    }
}
