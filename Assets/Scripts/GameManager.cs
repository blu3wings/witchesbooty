using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
    public TileInfo[] tiles;
    public Player[] players;
    public NPC zombie;

    //public UILabel movesCounter;
    public UISprite[] moveCounterDigits;
    //public UILabel dice1Counter;
    public UISprite dice1Sprite;
    public UISprite dice2Sprite;
    //public UILabel dice2Counter;
    public TweenPosition dice1Tween;
    public TweenPosition dice2Tween;

    public int sugarConsume = 100;

    private bool _isAllowRoll = true;
    public int _currentPlayerIndex = 0;
    private int _totalSteps;
    private bool _isWin = false;
    private bool _isBet = false;
    private int _roundCount = 1;
    private bool _isExtraRoll = false;
    private bool _isZombieTurn = false;

    public GameObject winnerLoserNotificationGroup;
    public UISprite winnerBg;
    public UISprite loserBg;
    public UILabel winnerLoserMessage;
    public UILabel continueButtonLabel;

    public GameObject lastCandyWinnerGroup;
    public UISprite greenCandy;
    public UISprite redCandy;
    public UISprite blueCandy;
    public UILabel candyCollectMessage;
    public UISprite candyWinnerImage;
    public UILabel candyCollectButtonLabel;

    public UILabel roundCounterLabel;

    public GameObject rollButton;
    public GameObject zombieRollButton;

    public GameObject betWinPanel;
    public GameObject resultTitle;
    public GameObject bootyTitle;
    public GameObject[] betButtons;
    public UILabel archiBetLabel;
    public UILabel sashiBetLabel;
    public UILabel galiverBetLabel;
    public UISprite archiWinner;
    public UISprite sashiWinner;
    public UISprite galiverWinner;

    public GameObject pickupNotification;
    public UISprite notificationPotrait;
    public GameObject buff;
    public GameObject debuff;
    public GameObject itemHeading;
    public UISprite itemIcon;
    public GameObject notificationMessage;
    public GameObject notificationContinueButton;
    public GameObject notificationOkButton;

    public GameObject deathLabel;
    public GameObject easyLabel;

    public GameObject deathParticle;

    public TileInfo[] shortCutGates;
    public TileInfo[] shortCutNodes;
    public GameObject[] closeGates;
    public GameObject[] openGates;
    private int _shortCutCounter = 0;
    private int currentLevel = 1;

    private int _defaultWagerAmount = 1000;
    private int _archiBetAmount;
    private int _sashiBetAmount;
    private int _galiverBetAmount;

    private List<int> _losePlayerIndex = new List<int>();

    private void Start()
    {
        Screen.fullScreen = true;

        easyLabel.SetActive(true);
        deathLabel.SetActive(false);

        _archiBetAmount = _defaultWagerAmount;
        _sashiBetAmount = _defaultWagerAmount;
        _galiverBetAmount = _defaultWagerAmount;

        showBetUI();
        dice1Tween.gameObject.SetActive(false);
        dice2Tween.gameObject.SetActive(false);
    }

    private void showBetUI()
    {
        archiWinner.enabled = false;
        sashiWinner.enabled = false;
        galiverWinner.enabled = false;

        _isBet = true;

        betWinPanel.SetActive(true);
        bootyTitle.SetActive(true);
        resultTitle.SetActive(false);

        foreach (GameObject g in betButtons)
        {
            g.SetActive(true);
        }

        _losePlayerIndex.Clear();

        _isWin = false;
        winnerLoserNotificationGroup.SetActive(false);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].uiGroup.SetActive(true);
            players[i].reset();
            players[i].registerEvent(onWin, showCandyUI, showNotificationUI, onPlayerDied);
            if (i == 0)
            {
                players[i].updateInitialWagerAmount(_archiBetAmount);
            }
            else if (i == 1)
            {
                players[i].updateInitialWagerAmount(_sashiBetAmount);
            }
            else if (i == 2)
            {
                players[i].updateInitialWagerAmount(_galiverBetAmount);
            }

            players[i].gameObject.SetActive(false);

        }

        rollButton.SetActive(false);
    }

    private void showWinResultUI()
    {
        //_isBet = true;

        betWinPanel.SetActive(true);
        bootyTitle.SetActive(false);
        resultTitle.SetActive(true);
        archiWinner.enabled = false;
        sashiWinner.enabled = false;
        galiverWinner.enabled = false;

        foreach (GameObject g in betButtons)
        {
            g.SetActive(false);
        }

        //_isWin = false;
        winnerLoserNotificationGroup.SetActive(false);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].uiGroup.SetActive(true);
            players[i].reset();
            players[i].registerEvent(onWin, showCandyUI, showNotificationUI, onPlayerDied);
            if (i == 0)
            {
                players[i].updateInitialWagerAmount(_archiBetAmount);
            }
            else if (i == 1)
            {
                players[i].updateInitialWagerAmount(_sashiBetAmount);
            }
            else if (i == 2)
            {
                players[i].updateInitialWagerAmount(_galiverBetAmount);
            }

            players[i].gameObject.SetActive(false);

        }

        for (int i = 0; i < players.Length; i++)
        {
            if (!_losePlayerIndex.Contains(i))
            {
                highlightNextPlayer(i);

                if((i + 1) == 1)
                {
                    Data.instance.saveData(1);
                    archiWinner.enabled = true;
                    AudioManager.instance.play(12, false);
                }
                else if ((i + 1) == 2)
                {
                    Data.instance.saveData(2);
                    sashiWinner.enabled = true;
                    AudioManager.instance.play(13, false);
                }
                else if ((i + 1) == 3)
                {
                    Data.instance.saveData(2);
                    galiverWinner.enabled = true;
                    AudioManager.instance.play(14, false);
                }
            }

            if(i == 0)
            {
                int multiplier = 1;
                if(players[i].isGreenCandy)
                {
                    multiplier = 1;
                }

                if (players[i].isGreenCandy && players[i].isRedCandy)
                {
                    multiplier = 2;
                }

                if (players[i].isGreenCandy && players[i].isRedCandy && players[i].isBlueCandy)
                {
                    multiplier = UnityEngine.Random.Range(3, 10);
                }

                int winAmount = (players[i].initialWagerAmount +
                    players[i].bonusCoinsAmount) * multiplier;

                archiBetLabel.text = winAmount.ToString();
            }
            else if (i == 1)
            {
                int multiplier = 1;
                if (players[i].isGreenCandy)
                {
                    multiplier = 1;
                }

                if (players[i].isGreenCandy && players[i].isRedCandy)
                {
                    multiplier = 2;
                }

                if (players[i].isGreenCandy && players[i].isRedCandy && players[i].isBlueCandy)
                {
                    multiplier = UnityEngine.Random.Range(3, 10);
                }

                int winAmount = (players[i].initialWagerAmount +
                    players[i].bonusCoinsAmount) * multiplier;

                sashiBetLabel.text = winAmount.ToString();
            }
            else if (i == 2)
            {
                int multiplier = 1;
                if (players[i].isGreenCandy)
                {
                    multiplier = 1;
                }

                if (players[i].isGreenCandy && players[i].isRedCandy)
                {
                    multiplier = 2;
                }

                if (players[i].isGreenCandy && players[i].isRedCandy && players[i].isBlueCandy)
                {
                    multiplier = UnityEngine.Random.Range(3, 10);
                }

                int winAmount = (players[i].initialWagerAmount +
                    players[i].bonusCoinsAmount) * multiplier;

                galiverBetLabel.text = winAmount.ToString();
            }
        }

        _losePlayerIndex.Clear();

        rollButton.SetActive(false);

        AudioManager.instance.play(11, false);
    }

    public void plusArchiBet()
    {
        AudioManager.instance.play(0, false);
        _archiBetAmount += 1000;
        if (_archiBetAmount > 5000)
            _archiBetAmount = 5000;

        archiBetLabel.text = _archiBetAmount.ToString();
    }

    public void minusArchiBet()
    {
        AudioManager.instance.play(0, false);
        _archiBetAmount -= 1000;
        if (_archiBetAmount < 1000)
            _archiBetAmount = 1000;

        archiBetLabel.text = _archiBetAmount.ToString();
    }

    public void plusSashiBet()
    {
        AudioManager.instance.play(0, false);
        _sashiBetAmount += 1000;
        if (_sashiBetAmount > 5000)
            _sashiBetAmount = 5000;

        sashiBetLabel.text = _sashiBetAmount.ToString();
    }

    public void minusSashiBet()
    {
        AudioManager.instance.play(0, false);
        _sashiBetAmount -= 1000;
        if (_sashiBetAmount < 1000)
            _sashiBetAmount = 1000;

        sashiBetLabel.text = _sashiBetAmount.ToString();
    }

    public void plusGaliverBet()
    {
        AudioManager.instance.play(0, false);
        _galiverBetAmount += 1000;
        if (_galiverBetAmount > 5000)
            _galiverBetAmount = 5000;

        galiverBetLabel.text = _galiverBetAmount.ToString();
    }

    public void minusGaliverBet()
    {
        AudioManager.instance.play(0, false);
        _galiverBetAmount -= 1000;
        if (_galiverBetAmount < 1000)
            _galiverBetAmount = 1000;

        galiverBetLabel.text = _galiverBetAmount.ToString();
    }

    public void confirmBetAmount()
    {
        AudioManager.instance.play(0, false);

        if(_isWin)
        {
            if (currentLevel < 2)
            {
                deathLabel.SetActive(true);
                easyLabel.SetActive(false);

                deathParticle.SetActive(true);

                currentLevel++;
                showBetUI();
            }
            else
            {
                Debug.Log("Game Over!");
                Application.LoadLevel(2);
            }
        }
        else if(_isBet)
        {
            betWinPanel.SetActive(false);
            startGame();
        }        
    }

    private void startGame()
    {        
        _roundCount = 1;
        _shortCutCounter = 0;
        roundCounterLabel.text = _roundCount.ToString();

        foreach (UISprite s in moveCounterDigits)
        {
            s.enabled = false;
        }

        _isAllowRoll = true;
        rollButton.SetActive(true);

        for (int i = 0; i < players.Length; i++ )
        {
            players[i].uiGroup.SetActive(true);
            players[i].reset();
            players[i].registerEvent(onWin, showCandyUI, showNotificationUI, onPlayerDied);
            if(i == 0)
            {
                players[i].updateInitialWagerAmount(_archiBetAmount);
            }
            else if (i == 1)
            {
                players[i].updateInitialWagerAmount(_sashiBetAmount);
            }
            else if (i == 2)
            {
                players[i].updateInitialWagerAmount(_galiverBetAmount);
            }
            players[i].gameObject.SetActive(true);
            players[i].transform.position = tiles[0].transform.localPosition;
        }

        _currentPlayerIndex = 0;

        highlightNextPlayer(_currentPlayerIndex);
    }

    private void rollZombieDice()
    {
        foreach (UISprite s in moveCounterDigits)
        {
            s.enabled = false;
        }

        int dice1 = UnityEngine.Random.Range(1, 6);
        string dice1SpriteName = string.Empty;
        if (dice1 == 1)
        {
            dice1SpriteName = "dice1";
        }
        else if (dice1 == 2)
        {
            dice1SpriteName = "dice2";
        }
        else if (dice1 == 3)
        {
            dice1SpriteName = "dice3";
        }
        else if (dice1 == 4)
        {
            dice1SpriteName = "dice4";
        }
        else if (dice1 == 5)
        {
            dice1SpriteName = "dice5";
        }
        else if (dice1 == 6)
        {
            dice1SpriteName = "dice6";
        }
        else if (dice1 == 0)
        {
            dice1SpriteName = "dicelost";
        }

        int dice2 = UnityEngine.Random.Range(1, 6);
        string dice2SpriteName = string.Empty;
        if (dice2 == 1)
        {
            dice2SpriteName = "dice1";
        }
        else if (dice2 == 2)
        {
            dice2SpriteName = "dice2";
        }
        else if (dice2 == 3)
        {
            dice2SpriteName = "dice3";
        }
        else if (dice2 == 4)
        {
            dice2SpriteName = "dice4";
        }
        else if (dice2 == 5)
        {
            dice2SpriteName = "dice5";
        }
        else if (dice2 == 6)
        {
            dice2SpriteName = "dice6";
        }

        dice1Tween.gameObject.SetActive(true);

        dice1Tween.ResetToBeginning();
        dice1Sprite.spriteName = dice1SpriteName;
        dice1Tween.PlayForward();
        dice2Tween.ResetToBeginning();

        _isZombieTurn = true;

        StartCoroutine(generalDelay(1, () =>
        {
            dice2Tween.gameObject.SetActive(true);
            dice2Tween.PlayForward();

            _totalSteps = dice1 + dice2;

            dice2Sprite.spriteName = dice2SpriteName;
        }));
    }

    public void rollDice()
    {
        if (!_isAllowRoll) return;

        AudioManager.instance.play(0, false);

        foreach(UISprite s in moveCounterDigits)
        {
            s.enabled = false;
        }

        _isExtraRoll = false;

        //Stop the player from spamming the dice roll
        _isAllowRoll = false;
        

        int dice1 = UnityEngine.Random.Range(1, 6);

        if (players[_currentPlayerIndex].isBrokenDice)
        {
            dice1 = 0;
        }

        string dice1SpriteName = string.Empty;
        if(dice1 == 1)
        {
            dice1SpriteName = "dice1";
        }
        else if (dice1 == 2)
        {
            dice1SpriteName = "dice2";
        }
        else if (dice1 == 3)
        {
            dice1SpriteName = "dice3";
        }
        else if (dice1 == 4)
        {
            dice1SpriteName = "dice4";
        }
        else if (dice1 == 5)
        {
            dice1SpriteName = "dice5";
        }
        else if (dice1 == 6)
        {
            dice1SpriteName = "dice6";
        }
        else if (dice1 == 0)
        {
            dice1SpriteName = "dicelost";
        }

        int dice2 = UnityEngine.Random.Range(1, 6);
        string dice2SpriteName = string.Empty;
        if (dice2 == 1)
        {
            dice2SpriteName = "dice1";
        }
        else if (dice2 == 2)
        {
            dice2SpriteName = "dice2";
        }
        else if (dice2 == 3)
        {
            dice2SpriteName = "dice3";
        }
        else if (dice2 == 4)
        {
            dice2SpriteName = "dice4";
        }
        else if (dice2 == 5)
        {
            dice2SpriteName = "dice5";
        }
        else if (dice2 == 6)
        {
            dice2SpriteName = "dice6";
        }

        AudioManager.instance.play(10, false);
        dice1Tween.gameObject.SetActive(true);        

        dice1Tween.ResetToBeginning();
        dice1Sprite.spriteName = dice1SpriteName;
        dice1Tween.PlayForward();
        dice2Tween.ResetToBeginning();      

        StartCoroutine(generalDelay(1, () => 
        {
            dice2Tween.gameObject.SetActive(true);
            dice2Tween.PlayForward();

            _totalSteps = dice1 + dice2;

            dice2Sprite.spriteName = dice2SpriteName;        
        }));

        players[_currentPlayerIndex].isBrokenDice = false;
        players[_currentPlayerIndex].brokenDice.enabled = false;
    }

    public void startMovingPlayer()
    {
        moveCounterDigits[_totalSteps].enabled = true;

        if(_isZombieTurn)
        {
            StartCoroutine(generalDelay(1, () => 
            {
                zombie.move(_totalSteps, tiles, moveCounterDigits, players, () => 
                {
                    _isZombieTurn = false;
                    zombieRollButton.SetActive(false);
                    zombie.deselectZombie();
                    rollButton.SetActive(true);
                    _isAllowRoll = true;
                });
            }));
        }
        else
        {
            //Consume sugar
            if (players[_currentPlayerIndex].updateSugarLevel(true, sugarConsume))
            {
                players[_currentPlayerIndex].sugarLevelUI.fillAmount = 0f;
                onLose();
            }
            else
            {
                StartCoroutine(generalDelay(1, () =>
                {
                    players[_currentPlayerIndex].move(_totalSteps, tiles,
                    moveCounterDigits, players, () =>
                    {
                        continueGame();
                    });
                }));
            } 
        }       
    }

    private void highlightNextPlayer(int index)
    {
        foreach(Player p in players)
        {
            p.highlight.enabled = false;
            p.deselectPlayer();
        }

        players[index].selectPlayer();
        players[index].highlight.enabled = true;
    }

    private void onPlayerDied(int id)
    {
        if(!_losePlayerIndex.Contains(id-1))
            _losePlayerIndex.Add(id - 1);

        //players[id - 1].uiGroup.SetActive(false);

        if (_losePlayerIndex.Count == 2)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (!_losePlayerIndex.Contains(i))
                {
                    highlightNextPlayer(i);

                    //winnerLoserNotificationGroup.SetActive(true);
                    //winnerBg.enabled = true;
                    //loserBg.enabled = false;

                    //string name = string.Empty;

                    //if ((i + 1) == 1)
                    //{
                    //    name = "Archi";
                    //}
                    //else if ((i + 1) == 2)
                    //{
                    //    name = "Sashi";
                    //}
                    //else if ((i + 1) == 3)
                    //{
                    //    name = "Galiver";
                    //}

                    //winnerLoserMessage.text = string.Format("Player {0} wins!", name);
                    //continueButtonLabel.text = "Next Level";
                }
            }

            _isWin = true;
            _isBet = false;

            showWinResultUI();

            return;
        }

        if(_currentPlayerIndex == (id-1))
        {
            _currentPlayerIndex++;

            if (_currentPlayerIndex >= players.Length)
            {
                _currentPlayerIndex = 0;
                updateRoundCounter();
            }

            if (_losePlayerIndex.Contains(_currentPlayerIndex))
                _currentPlayerIndex++;

            if (_currentPlayerIndex >= players.Length)
            {
                _currentPlayerIndex = 0;
            } 

            highlightNextPlayer(_currentPlayerIndex);   
        }
    }

    private void onLose()
    {
        if (!_losePlayerIndex.Contains(_currentPlayerIndex))
            _losePlayerIndex.Add(_currentPlayerIndex);

        players[_currentPlayerIndex].justDead();

        rollButton.SetActive(false);

        pickupNotification.SetActive(true);
        buff.SetActive(false);
        debuff.SetActive(false);
        itemHeading.SetActive(false);
        itemIcon.enabled = false;
        notificationOkButton.SetActive(false);
        notificationContinueButton.SetActive(false);
        notificationMessage.SetActive(false);

        if (_currentPlayerIndex == 0)
        {
            notificationPotrait.spriteName = "archiface";
            AudioManager.instance.play(3, false);
        }
        else if (_currentPlayerIndex == 1)
        {
            notificationPotrait.spriteName = "sashiface";
            AudioManager.instance.play(4, false);
        }
        else if (_currentPlayerIndex == 2)
        {
            notificationPotrait.spriteName = "galiverface";
            AudioManager.instance.play(2, false);
        }

        notificationContinueButton.SetActive(true);
        notificationMessage.SetActive(true);      
    }

    private void onWin(int id)
    {
        showWinResultUI();

        _isWin = true;
        _isBet = false;

        rollButton.SetActive(false);
    }

    public void closeWinnerLoserNotification()
    {
        AudioManager.instance.play(0, false);

        //winnerLoserNotificationGroup.SetActive(false);
        pickupNotification.SetActive(false);

        if(_isWin)
        {
            if(currentLevel < 2)
            {
                currentLevel++;
                showBetUI();
            } 
            else
            {
                Debug.Log("Game Over!");
                Application.LoadLevel(0);
            }
        }
        else
        {
            if (_losePlayerIndex.Count == 2)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (!_losePlayerIndex.Contains(i))
                    {
                        highlightNextPlayer(i);                                            
                    }
                }

                _isWin = true;
                _isBet = false;

                showWinResultUI();
                return;
            }

            rollButton.SetActive(true);
            _isAllowRoll = true;

            _currentPlayerIndex++;

            if (_currentPlayerIndex >= players.Length)
            {
                _currentPlayerIndex = 0;
                updateRoundCounter();
            }

            if (_losePlayerIndex.Contains(_currentPlayerIndex))
                _currentPlayerIndex++;

            if (_currentPlayerIndex >= players.Length)
            {
                _currentPlayerIndex = 0;
            } 

            highlightNextPlayer(_currentPlayerIndex);  
        }              
    }

    private void showNotificationUI(int id)
    {
        pickupNotification.SetActive(true);
        buff.SetActive(false);
        debuff.SetActive(false);
        itemHeading.SetActive(false);
        itemIcon.enabled = false;
        notificationOkButton.SetActive(false);
        notificationContinueButton.SetActive(false);
        notificationMessage.SetActive(false);

        itemIcon.enabled = true;
        notificationOkButton.SetActive(true);

        //lastCandyWinnerGroup.SetActive(true);
        //greenCandy.enabled = false;
        //redCandy.enabled = false;
        //blueCandy.enabled = false;

        string message = string.Empty;

        if(_currentPlayerIndex == 0)
        {
            notificationPotrait.spriteName = "archiface";
        }
        else if (_currentPlayerIndex == 1)
        {
            notificationPotrait.spriteName = "sashiface";
        }
        else if (_currentPlayerIndex == 2)
        {
            notificationPotrait.spriteName = "galiverface";
        }

        if (id == 1) //Guarantee Candy
        {
            AudioManager.instance.play(8, false);
            //greenCandy.enabled = true;
            buff.SetActive(true);
            itemIcon.spriteName = "eightball";
            //message = "You got the guarantee candy.";

        }
        else if (id == 2)//Candy Snatcher
        {
            AudioManager.instance.play(8, false);
            //redCandy.enabled = true;
            buff.SetActive(true);
            itemIcon.spriteName = "snatchknife";
            //message = "You got the candy snatcher.";
        }
        else if (id == 3)//Glucose Pill
        {
            AudioManager.instance.play(9, false);
            //blueCandy.enabled = true;
            itemHeading.SetActive(true);
            itemIcon.spriteName = "glucosepill";
            //message = "You got the glucose pill.";
            _isExtraRoll = true;
        }
        else if (id == 4)//Witch Spell
        {
            AudioManager.instance.play(9, false);
            //blueCandy.enabled = true;
            debuff.SetActive(true);
            itemIcon.spriteName = "witchery";
            //message = "You got the witch spell.";
        }
        else if (id == 5)//Frozen Finger
        {
            AudioManager.instance.play(9, false);
            //blueCandy.enabled = true;
            debuff.SetActive(true);
            itemIcon.spriteName = "frozenhand";
            //message = "You got the frozen finger.";
        }
        else if (id == 6)//Broken Dice
        {
            AudioManager.instance.play(9, false);
            //blueCandy.enabled = true;
            debuff.SetActive(true);
            itemIcon.spriteName = "brokendice";
            //message = "You got the broken dice.";
        }

        rollButton.SetActive(false);
    }

    private void showCandyUI(int id)
    {
        AudioManager.instance.play(1, false);

        pickupNotification.SetActive(true);
        buff.SetActive(false);
        debuff.SetActive(false);
        itemHeading.SetActive(false);
        itemIcon.enabled = false;
        notificationOkButton.SetActive(false);
        notificationContinueButton.SetActive(false);
        notificationMessage.SetActive(false);

        itemIcon.enabled = true;
        notificationOkButton.SetActive(true);

        if (_currentPlayerIndex == 0)
        {
            notificationPotrait.spriteName = "archiface";
        }
        else if (_currentPlayerIndex == 1)
        {
            notificationPotrait.spriteName = "sashiface";
        }
        else if (_currentPlayerIndex == 2)
        {
            notificationPotrait.spriteName = "galiverface";
        }

        string message = string.Empty;

        if (id == 1)
        {
            itemHeading.SetActive(true);
            itemIcon.spriteName = "batwing";
        }
        else if (id == 2)
        {
            itemHeading.SetActive(true);
            itemIcon.spriteName = "newteye";
        }
        else if (id == 3)
        {
            itemHeading.SetActive(true);
            itemIcon.spriteName = "goldenpumpkin";
        }

        rollButton.SetActive(false);
    }

    private void updateRoundCounter()
    {
        _roundCount++;
        _shortCutCounter++;

        if(_shortCutCounter == 3 && currentLevel >= 2)
        {
            AudioManager.instance.play(5, false);

            foreach(TileInfo t in shortCutGates)
            {
                if (t.currentOrietation == 0)
                {
                    foreach(TileInfo ti in shortCutNodes)
                    {
                        ti.GetComponent<SpriteRenderer>().enabled = true;
                    }

                    foreach (GameObject g in closeGates)
                    {
                        g.SetActive(false);
                    }

                    foreach(GameObject g in openGates)
                    {
                        g.SetActive(true);
                    }

                    t.currentOrietation = 1;
                }
                    
                else
                {
                    t.currentOrietation = 0;
                    foreach(TileInfo ti in shortCutNodes)
                    {
                        ti.GetComponent<SpriteRenderer>().enabled = false;
                    }

                    foreach (GameObject g in closeGates)
                    {
                        g.SetActive(true);
                    }

                    foreach (GameObject g in openGates)
                    {
                        g.SetActive(false);
                    }
                }
                    
            }

            _shortCutCounter = 0;
        }

        roundCounterLabel.text = _roundCount.ToString();

        if(currentLevel >= 2)
        {
            if (_roundCount == 5)
            {
                AudioManager.instance.play(6, false);

                zombie.gameObject.SetActive(true);
                zombie.transform.position = tiles[0].transform.localPosition;
            }

            if (zombie.gameObject.activeInHierarchy)
            {
                rollZombieDice();
                rollButton.SetActive(false);
                zombieRollButton.SetActive(true);
                zombie.selectZombie();
            }
        }        
    }

    public void closeCandyCollectUI()
    {
        AudioManager.instance.play(0, false);

        //lastCandyWinnerGroup.SetActive(false);
        pickupNotification.SetActive(false);

        if (!_isWin)
        {
            if(!_isExtraRoll)
            {
                continueGame();
            }
            else
            {
                _isAllowRoll = true;
            }                

            rollButton.SetActive(true);
        }        
    }

    private void continueGame()
    {
        _currentPlayerIndex++;

        _isAllowRoll = true;

        if (_currentPlayerIndex >= players.Length)
        {
            _currentPlayerIndex = 0;
            updateRoundCounter();
        }

        if (_losePlayerIndex.Contains(_currentPlayerIndex))
        {
            _currentPlayerIndex++;
        }

        if (_currentPlayerIndex >= players.Length)
        {
            _currentPlayerIndex = 0;
        }                    

        //Highlight the next player
        highlightNextPlayer(_currentPlayerIndex);       
    }

    private IEnumerator generalDelay(float interval,Action callback)
    {
        yield return new WaitForSeconds(interval);
 
        if(callback != null)
            callback();
    }
}