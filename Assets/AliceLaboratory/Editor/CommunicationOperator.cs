using UnityEngine;

namespace AliceLaboratory.Editor {
    public class CommunicationOperator {

        private CommunicationState _state = CommunicationState.NONE;

        public CommunicationState State {
            set { _state = value; }
            get { return _state; }
        }

        public void Execute(Communication com) {
            // 通信実装を毎フレーム呼び出し
            switch (_state) {
                case CommunicationState.GETTING_DREAMS_LIST:
                    Dream dream = com.GetDreams();
                    if (dream != null) {
                        Debug.Log(string.Join(" ", dream.images));
                        _state = CommunicationState.GETTING_DREAM_TEXTURES_FINISHED;
                    }
                    break;
                case CommunicationState.GETTING_DREAMS_LIST_FINISHED:
                    break;
                case CommunicationState.CONVERTING_JSON_TO_OBJECT:
                    break;
                case CommunicationState.CONVERTING_JSON_TO_OBJECT_FINISHED:
                    break;
                case CommunicationState.GETTING_DREAM_TEXTURES:
                    break;
                case CommunicationState.GETTING_DREAM_TEXTURES_FINISHED:
                    break;
            }
        }
    }
}