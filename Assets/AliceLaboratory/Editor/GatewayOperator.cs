using UnityEngine;
using System.Collections.Generic;

namespace AliceLaboratory.Editor {
    public class GatewayOperator {

        private GatewayState _state = GatewayState.NONE;

        private Dream _dream;

        private List<string> _existsFiles = new List<string>();

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
                    _existsFiles = FilerOperator.getExistsTextures();
                    _dream = gateway.GetDreams();
                    if (_dream != null) {
                        _state = GatewayState.GETTING_DREAM_TEXTURE_INIT;
                    }
                    break;

                case GatewayState.GETTING_DREAM_TEXTURE_INIT:
                    _image = _dream.images[_counter];
                    // 既にローカルにテクスチャが存在する場合はスキップ
                    if (_existsFiles != null && _existsFiles.Contains(_image)) {
                        _state = GatewayState.GETTING_DREAM_TEXTURE_FINISHED;
                        break;
                    }
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