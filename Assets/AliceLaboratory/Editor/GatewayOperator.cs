using UnityEngine;

namespace AliceLaboratory.Editor {
    public class GatewayOperator {

        private GatewayState _state = GatewayState.NONE;

        private Dream _dream;

        private string _image = "";

        private int _counter;

        public GatewayState State {
            set { _state = value; }
            get { return _state; }
        }

        public void Execute(Gateway gateway) {
            // 通信実装を毎フレーム呼び出し
            switch (_state) {
                case GatewayState.GETTING_DREAMS_LIST:
                    _dream = gateway.GetDreams();
                    if (_dream != null) {
                        _state = GatewayState.GETTING_DREAM_TEXTURE_INIT;
                    }
                    break;
                case GatewayState.GETTING_DREAM_TEXTURE_INIT:
                    _image = _dream.images[_counter];
                    gateway.SetUrlFromFileName(_image);
                    _state = gateway.GetTexture(_image);
                    break;
                case GatewayState.GETTING_DREAM_TEXTURE:
                    _state = gateway.GetTexture(_image);
                    break;
                case GatewayState.GETTING_DREAM_TEXTURE_FINISHED:
                    _counter++;
                    if (_counter < _dream.images.Length) {
                        _state = GatewayState.GETTING_DREAM_TEXTURE_INIT;
                    } else {
                        _state = GatewayState.GETTING_DREAM_TEXTURES_COMPLETED;
                    }
                    break;
                case GatewayState.GETTING_DREAM_TEXTURES_COMPLETED:
                    break;
            }
        }
    }
}