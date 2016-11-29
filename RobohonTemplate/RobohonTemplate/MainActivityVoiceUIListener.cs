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
         * Activity���ł�Callback�����`�F�b�N�i�������ĂȂ��Ɨ�O�����j.
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
            //priority�������V�i���I�Ɋ��荞�܂ꂽ�ꍇ�̒ʒm.
            Log.Verbose(TAG, "onVoiceUIActionCancelled");
            if (VoiceUIVariableUtil.IsTargetFuncution(variables, ScenarioDefinitions.TARGET,
                    ScenarioDefinitions.FUNC_END_APP))
            {
                mCallback.OnExecCommand(ScenarioDefinitions.FUNC_END_APP, variables);
            }
        }

        public void OnVoiceUIActionEnd(IList<VoiceUIVariable> variables)
        {
            //Action�̊����ʒm(�V�i���I����control�^�O��������Action����������ƌĂяo�����).
            //���b���I�������ŃA�v�����̏��������s�������ꍇ�͂�������g��.
            Log.Verbose(TAG, "onVoiceUIActionEnd");
            if (VoiceUIVariableUtil.IsTarget(variables, ScenarioDefinitions.TARGET))
            {
                mCallback.OnExecCommand(VoiceUIVariableUtil.GetVariableData(variables, ScenarioDefinitions.ATTR_FUNCTION), variables);
            }
        }

        public void OnVoiceUIEvent(IList<VoiceUIVariable> variable)
        {
            //control�^�O����̒ʒm(�V�i���I����control�^�O�̂���Action���J�n�����ƌĂяo�����).
            //���b�Ɠ����ɃA�v�����ŏ��������s�������ꍇ�͂�������g��.
            Log.Verbose(TAG, "onVoiceUIEvent");
        }

        public void OnVoiceUIRejection(VoiceUIVariable variable)
        {
            //priority�����ȂǂŔ��b�����p���ꂽ�ꍇ�̃R�[���o�b�N.
            Log.Verbose(TAG, "onVoiceUIRejection");
            if (ScenarioDefinitions.ACC_END_APP == variable.StringValue)
            {
                mCallback.OnExecCommand(ScenarioDefinitions.FUNC_END_APP, null);
            }
        }

        public void OnVoiceUIResolveVariable(IList<VoiceUIVariable> variable)
        {
            // �A�v�����ł̕ϐ������p�R�[���o�b�N(�V�i���I���Ƀp�b�P�[�W���������ϐ��������Ă����ƌĂяo�����).
            Log.Verbose(TAG, "onVoiceUIResolveVariable");
        }

        public void OnVoiceUISchedule(int i)
        {
            //�����s�v(���}�C���_�[�A�v���ȊO�͎g���Ȃ�).
        }

        /* Activity�ւ̒ʒm�pIF�N���X */
        public interface MainActivityScenarioCallback
        {
            /**
             * ���s���ꂽcontrol�̒ʒm.
             *
             * @param function ���s���ꂽ����R�}���h���.
             */
            void OnExecCommand(string function, IList<VoiceUIVariable> variables);
        }
    }
}