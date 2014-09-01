using UnityEngine;
using System;
using System.Collections;

public class NPC : MonoBehaviour 
{
    public int currentTileIndex = 0;
    public float speed = 10f;

    private bool _isMoving = false;
    private UISprite[] _moveCounterDigits;
    private TileInfo[] _tiles;
    private Player[] _players;
    private int _wantedSteps;
    private Action _onMoveCompleted;
    private int _stepsCounter = 0;
    private int _nextId;
    private Vector2 _destination;

    public SpriteRenderer zombieRenderer;
    public SpriteRenderer zombieAttack;
    public SpriteRenderer zombieHighlight;

    public AudioSource audio;

    private void Update()
    {
        if (!_isMoving) return;

        transform.position = Vector2.MoveTowards(transform.position,
            _destination, speed * Time.deltaTime);

        transform.localScale = new Vector3(_tiles[currentTileIndex].direction, 1, 1);

        zombieRenderer.enabled = true;
        zombieAttack.enabled = false;

        if (Vector2.Distance(transform.position, _destination) <= 0.1f)
        {
            _isMoving = false;

            _stepsCounter++;

            int stepsRemaining = _wantedSteps - _stepsCounter;

            foreach (UISprite s in _moveCounterDigits)
            {
                s.enabled = false;
            }

            _moveCounterDigits[stepsRemaining].enabled = true;

            if (_stepsCounter == _wantedSteps)
            {
                audio.Stop();

                currentTileIndex++;

                killPlayer();

                if (_onMoveCompleted != null)
                    _onMoveCompleted();                
            }
            else
            {
                killPlayer();

                getNextWaypoint();
            }
        }
    }

    public void move(int totalSteps,TileInfo[] tiles,
        UISprite[] moveCounterDigits,Player[] players,
        Action onMoveCompleted)
    {
        audio.Play();

        _moveCounterDigits = moveCounterDigits;
        _stepsCounter = 0;

        _tiles = tiles;
        _players = players;

        if (currentTileIndex > 47)
            currentTileIndex = 0;

        _wantedSteps = totalSteps;
        _onMoveCompleted = onMoveCompleted;

        _nextId = _tiles[currentTileIndex].nextId;
        _destination = _tiles[_nextId - 1].transform.localPosition;

        _isMoving = true;
    }

    private void getNextWaypoint()
    {
        currentTileIndex++;

        if (currentTileIndex > 47)
            currentTileIndex = 0;

        _nextId = _tiles[currentTileIndex].nextId;
        _destination = _tiles[_nextId - 1].transform.localPosition;

        _isMoving = true;
    }

    private void killPlayer()
    {
        foreach (Player p in _players)
        {
            //Debug.Log(p.id + " | " + p.currentTileIndex + " | " + currentTileIndex);

            if (p.currentTileIndex == currentTileIndex)
            {
                AudioManager.instance.play(15, false);

                p.dead();

                zombieAttack.enabled = true;
                zombieRenderer.enabled = false;

                Debug.Log("Killed player " + p.id);             
            }
        }
    }

    public void selectZombie()
    {
        zombieHighlight.enabled = true;
    }

    public void deselectZombie()
    {
        zombieHighlight.enabled = false;
    }
}