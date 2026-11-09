using UnityEngine;
using KngChak.Data;
using System.Collections.Generic;

namespace KngChak.Rocks
{
    public class RockSpawner : MonoBehaviour
    {

        [SerializeField] private Rock _rockPrefab;
        [SerializeField] private ArenaConfig _arena;

        [Header("Tuning (moves to WaveDefinition in #17)")]
        [SerializeField, Min(1)] private int _maxInFlight = 4;
        [SerializeField, Min(0.1f)] private float _spawnInterval = 2f;
        [SerializeField, Min(0f)] private float _spawnMargin = 3f;

        private float _timer;
        private readonly List<Rock> _rockInFlight = new List<Rock>();
        private float SpawnRadius => _arena.ArenaRadius + _spawnMargin;




        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        private void Awake()
        {
            if (_rockPrefab == null || _arena == null)
            {
                Debug.LogError("RockSpawner: assign Rock Prefab and Arena Config", this);
                enabled = false;
            }
        }

        

        // Update is called once per frame
        private void Update()
        {
            _rockInFlight.RemoveAll(rock => rock == null || rock.IsBroken);
            _timer -= Time.deltaTime;

            if (_timer <= 0f && _rockInFlight.Count < _maxInFlight)

            {
                spawnRock();
                _timer = _spawnInterval;
            }

        }

        private void spawnRock()
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector2 position = direction * SpawnRadius;


            Rock rock = Instantiate(_rockPrefab, position, Quaternion.identity, transform);

            _rockInFlight.Add(rock);
        }

        private void OnDrawGizmosSelected()
        {
            if(_arena == null) return ;
            Gizmos.DrawWireSphere(transform.position, SpawnRadius);
        }
    };
}
