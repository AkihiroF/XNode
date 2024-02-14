using _Source;
using XNode;

namespace _Presentation.Nodes
{
	public class CardNode : Node {
		[Input] public CardNode enter;
		[Output] public CardNode yesExit;
		[Output] public CardNode noExit;
		public string cardText;
		public int influenceYes; // Изменение "влияния" при выборе "Да"
		public int influenceNo;  // Изменение "влияния" при выборе "Нет" // Текст карточки

		public CardNode GetNextCard(CardNode currentCard, bool choice) {
			NodePort exitPort = choice ? currentCard.GetOutputPort("yesExit") : currentCard.GetOutputPort("noExit");
			if (exitPort.Connection != null) {
				return exitPort.Connection.node as CardNode;
			}
			return null; // или возвращайте начальную карточку, если это конец ветви
		}
	}
}