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
using Android.Util;
using RobohonTemplate.Customize;

namespace RobohonTemplate.Util
{
    public static class VoiceUIManagerUtil
    {
        public static string TAG = nameof(VoiceUIManagerUtil);

        /**
   * {@link VoiceUIManager#registerVoiceUIListener} のラッパー関数
   *
   * @param vm       VoiceUIManagerインスタンス
   * @param listener {@link VoiceUIListener}
   * @return 関数の実行結果
   * @see VoiceUIManager#registerVoiceUIListener(VoiceUIListener)
   */
        static public int RegisterVoiceUIListener(VoiceUIManager vm, IVoiceUIListener listener)
        {
            int result = VoiceUIManager.VoiceuiError;
            if (vm != null)
            {
                try
                {
                    result = vm.RegisterVoiceUIListener(listener);
                }
                catch (RemoteException e)
                {
                    Log.Error(TAG, "Failed registerVoiceUIListener.[" + e.Message + "]");
                }
            }
            return result;
        }

        /**
         * {@link VoiceUIManager#unregisterVoiceUIListener} のラッパー関数
         *
         * @param vm       VoiceUIManagerインスタンス
         * @param listener {@link VoiceUIListener}
         * @return 関数の実行結果
         * @see VoiceUIManager#unregisterVoiceUIListener(VoiceUIListener)
         */
        public static int UnregisterVoiceUIListener(VoiceUIManager vm, IVoiceUIListener listener)
        {
            int result = VoiceUIManager.VoiceuiError;
            if (vm != null)
            {
                try
                {
                    result = vm.UnregisterVoiceUIListener(listener);
                }
                catch (RemoteException e)
                {
                    Log.Error(TAG, "Failed unregisterVoiceUIListener.[" + e.Message + "]");
                }
            }
            return result;
        }

        /**
         * sceneを有効にする.
         * <br>
         * 指定のsceneを1つだけ有効化するのみであり、複数指定も発話指定もしない.
         *
         * @param vm    VoiceUIManagerインスタンス.
         *              {@code null}の場合は {@code VoiceUIManager.VOICEUI_ERROR} を返す.
         * @param scene 有効にするscene名.
         *              {@code null}や空文字の場合は {@code VoiceUIManager.VOICEUI_ERROR} を返す.
         * @return updateAppInfoの実行結果
         */
        static public int EnableScene(VoiceUIManager vm, string scene)
        {
            int result = VoiceUIManager.VoiceuiError;
            // 引数チェック.
            if (vm == null || scene == null || "" == scene)
            {
                return result;
            }
            VoiceUIVariable variable = new VoiceUIVariable(ScenarioDefinitions.TAG_SCENE, VoiceUIVariable.VariableType.String);
            variable.StringValue = scene;
            variable.ExtraInfo = VoiceUIManager.SceneEnable;
            var listVariables = new List<VoiceUIVariable>();
            listVariables.Add(variable);
            try
            {
                result = vm.UpdateAppInfo(listVariables);
            }
            catch (RemoteException e)
            {
                Log.Error(TAG, "Failed updateAppInfo.[" + e.Message + "]");
            }
            return result;
        }

        /**
         * sceneを無効にする.
         * <br>
         * 指定のsceneを1つだけ無効にするのみであり、複数指定も発話指定もしない.
         *
         * @param vm    VoiceUIManagerインスタンス.
         *              {@code null}の場合は {@code VoiceUIManager.VOICEUI_ERROR} を返す.
         * @param scene 有効にするscene名.
         *              {@code null}や空文字の場合は {@code VoiceUIManager.VOICEUI_ERROR} を返す.
         * @return updateAppInfoの実行結果
         */
        static public int DisableScene(VoiceUIManager vm, string scene)
        {
            int result = VoiceUIManager.VoiceuiError;
            // 引数チェック.
            if (vm == null || scene == null || "" == scene)
            {
                return result;
            }
            VoiceUIVariable variable = new VoiceUIVariable(ScenarioDefinitions.TAG_SCENE, VoiceUIVariable.VariableType.String);
            variable.StringValue = scene;
            variable.ExtraInfo = VoiceUIManager.SceneDisable;
            var listVariables = new List<VoiceUIVariable>();
            listVariables.Add(variable);
            try
            {
                result = vm.UpdateAppInfo(listVariables);
            }
            catch (RemoteException e)
            {
                Log.Error(TAG, "Failed updateAppInfo.[" + e.Message + "]");
            }
            return result;
        }

        /**
         * {@link VoiceUIManager#updateAppInfo}と {@link VoiceUIManager#updateAppInfoAndSpeech} のラッパー関数
         *
         * @param vm            VoiceUIManagerインスタンス
         * @param listVariables variableリスト
         * @param speech        発話するかどうか
         * @return 関数の実行結果
         */
        static public int UpdateAppInfo(VoiceUIManager vm, IList<VoiceUIVariable> listVariables, bool speech)
        {
            int result = VoiceUIManager.VoiceuiError;
            // 引数チェック.
            if (vm == null || listVariables == null)
            {
                return result;
            }
            try
            {
                if (speech)
                {
                    result = vm.UpdateAppInfoAndSpeech(listVariables);
                }
                else
                {
                    result = vm.UpdateAppInfo(listVariables);
                }
            }
            catch (RemoteException e)
            {
                if (speech)
                {
                    Log.Error(TAG, "Failed updateAppInfoAndSpeech.[" + e.Message + "]");
                }
                else
                {
                    Log.Error(TAG, "Failed updateAppInfo.[" + e.Message + "]");
                }
            }
            return result;
        }

        /**
         * {@link VoiceUIManager#stopSpeech} のラッパー関数.
         * <br>
         * RemoteExceptionをthrowせずにerrorログを出力する.
         */
        static public void StopSpeech()
        {
            try
            {
                VoiceUIManager.StopSpeech();
            }
            catch (RemoteException e)
            {
                Log.Error(TAG, "Failed StopSpeech.[" + e.Message + "]");
            }
        }
    }
}