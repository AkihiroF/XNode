using System;
using _Presentation.Nodes;
using TMPro;
using UnityEngine;
using XNode;

// Убедитесь, что импортировали пространство имен XNode

namespace _Source.CardSystem
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private NodeGraph cardGraph;
        [SerializeField] private CardView cardView;
        [SerializeField] private TextMeshProUGUI influenceText;
        private CardNode _currentCard;

        private InfluenceData _influenceData;

        private void Awake()
        {
            _influenceData = new();
        }

        void Start()
        {
            // Инициализация игры, например, установка начальной карточки
            SetInitialCard();
        }

        void SetInitialCard()
        {
            // Здесь должна быть логика для выбора начальной карточки,
            // например, выбор первой карточки из графа
            _currentCard = cardGraph.nodes.Find(node => node is CardNode) as CardNode;
            DisplayCurrentCard();
        }

        public void MakeChoice(bool choice)
        {
            // Предполагаем, что _currentCard.enter уже правильно установлен
            int influenceChange = choice ? _currentCard.influenceYes : _currentCard.influenceNo;
            _influenceData.ApplyInfluenceChange(influenceChange);

            NodePort exitPort = choice ? _currentCard.GetOutputPort("yesExit") : _currentCard.GetOutputPort("noExit");
            if (exitPort.Connection != null)
            {
                CardNode nextCard = exitPort.Connection.node as CardNode;
                if(nextCard != null)
                    nextCard.enter = _currentCard; // Устанавливаем текущую ноду как enter для следующей, перед тем как перейти
                if (choice)
                    _currentCard.yesExit = nextCard;
                else
                    _currentCard.noExit = nextCard;
                
                _currentCard = nextCard;
                DisplayCurrentCard();
                UpdateInfluenceView();
            }
            else
            {
                UnityEngine.Debug.Log("TheEnd");
            }
        }
        public void UndoLastChoice()
        {
            if (_currentCard.enter != null)
            {
                Debug.Log(_currentCard == _currentCard.enter.yesExit);
                // Восстановление влияния
                int influenceChange = _currentCard == _currentCard.enter.yesExit ? -_currentCard.enter.influenceYes : -_currentCard.enter.influenceNo;
                _influenceData.ApplyInfluenceChange(influenceChange);

                // Возврат к предыдущей ноде
                _currentCard = _currentCard.enter;
                DisplayCurrentCard();
                UpdateInfluenceView();
            }
            else
            {
                Debug.Log("No previous move to undo");
            }
        }

        private void UpdateInfluenceView()
        {
            influenceText.text = _influenceData.Influence.ToString();
        }

        void DisplayCurrentCard()
        {
            if (cardView != null)
            {
                cardView.UpdateCardText(_currentCard.cardText);
            }
            else
            {
                Debug.LogError("CardView is not set in CardManager");
            }
        }
    }
}