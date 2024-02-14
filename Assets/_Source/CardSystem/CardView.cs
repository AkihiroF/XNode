using TMPro;
using UnityEngine;

namespace _Source.CardSystem
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textCard;


        public void UpdateCardText(string newInfo) => textCard.text = newInfo;
    }
}