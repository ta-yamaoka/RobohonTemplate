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
   * {@link VoiceUIManager#registerVoiceUIListener} �̃��b�p�[�֐�
   *
   * @param vm       VoiceUIManager�C���X�^���X
   * @param listener {@link VoiceUIListener}
   * @return �֐��̎��s����
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
         * {@link VoiceUIManager#unregisterVoiceUIListener} �̃��b�p�[�֐�
         *
         * @param vm       VoiceUIManager�C���X�^���X
         * @param listener {@link VoiceUIListener}
         * @return �֐��̎��s����
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
         * scene��L���ɂ���.
         * <br>
         * �w���scene��1�����L��������݂̂ł���A�����w������b�w������Ȃ�.
         *
         * @param vm    VoiceUIManager�C���X�^���X.
         *              {@code null}�̏ꍇ�� {@code VoiceUIManager.VOICEUI_ERROR} ��Ԃ�.
         * @param scene �L���ɂ���scene��.
         *              {@code null}��󕶎��̏ꍇ�� {@code VoiceUIManager.VOICEUI_ERROR} ��Ԃ�.
         * @return updateAppInfo�̎��s����
         */
        static public int EnableScene(VoiceUIManager vm, string scene)
        {
            int result = VoiceUIManager.VoiceuiError;
            // �����`�F�b�N.
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
         * scene�𖳌��ɂ���.
         * <br>
         * �w���scene��1���������ɂ���݂̂ł���A�����w������b�w������Ȃ�.
         *
         * @param vm    VoiceUIManager�C���X�^���X.
         *              {@code null}�̏ꍇ�� {@code VoiceUIManager.VOICEUI_ERROR} ��Ԃ�.
         * @param scene �L���ɂ���scene��.
         *              {@code null}��󕶎��̏ꍇ�� {@code VoiceUIManager.VOICEUI_ERROR} ��Ԃ�.
         * @return updateAppInfo�̎��s����
         */
        static public int DisableScene(VoiceUIManager vm, string scene)
        {
            int result = VoiceUIManager.VoiceuiError;
            // �����`�F�b�N.
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
         * {@link VoiceUIManager#updateAppInfo}�� {@link VoiceUIManager#updateAppInfoAndSpeech} �̃��b�p�[�֐�
         *
         * @param vm            VoiceUIManager�C���X�^���X
         * @param listVariables variable���X�g
         * @param speech        ���b���邩�ǂ���
         * @return �֐��̎��s����
         */
        static public int UpdateAppInfo(VoiceUIManager vm, IList<VoiceUIVariable> listVariables, bool speech)
        {
            int result = VoiceUIManager.VoiceuiError;
            // �����`�F�b�N.
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
         * {@link VoiceUIManager#stopSpeech} �̃��b�p�[�֐�.
         * <br>
         * RemoteException��throw������error���O���o�͂���.
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