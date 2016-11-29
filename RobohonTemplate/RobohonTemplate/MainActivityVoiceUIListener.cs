using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using JP.CO.Sharp.Android.Voiceui;
using Java.Lang;
using Android.Util;
using RobohonTemplate.Util;
using RobohonTemplate.Customize;

namespace RobohonTemplate
{
    public class MainActivityVoiceUIListener : Java.Lang.Object, IVoiceUIListener
    {
        private const string TAG = nameof(MainActivityVoiceUIListener);

        private MainActivityScenarioCallback mCallback;

        /**
         * Activity側でのCallback実装チェック（実装してないと例外発生）.
        */
        public MainActivityVoiceUIListener(Context context) : base()
        {
            try
            {
                mCallback = (MainActivityScenarioCallback)context;
            }
            catch (ClassCastException e)
            {
                throw new ClassCastException(context.ToString() + " must implement " + TAG);
            }
        }

        public void OnVoiceUIActionCancelled(IList<VoiceUIVariable> variables)
        {
            //priorityが高いシナリオに割り込まれた場合の通知.
            Log.Verbose(TAG, "onVoiceUIActionCancelled");
            if (VoiceUIVariableUtil.IsTargetFuncution(variables, ScenarioDefinitions.TARGET,
                    ScenarioDefinitions.FUNC_END_APP))
            {
                mCallback.OnExecCommand(ScenarioDefinitions.FUNC_END_APP, variables);
            }
        }

        public void OnVoiceUIActionEnd(IList<VoiceUIVariable> variables)
        {
            //Actionの完了通知(シナリオ側にcontrolタグを書いたActionが完了すると呼び出される).
            //発話が終わった後でアプリ側の処理を実行したい場合はこちらを使う.
            Log.Verbose(TAG, "onVoiceUIActionEnd");
            if (VoiceUIVariableUtil.IsTarget(variables, ScenarioDefinitions.TARGET))
            {
                mCallback.OnExecCommand(VoiceUIVariableUtil.GetVariableData(variables, ScenarioDefinitions.ATTR_FUNCTION), variables);
            }
        }

        public void OnVoiceUIEvent(IList<VoiceUIVariable> variable)
        {
            //controlタグからの通知(シナリオ側にcontrolタグのあるActionが開始されると呼び出される).
            //発話と同時にアプリ側で処理を実行したい場合はこちらを使う.
            Log.Verbose(TAG, "onVoiceUIEvent");
        }

        public void OnVoiceUIRejection(VoiceUIVariable variable)
        {
            //priority負けなどで発話が棄却された場合のコールバック.
            Log.Verbose(TAG, "onVoiceUIRejection");
            if (ScenarioDefinitions.ACC_END_APP == variable.StringValue)
            {
                mCallback.OnExecCommand(ScenarioDefinitions.FUNC_END_APP, null);
            }
        }

        public void OnVoiceUIResolveVariable(IList<VoiceUIVariable> variable)
        {
            // アプリ側での変数解決用コールバック(シナリオ側にパッケージ名をつけた変数を書いておくと呼び出される).
            Log.Verbose(TAG, "onVoiceUIResolveVariable");
        }

        public void OnVoiceUISchedule(int i)
        {
            //処理不要(リマインダーアプリ以外は使われない).
        }

        /* Activityへの通知用IFクラス */
        public interface MainActivityScenarioCallback
        {
            /**
             * 実行されたcontrolの通知.
             *
             * @param function 実行された操作コマンド種別.
             */
            void OnExecCommand(string function, IList<VoiceUIVariable> variables);
        }
    }
}