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
using static JP.CO.Sharp.Android.Voiceui.VoiceUIVariable;
using RobohonTemplate.Customize;

namespace RobohonTemplate.Util
{
    public static class VoiceUIVariableUtil
    {
        /**
     * �w�肵��target���܂܂�邩���肷��.
     *
     * @param variableList variable���X�g
     * @param target       target������value�l
     * @return {@code true} : �܂�<br>
     * {@code false} : �܂܂Ȃ�
     */
        public static bool IsTarget(IList<VoiceUIVariable> variableList, string target)
        {
            bool result = false;
            for (int i = 0; i < variableList.Count; i++)
            {
                if (GetVariableData(variableList, ScenarioDefinitions.ATTR_TARGET) == target)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /**
         * �w�肵��target��function���܂܂�邩���肷��.
         *
         * @param variableList variable���X�g
         * @param target       target������value�l
         * @param function     function������value�l
         * @return {@code true} : �܂�<br>
         * {@code false} : �܂܂Ȃ�
         */
        public static bool IsTargetFuncution(IList<VoiceUIVariable> variableList, string target, string function)
        {
            bool result = false;
            for (int i = 0; i < variableList.Count; i++)
            {
                if (GetVariableData(variableList, ScenarioDefinitions.ATTR_TARGET) == target)
                {
                    if (GetVariableData(variableList, ScenarioDefinitions.ATTR_FUNCTION) == function)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        /**
         * �ϐ�name��String�f�[�^value���i�[���ĉ���UI�ɓo�^����.
         *
         * @param vm    VoiceUIManager�C���X�^���X
         * @param name  �f�[�^�̖��O
         * @param value �f�[�^
         * @return �֐��̎��s����
         */
        public static int SetVariableData(VoiceUIManager vm, string name, string value)
        {
            var list = new List<VoiceUIVariable>();
            VoiceUIVariable tmp;
            int result;
            tmp = new VoiceUIVariable(name, VariableType.String);//STRING�^��name�ϐ����Z�b�g
            tmp.Name = value; //tmp����name�ϐ��ɁA�lvalue���Z�b�g
            list.Add(tmp);
            result = VoiceUIManagerUtil.UpdateAppInfo(vm, list, false);
            return result;
        }

        /**
         * variable�̃��X�g����w�肵�����O��variable�Ɋi�[����Ă���String�f�[�^���擾����.
         *
         * @param variableList variable���X�g
         * @param name         �擾����variable�̖��O
         * @return �w�肵��variable�Ɋi�[����Ă���String�^��value�l.<br>
         * {@code name}�ƈ�v������̂��Ȃ��Ă�variable�󕶎���Ԃ�.<br>
         * {@code null}�͕Ԃ��Ȃ�.
         */
        public static string GetVariableData(IList<VoiceUIVariable> variableList, string name)
        {
            String result = "";
            int index = variableList.ToList().FindIndex(x => x.Name == name);
            //int index = getListIndex(variableList, name);
            if (index != -1)
            {
                result = variableList[index].StringValue;
            }
            return result;
        }

        /**
         * variable�̃��X�g����w�肵�����O��variable���i�[����Ă���index�l���擾����.
         *
         * @param variableList variable���X�g
         * @param name         �擾����variable�̖��O
         * @return {@code index} : �i�[����Ă���index�l<br>
         * {@code -1} : �w�肵�����O�����݂��Ȃ�
         */
        public static int GetListIndex(List<VoiceUIVariable> variableList, string name)
        {
            int index = -1;
            int tmp = 0;

            foreach (VoiceUIVariable variable in variableList)
            {
                if (variable.Name == name)
                {
                    index = tmp;
                    break;
                }
                tmp++;
            }

            return index;
        }

        public class VoiceUIVariableListHelper
        {
            List<VoiceUIVariable> mList = null;

            public VoiceUIVariableListHelper()
            {
                mList = new List<VoiceUIVariable>();
            }

            /**
             * �쐬�������X�g��Ԃ�
             *
             * @return VoiceUIVariable �̃��X�g
             */
            public IList<VoiceUIVariable> GetVariableList()
            {
                return mList;
            }

            /**
             * �g�p����V�i���I��accost���w�肷��.
             *
             * @param accost �w�肷��accost�^�u��value
             * @return VoiceUIVariableListHelper�I�u�W�F�N�g
             */
            public VoiceUIVariableListHelper AddAccost(string accost)
            {
                VoiceUIVariable variable = new VoiceUIVariable(ScenarioDefinitions.TAG_ACCOST, VariableType.String);
                variable.StringValue = accost;
                mList.Add(variable);
                return this;
            }

            /**
             * �g�p����V�i���I��scene���w�肷��.
             *
             * @param scene     �w�肷��scene�^�u��value
             * @param extraInfo �w�肷��scene�^�u��value
             * @return VoiceUIVariableListHelper�I�u�W�F�N�g
             */
            public VoiceUIVariableListHelper AddScene(string scene, string extraInfo)
            {
                VoiceUIVariable variable = new VoiceUIVariable(ScenarioDefinitions.TAG_SCENE, VariableType.String);
                variable.StringValue = scene;
                variable.ExtraInfo = extraInfo;
                mList.Add(variable);
                return this;
            }

            /**
             * String�^�̒l��ǉ�����.
             *
             * @param key   �ǉ�����l�̖��O
             * @param value �ǉ����镶����
             * @return VoiceUIVariableListHelper�I�u�W�F�N�g
             */
            public VoiceUIVariableListHelper AddStringValue(string key, string value)
            {
                VoiceUIVariable variable = new VoiceUIVariable(ScenarioDefinitions.TAG_MEMORY_PERMANENT + key, VariableType.String);
                variable.StringValue = value;
                mList.Add(variable);
                return this;
            }

            /**
             * float�^�̒l��ǉ�����.
             *
             * @param key   �ǉ�����l�̖��O
             * @param value �ǉ����鐔�l
             * @return VoiceUIVariableListHelper�I�u�W�F�N�g
             */
            public VoiceUIVariableListHelper AddFloatValue(string key, float value)
            {
                VoiceUIVariable variable = new VoiceUIVariable(ScenarioDefinitions.TAG_MEMORY_PERMANENT + key, VariableType.Float);
                variable.FloatValue = value;
                mList.Add(variable);
                return this;
            }

            /**
             * boolean�^�̒l��ǉ�����.
             *
             * @param key   �ǉ�����l�̖��O
             * @param value �ǉ�����^�U�l
             * @return VoiceUIVariableListHelper�I�u�W�F�N�g
             */
            public VoiceUIVariableListHelper AddBooleanValue(string key, bool value)
            {
                VoiceUIVariable variable = new VoiceUIVariable(ScenarioDefinitions.TAG_MEMORY_PERMANENT + key, VariableType.Boolean);
                variable.BooleanValue = value;
                mList.Add(variable);
                return this;
            }
        }
    }
}