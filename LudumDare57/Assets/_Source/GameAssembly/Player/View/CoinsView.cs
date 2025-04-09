using TMPro;
using UnityEngine;
using VContainer;

namespace Player.View
{
    public class CoinsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;

        private PlayerStats _stats;
        
        [Inject]
        private void Construct(PlayerStats playerStats) => _stats = playerStats;

        private void Start()
        {
            Bind();
            Redraw();
        }

        private void OnDestroy() => Expose();

        private void Redraw() => coinText.text = _stats.Coins.ToString() + "$";

        private void Bind() => _stats.OnCoinsChanged += Redraw;

        private void Expose() => _stats.OnCoinsChanged -= Redraw;
    }
}