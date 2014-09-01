using UnityEngine;
using System;
using System.Collections;

public class Player : MonoBehaviour 
{
    public int currentTileIndex = 0;
    public int id = 0;
    public float speed = 10f;
    public bool isDebug = false;

    public bool isGreenCandy = false;
    public bool isRedCandy = false;
    public bool isBlueCandy = false;

    public UISprite greenCandySprite;
    public UISprite redCandySprite;
    public UISprite blueCandySprite;

    public bool isGuaranteedCandy = false;
    public UISprite guaranteedCandy;
    public bool isCandySnatcher = false;
    public UISprite candySnatcher;
    public bool isFrozenFingers = false;
    public UISprite frozenFingers;
    public bool isBrokenDice = false;
    public UISprite brokenDice;

    public int initialWagerAmount = 0;
    public int bonusCoinsAmount = 0;

    public UILabel wagerAmountLabel;
    public UILabel bonusCoinsLabel;

    public float sugarLevel;
    public float initialSugarLevel = 10000;
    public UISprite sugarLevelUI;

    public UISprite highlight;
    public UISprite deadHighlight;

    public GameObject uiGroup;

    private bool _isMoving = false;
    private Action _onMoveCompleted;
    private Action<int> _onShowCandyUI;
    private Action<int> _onWin;
    private Action<int> _onShowNotificationUI;
    private Action<int> _onDead;
    private TileInfo[] _tiles;
    private Player[] _players;
    private Vector2 _destination;
    private int _nextId;
    public int _wantedSteps;
    private int _stepsCounter = 0;
    private UISprite[] _movesCounterDigits;
    private bool _isShortCut = false;

    public SpriteRenderer deadRenderer;
    public SpriteRenderer aliveRenderer;
    public SpriteRenderer highlightRenderer;

    public Animator characterAnimator;

    public AudioSource audio;

    public void registerEvent(Action<int> onWin,
        Action<int> onShowCandyUI,Action<int> onShowNotificationUI,Action<int> onDead)
    {
        _onWin = onWin;
        _onShowCandyUI = onShowCandyUI;
        _onShowNotificationUI = onShowNotificationUI;
        _onDead = onDead;
    }

    private void Update()
    {
        if (!_isMoving) return;

        transform.position = Vector2.MoveTowards(transform.position, 
            _destination, speed * Time.deltaTime);

        characterAnimator.SetBool("isWalk", true);

        transform.localScale = new Vector3(_tiles[currentTileIndex].direction, 1, 1);

        if(Vector2.Distance(transform.position,_destination) <= 0.1f)
        {
            _isMoving = false;

            //Add the sugar credit created into player's current sugar level
            updateSugarLevel(false,_tiles[currentTileIndex].sugarAward);
            updateBonusCoinsAmount(_tiles[currentTileIndex].bonusCoinsAward);

            //Here we need to find out if other player is on the same tile

            _stepsCounter++;

            int stepsRemaining = _wantedSteps - _stepsCounter;

            foreach (UISprite s in _movesCounterDigits)
            {
                s.enabled = false;
            }

            _movesCounterDigits[stepsRemaining].enabled = true;

            if(_stepsCounter == _wantedSteps)
            {
                audio.Stop();

                characterAnimator.SetBool("isWalk", false);

                currentTileIndex++;

                if (isCandySnatcher)
                {
                    snatchCandy();
                }

                if (_tiles[currentTileIndex].type == TileInfo.TileType.CHEST)
                {
                    if(!isFrozenFingers)
                    {
                        collectCandy(_tiles[currentTileIndex].getRandomCandy());
                        _tiles[currentTileIndex].chestPicked();
                    }
                    else
                    {
                        isFrozenFingers = false;
                        frozenFingers.enabled = false;
                        showNotification(7);
                    }                    
                }
                else if (_tiles[currentTileIndex].type == TileInfo.TileType.BONUS_GUARANTEE_CANDY)
                {
                    isGuaranteedCandy = true;
                    guaranteedCandy.enabled = true;

                    showNotification(1);
                }
                else if (_tiles[currentTileIndex].type == TileInfo.TileType.BONUS_CANDY_SNATCHER)
                {
                    isCandySnatcher = true;
                    candySnatcher.enabled = true;

                    showNotification(2);
                }
                else if (_tiles[currentTileIndex].type == TileInfo.TileType.BONUS_GLUCOSE_PILL)
                {
                    showNotification(3);
                }
                else if (_tiles[currentTileIndex].type == TileInfo.TileType.PENALTY_WITCH_SPELL)
                {
                    transform.position = _tiles[0].transform.localPosition;
                    currentTileIndex = 0;

                    showNotification(4);
                }
                else if (_tiles[currentTileIndex].type == TileInfo.TileType.PENALTY_FROZEN_FINGERS)
                {
                    isFrozenFingers = true;
                    frozenFingers.enabled = true;
                    showNotification(5);
                }
                else if (_tiles[currentTileIndex].type == TileInfo.TileType.PENALTY_BROKEN_DICE)
                {
                    isBrokenDice = true;
                    brokenDice.enabled = true;
                    showNotification(6);
                }
                if (_tiles[currentTileIndex].type == TileInfo.TileType.NORMAL 
                    || _tiles[currentTileIndex].type == TileInfo.TileType.GATE)
                {
                    if (_onMoveCompleted != null)
                        _onMoveCompleted();
                }                
            }
            else
            {
                if(isCandySnatcher)
                {
                    snatchCandy();

                    getNextWaypoint();
                }
                else
                {
                    getNextWaypoint();
                }                
            }                 
        }
    }

    private void snatchCandy()
    {
        foreach (Player p in _players)
        {
            if (p.id != id)
            {
                if (p.currentTileIndex == currentTileIndex)
                {
                    if (p.isGreenCandy && !isGreenCandy)
                    {
                        p.isGreenCandy = false;
                        p.greenCandySprite.enabled = false;

                        isGreenCandy = true;
                        greenCandySprite.enabled = true;

                        isCandySnatcher = false;
                        candySnatcher.enabled = false;

                        break;
                    }
                    else if (p.isRedCandy && !isRedCandy)
                    {
                        p.isRedCandy = false;
                        p.redCandySprite.enabled = false;

                        isRedCandy = true;
                        redCandySprite.enabled = true;

                        isCandySnatcher = false;
                        candySnatcher.enabled = false;

                        break;
                    }
                    else if (p.isBlueCandy && !isBlueCandy)
                    {
                        p.isBlueCandy = false;
                        p.blueCandySprite.enabled = false;

                        isBlueCandy = true;
                        blueCandySprite.enabled = true;

                        isCandySnatcher = false;
                        candySnatcher.enabled = false;

                        break;
                    }
                }
            }
        }
    }

    public void move(int steps, TileInfo[] tiles,
        UISprite[] movesCounterDigits, Player[] players, Action onMoveCompleted)
    {
        if (isDebug)
            steps = 1;

        audio.Play();

        _movesCounterDigits = movesCounterDigits;

        _stepsCounter = 0;

        _tiles = tiles;
        _players = players;

        if(!_isShortCut)
        {
            if (currentTileIndex > 47)
                currentTileIndex = 0;
        }        

        _wantedSteps = steps;
        
        _onMoveCompleted = onMoveCompleted;

        //if (_tiles[currentTileIndex].isShortCut)
        //{
        //    _nextId = _tiles[currentTileIndex].short_cut_id;
        //    _destination = _tiles[_nextId - 1].transform.localPosition;
        //    _isShortCut = true;
        //}
        //else
        //{
        //    _nextId = _tiles[currentTileIndex].nextId;
        //    _destination = _tiles[_nextId - 1].transform.localPosition;
        //}

        if (_tiles[currentTileIndex].type == TileInfo.TileType.GATE 
            && _tiles[currentTileIndex].currentOrietation == 1)
        {
            _nextId = _tiles[currentTileIndex].short_cut_id;
            _destination = _tiles[_nextId - 1].transform.localPosition;
            _isShortCut = _tiles[currentTileIndex].isShortCut;
            currentTileIndex = _nextId - 1;
        }
        else
        {
            _nextId = _tiles[currentTileIndex].nextId;
            _destination = _tiles[_nextId - 1].transform.localPosition;
        }   

        //_nextId = _tiles[currentTileIndex].nextId;
        //_destination = _tiles[_nextId - 1].transform.localPosition;

        _isMoving = true;
    }

    private void getNextWaypoint()
    {
        currentTileIndex++;

        if(!_isShortCut)
        {
            if (currentTileIndex > 47)
                currentTileIndex = 0;
        }

        if (_tiles[currentTileIndex].type == TileInfo.TileType.GATE 
            && _tiles[currentTileIndex].currentOrietation == 1)
        {
            _nextId = _tiles[currentTileIndex].short_cut_id;
            _destination = _tiles[_nextId - 1].transform.localPosition;
            _isShortCut = _tiles[currentTileIndex].isShortCut;
            currentTileIndex = _nextId - 1;         
        }
        else
        {
            _nextId = _tiles[currentTileIndex].nextId;
            _destination = _tiles[_nextId - 1].transform.localPosition;
        }        

        _isMoving = true;
    }

    private bool checkShortCut()
    {
        if(_tiles[currentTileIndex].isShortCut)
        {
            _nextId = _tiles[currentTileIndex].short_cut_id;
            currentTileIndex = _nextId - 1;
            _destination = _tiles[_nextId - 1].transform.localPosition;

            return true;
        }

        return false;
    }

    public void updateInitialWagerAmount(int amount)
    {
        initialWagerAmount = amount;
        wagerAmountLabel.text = amount.ToString();
    }

    private void updateBonusCoinsAmount(int amount)
    {
        bonusCoinsAmount += amount;
        bonusCoinsLabel.text = bonusCoinsAmount.ToString();
    }

    public bool updateSugarLevel(bool isDeduct,int amount)
    {
        if(isDeduct)
        {
            sugarLevel -= amount;
            if(sugarLevel < 0)
            {
                return true;
            }
        }
        else
        {
            sugarLevel += amount;
        }

        float sugarLevelFraction = sugarLevel / initialSugarLevel;
        sugarLevelUI.fillAmount = sugarLevelFraction;
        return false;
    }

    private void showNotification(int id)
    {
        if (_onShowNotificationUI != null)
            _onShowNotificationUI(id);
    }

    private void collectCandy(int id)
    {
        if(isGuaranteedCandy)
        {
            if(!isGreenCandy)
            {
                isGreenCandy = true;
                greenCandySprite.enabled = true;
                id = 1;
            }
            else if(!isRedCandy)
            {
                isRedCandy = true;
                redCandySprite.enabled = true;
                id = 2;
            }
            else
            {
                isBlueCandy = true;
                blueCandySprite.enabled = true;
                id = 3;
            }

            isGuaranteedCandy = false;
            guaranteedCandy.enabled = false;
        }
        else
        {
            if (id == 1)
            {
                isGreenCandy = true;
                greenCandySprite.enabled = true;
            }
            else if (id == 2)
            {
                isRedCandy = true;
                redCandySprite.enabled = true;
            }
            else if (id == 3)
            {
                isBlueCandy = true;
                blueCandySprite.enabled = true;
            }
        }        

        if(isGreenCandy && isRedCandy && isBlueCandy)
        {
            if (_onWin != null)
                _onWin(id);
        }
        else
        {
            if (_onShowCandyUI != null)
                _onShowCandyUI(id);
        }
    }

    public void justDead()
    {
        aliveRenderer.enabled = false;
        deadRenderer.enabled = true;
        deadHighlight.enabled = true;
    }

    public void dead()
    {
        if(id == 1)
        {
            AudioManager.instance.play(3, false);
        }
        else if(id == 2)
        {
            AudioManager.instance.play(4, false);
        }
        else if(id == 3)
        {
            AudioManager.instance.play(2, false);
        }              

        aliveRenderer.enabled = false;
        deadRenderer.enabled = true;
        deadHighlight.enabled = true;

        if (_onDead != null)
            _onDead(id);
    }

    public void selectPlayer()
    {
        highlightRenderer.enabled = true;
    }

    public void deselectPlayer()
    {
        highlightRenderer.enabled = false;
    }

    public void reset()
    {
        isGreenCandy = false;
        isRedCandy = false;
        isBlueCandy = false;

        highlight.enabled = false;
        deadHighlight.enabled = false;

        greenCandySprite.enabled = false;
        redCandySprite.enabled = false;
        blueCandySprite.enabled = false;

        guaranteedCandy.enabled = false;
        candySnatcher.enabled = false;
        frozenFingers.enabled = false;
        brokenDice.enabled = false;

        aliveRenderer.enabled = true;
        deadRenderer.enabled = false;

        initialWagerAmount = 0;
        bonusCoinsAmount = 0;
        sugarLevel = initialSugarLevel;
        sugarLevelUI.fillAmount = 1f;

        wagerAmountLabel.text = "0";
        bonusCoinsLabel.text = "0";

        currentTileIndex = 0;
    }
}