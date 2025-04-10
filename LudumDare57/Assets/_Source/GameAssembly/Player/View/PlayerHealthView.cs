using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Player.View
{
    public class PlayerHealthView : MonoBehaviour
    {
        [SerializeField] private Gradient healthGradient;
        [SerializeField] private Image healthBarFill;
        [SerializeField] private TMP_Text healthText;
            
        private PlayerHealth _playerHealth;
        
        [Inject]
        private void Construct(PlayerHealth playerHealth) => _playerHealth = playerHealth;

        private void Start()
        {
            Bind();
            DrawHealth();
        }

        private void OnDestroy() => Expose();

        private void DrawHealth()
        {
            var healthProgress = (float)_playerHealth.GetHealth() / _playerHealth.GetMaxHealth();
            healthBarFill.color = healthGradient.Evaluate(healthProgress);
            healthText.color = healthGradient.Evaluate(healthProgress);
            healthBarFill.fillAmount = healthProgress;
            healthText.text = $"{_playerHealth.GetHealth()}/{_playerHealth.GetMaxHealth()}";
        }

        private void Bind() => _playerHealth.OnHealthChanged += DrawHealth;

        private void Expose() => _playerHealth.OnHealthChanged -= DrawHealth;
    }
}