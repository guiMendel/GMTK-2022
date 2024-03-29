using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

abstract public class UniverseType : MonoBehaviour
{
  // === INTERFACE

  public List<GameObject> synchronizeObjects;

  public Color universeColor;


  // === REFS

  UniverseMapper UniverseMapper;
  TheDie theDie;
  RhythmicExecuter rhythmicExecuter;
  Tilemap tilemap;


  // === PROPERTIES

  public int DieValue => UniverseMapper.InverseType[this.GetType().ToString()];
  public bool IsActive => UniverseMapper.CurrentUniverse == this;


  // === OVERRIDABLES

  protected abstract void OnStart();
  protected abstract void OnAwake();
  protected virtual void BeatAction() { }
  protected virtual void DownbeatAction() { }
  protected virtual void OnActivate() { }


  private void Awake()
  {
    synchronizeObjects ??= new List<GameObject>();
    UniverseMapper = FindObjectOfType<UniverseMapper>();
    theDie = FindObjectOfType<TheDie>();
    rhythmicExecuter = GetComponent<RhythmicExecuter>();
    tilemap = FindObjectOfType<Tilemap>();

    EnsureNotNull.Objects(UniverseMapper, theDie, rhythmicExecuter, tilemap);

    OnAwake();
  }

  private void Start()
  {
    SetSynchronizedObjects(IsActive);

    theDie.OnDieRoll.AddListener(WatchDieRoll);

    rhythmicExecuter.OnEveryUpbeat.AddListener(() =>
    {
      if (IsActive) BeatAction();
    });

    rhythmicExecuter.OnEveryDownbeat.AddListener(() =>
    {
      if (IsActive) DownbeatAction();
    });

    OnStart();
  }

  private void OnDestroy()
  {
    theDie?.OnDieRoll?.RemoveListener(WatchDieRoll);
  }

  void WatchDieRoll(int newValue, int oldValue)
  {
    // Ignore redundancy
    if (newValue == oldValue) return;

    // Enable when activated
    if (newValue == DieValue)
    {
      SetSynchronizedObjects(true);
      OnActivate();

      OnOwnActivate();
    }

    // Disable when deactivated
    else if (oldValue == DieValue) SetSynchronizedObjects(false);
  }

  void SetSynchronizedObjects(bool enabled)
  {
    foreach (var synchronizeObject in synchronizeObjects)
    {
      synchronizeObject.SetActive(enabled);
    }
  }

  // Own OnActivate behavior
  void OnOwnActivate()
  {
    Camera.main.backgroundColor = universeColor;
    universeColor = new Vector4(universeColor.r, universeColor.g, universeColor.b, 1f);
    tilemap.color = universeColor;

    foreach (MovingPlatform platform in FindObjectsOfType<MovingPlatform>())
    {
      foreach (SpriteRenderer sprite in platform.GetComponentsInChildren<SpriteRenderer>())
      {
        sprite.color = universeColor;
      }
    }
  }
}
