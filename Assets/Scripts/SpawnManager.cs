using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _powerupContainer;
    private bool _stopSpawning;

    void Start(){
        StartCoroutine(SpwanRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }


    IEnumerator SpwanRoutine() {
        yield return new WaitForSeconds(3.0f);

        while(_stopSpawning == false){
            Vector3 PosToSpawn = new Vector3(Random.Range(-9f, 9.9f), 8f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, PosToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine(){
        yield return new WaitForSeconds(3.0f);
        
        while(_stopSpawning == false){
            Vector3 PosToSpawn = new Vector3(Random.Range(-9.2f, 9.2f), 8f, 0);
            int randomPowerup = Random.Range(0, 3);
            GameObject newPowerup = Instantiate(_powerups[randomPowerup], PosToSpawn, Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(5f, 7f));
        }
    }

    public void onPlayerDeath(){
        _stopSpawning = true;
    }
    
    public void get(){
        Debug.Log("Succesfully found the spawn manager component");
    }
}
