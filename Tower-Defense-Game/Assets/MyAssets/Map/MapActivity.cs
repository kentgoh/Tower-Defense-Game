using System.Collections;
using UnityEngine;
using static GlobalPredefinedModel;

public class MapActivity : MonoBehaviour
{
    public static MapActivity Instance;

    // Spell
    public GameObject spellIndicatorPrefab;
    private GameObject spellIndicator;

    void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            ShowSpellIndicator();
            UseSpell();
        }
    }

    private void ShowSpellIndicator()
    {
        Camera camera = GameActivity.Instance.camera;

        if (!GameActivity.Instance.ga_MouseState.IsOnUI()) { 
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, LayerMask.GetMask("Map")))
            {
                if (raycastHit.collider.CompareTag("Base"))
                {
                    GameActivity.Instance.ga_MouseState.UpdateMouseState(MouseState.Base_Map);
                    if (GameActivity.Instance.ga_Spell.selectedSpell != null)
                    {
                        Vector3 pointedBaseLocation = raycastHit.point;

                        pointedBaseLocation.y = 0.5f;
                        // The x and z value will have the value of 0.5 according to the position of the mouse pointed on the base
                        // Example:
                        //      0.2 => 0.5
                        //      -1.7 => -1.5
                        pointedBaseLocation.x = Mathf.Sign(pointedBaseLocation.x) * (Mathf.Floor(Mathf.Abs(pointedBaseLocation.x)) + 0.5f);
                        pointedBaseLocation.z = Mathf.Sign(pointedBaseLocation.z) * (Mathf.Floor(Mathf.Abs(pointedBaseLocation.z)) + 0.5f);

                        // Create spellIndicator if it does not exist yet
                        if (spellIndicator == null)
                            spellIndicator = Instantiate(spellIndicatorPrefab, pointedBaseLocation, Quaternion.identity);
                        // Only destroy old spellIndicator if the instantiate position need to be changed
                        else if (!pointedBaseLocation.Equals(spellIndicator.transform.position))
                        {
                            Destroy(spellIndicator);
                            spellIndicator = Instantiate(spellIndicatorPrefab, pointedBaseLocation, Quaternion.identity);
                        }

                    }
                    // No spell selected
                    else
                    {
                        if (spellIndicator != null)
                            Destroy(spellIndicator);
                    }

                }
                // Mouse not pointed to base, remove any spellIndicator if exist
                else { 
                    if (spellIndicator != null)
                        Destroy(spellIndicator);
                }
            }
        }
        // Ray blocked by UI, remove any spellIndicator if exist
        else{
            if (spellIndicator != null)
                Destroy(spellIndicator);
        }


    }
    private void UseSpell()
    {
        if (Input.GetMouseButtonDown(0) && !GameActivity.Instance.ga_MouseState.IsOnUI())
        {
            Camera camera = GameActivity.Instance.camera;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, LayerMask.GetMask("Map")))
            {
                Spell selectedSpell = GameActivity.Instance.ga_Spell.selectedSpell;

                if (raycastHit.collider.CompareTag("Base"))
                    if (selectedSpell != null)
                    {
                        ExecutingSpell(selectedSpell);
                        selectedSpell.currentCooldown = selectedSpell.maxCooldown;
                        Destroy(spellIndicator);
                        GameActivity.Instance.ResetSelectedSpell();
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.Standard, "UIError");
                        GameUIActivity.Instance.CoroutineDisplayDialog("Selected Spell not found.");
                    }
            }

        }

    }

    private void ExecutingSpell(Spell selectedSpell)
    {
        // Get spellIndicator position
        Vector3 spellPosition = spellIndicator.transform.position;
        spellPosition.y = 0;

        // Create the spell on the map
        GameObject spellGameObject =
            Instantiate(
                selectedSpell.prefab,
                spellPosition,
                Quaternion.identity
            );
        spellGameObject.name = selectedSpell.spellName.ToString();

        //if (selectedSpell.spellName == SpellName.Blizzard) {
        //    AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.IceSpell);
        //    yield return new WaitForSeconds(5.0f);
        //}
        //else if (selectedSpell.spellName == SpellName.LightningStrike) {
        //    AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.LightningSpell);
        //    yield return new WaitForSeconds(0.2f);
        //}

        //// Destroy spell after certain time
        //Destroy(spellGameObject);
    }
}
