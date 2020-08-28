using UnityEngine;
using System.Collections.Generic;

namespace AliceLaboratory.Editor {

    /// <summary>
    /// 負の遺産です。OnUpdate()を使ったデータのダウンロードは毎フレームExecute()メソッド
    /// が呼ばれるため、今の状態を持っておく必要が生じる。
    /// その結果、状態遷移と今自分がどの状態にいるのかを毎フレーム確認することになる。
    /// これは、コードを読む際も同様に処理を追いかける必要が生じるため、コードの可読性が悪くなる。
    /// </summary>
    public class GatewayOperator {
        private Dream _dream;

        private List<string> _existsFiles = new List<string>();

        private string _image = "";

        private int _counter;

        public GatewayState State { set; get; } = GatewayState.NONE;

        public async void Execute(Gateway gateway) {
            // 通信実装を毎フレーム呼び出し
            switch (State) {
                case GatewayState.GETTING_DREAMS_LIST:
                    _existsFiles = FilerOperator.getExistsTextures();
                    _dream = await gateway.GetDreamsData();
                    if (_dream != null) {
                        State = GatewayState.GETTING_DREAM_TEXTURE_INIT;
                    }
                    break;

                case GatewayState.GETTING_DREAM_TEXTURE_INIT:
                    _image = _dream.images[_counter];
                    // 既にローカルにテクスチャが存在する場合はスキップ
                    if (_existsFiles != null && _existsFiles.Contains(_image)) {
                        State = GatewayState.GETTING_DREAM_TEXTURE_FINISHED;
                        break;
                    }
                    //gateway.SetUrlFromFileName(_image);
                    //State = gateway.GetDreamTexture(_image);
                    break;

                case GatewayState.GETTING_DREAM_TEXTURE:
                    //State = gateway.GetDreamTexture(_image);
                    break;

                case GatewayState.GETTING_DREAM_TEXTURE_FINISHED:
                    _counter++;
                    if (_counter < _dream.images.Length) {
                        State = GatewayState.GETTING_DREAM_TEXTURE_INIT;
                    } else {
                        State = GatewayState.GETTING_DREAM_TEXTURES_COMPLETED;
                    }
                    break;

                case GatewayState.GETTING_DREAM_TEXTURES_COMPLETED:
                    break;
            }
        }
    }
}