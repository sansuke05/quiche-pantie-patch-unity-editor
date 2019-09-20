using UnityEngine;

namespace AliceLaboratory.Editor {
    public class CommunicationOperator {

        private CommunicationState _state = CommunicationState.NONE;

        private Dream _dream;

        private string _image = "";

        private int _counter;

        public CommunicationState State {
            set { _state = value; }
            get { return _state; }
        }

        public void Execute(Communication com) {
            // 通信実装を毎フレーム呼び出し
            switch (_state) {
                case CommunicationState.GETTING_DREAMS_LIST:
                    _dream = com.GetDreams();
                    if (_dream != null) {
                        _state = CommunicationState.GETTING_DREAM_TEXTURE_INIT;
                    }
                    break;
                case CommunicationState.GETTING_DREAM_TEXTURE_INIT:
                    _image = _dream.images[_counter];
                    com.SetURLFromFileName(_image);
                    _state = com.GetTexture(_image);
                    break;
                case CommunicationState.GETTING_DREAM_TEXTURE:
                    _state = com.GetTexture(_image);
                    break;
                case CommunicationState.GETTING_DREAM_TEXTURE_FINISHED:
                    _counter++;
                    if (_counter < _dream.images.Length) {
                        _state = CommunicationState.GETTING_DREAM_TEXTURE_INIT;
                    } else {
                        _state = CommunicationState.GETTING_DREAM_TEXTURES_COMPLETED;
                    }
                    break;
                case CommunicationState.GETTING_DREAM_TEXTURES_COMPLETED:
                    break;
            }
        }
    }
}