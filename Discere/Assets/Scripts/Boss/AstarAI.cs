using System;
using UnityEngine;
using Pathfinding;

public class AstarAI : MonoBehaviour
{
    public Transform targetPos;
    private Seeker _seeker;
    private Rigidbody2D _rb2D;

    private Path _path;

    public float speed = 2f;
    public float nextWaypointDist = 2.5f;

    private int _currentWayptoint = 0;

    public bool reachedEndOfPath;

    private void Start() {
        _seeker = GetComponent<Seeker>();
        _rb2D = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath() {
        if (_seeker.IsDone())
            _seeker.StartPath(transform.position, targetPos.position, OnPathComplete);
    }

    public void OnPathComplete(Path p) {
        if (p.error) return;

        _path = p;
        _currentWayptoint = 0;
    }

    private void FixedUpdate() {
        // S'il n'y a pas de chemin généré
        if (_path == null) return;

        // Calcule la distance vers le prochain point
        float distance = Vector3.SqrMagnitude(_path.vectorPath[_currentWayptoint] - transform.position);

        // Si on s'approche du point
        if (distance < nextWaypointDist) {
            _currentWayptoint++; // Mise à jour vers le prochain point
        }

        // Si l'on a atteint le dernier point
        if (_currentWayptoint >= _path.vectorPath.Count) {
            reachedEndOfPath = true; // On le signale via un booléen
            return; // La boucle se stoppe
        }

        reachedEndOfPath = false; // On continue d'avancer

        Vector3 direction = (_path.vectorPath[_currentWayptoint] - transform.position).normalized; // Vercteur direction
        Vector2 ok = Vector2.MoveTowards(_rb2D.position, _path.vectorPath[_currentWayptoint], speed * Time.fixedDeltaTime);
        _rb2D.MovePosition(ok);

        //transform.position += direction * (speed * Time.fixedDeltaTime); // Application du mouvement
    }
}