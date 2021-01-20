using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCountWindow : MonoBehaviour
{
    private Text ammoDiplay;

    private void Awake()
    {
        ammoDiplay = transform.Find("AmmoDisplay").GetComponent<Text>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        AmmoDisplayHandler.Instance.OnAmmoChanged += OnAmmoCountChanged;
        ammoDiplay.text = "0/0";
    }

    private void OnDestroy()
    {
        AmmoDisplayHandler.Instance.OnAmmoChanged -= OnAmmoCountChanged;
    }

    private void OnAmmoCountChanged(object sender, AmmoDisplayHandler.OnAmmoDiplayChangeArgs e)
    {
        ammoDiplay.text = e.CurrentAmmo + "/" + e.MaxAmmo;
    }
}
