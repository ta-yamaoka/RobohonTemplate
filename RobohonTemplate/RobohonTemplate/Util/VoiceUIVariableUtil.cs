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
     * 指定したtargetが含まれるか判定する.
     *
     * @param variableList variableリスト
     * @param target       target属性のvalue値
     * @return {@code true} : 含む<br>
     * {@code false} : 含まない
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
         * 指定したtargetとfunctionが含まれるか判定する.
         *
         * @param variableList variableリスト
         * @param target       target属性のvalue値
         * @param function     function属性のvalue値
         * @return {@code true} : 含む<br>
         * {@code false} : 含まない
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
         * 変数nameにStringデータvalueを格納して音声UIに登録する.
         *
         * @param vm    VoiceUIManagerインスタンス
         * @param name  データの名前
         * @param value データ
         * @return 関数の実行結果
         */
        public static int SetVariableData(VoiceUIManager vm, string name, string value)
        {
            var list = new List<VoiceUIVariable>();
            VoiceUIVariable tmp;
            int result;
            tmp = new VoiceUIVariable(name, VariableType.String);//STRING型のname変数をセット
            tmp.Name = value; //tmp内のname変数に、値valueをセット
            list.Add(tmp);
            result = VoiceUIManagerUtil.UpdateAppInfo(vm, list, false);
            return result;
        }

        /**
         * variableのリストから指定した名前のvariableに格納されているStringデータを取得する.
         *
         * @param variableList variableリスト
         * @param name         取得するvariableの名前
         * @return 指定したvariableに格納されているString型のvalue値.<br>
         * {@code name}と一致するものがなくてもvariable空文字を返す.<br>
         * {@code null}は返さない.
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
         * variableのリストから指定した名前のvariableが格納されているindex値を取得する.
         *
         * @param variableList variableリスト
         * @param name         取得するvariableの名前
         * @return {@code index} : 格納されているindex値<br>
         * {@code -1} : 指定した名前が存在しない
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
             * 作成したリストを返す
             *
             * @return VoiceUIVariable のリスト
             */
            public IList<VoiceUIVariable> GetVariableList()
            {
                return mList;
            }

            /**
             * 使用するシナリオのaccostを指定する.
             *
             * @param accost 指定するaccostタブのvalue
             * @return VoiceUIVariableListHelperオブジェクト
             */
            public VoiceUIVariableListHelper AddAccost(string accost)
            {
                VoiceUIVariable variable = new VoiceUIVariable(ScenarioDefinitions.TAG_ACCOST, VariableType.String);
                variable.StringValue = accost;
                mList.Add(variable);
                return this;
            }

            /**
             * 使用するシナリオのsceneを指定する.
             *
             * @param scene     指定するsceneタブのvalue
             * @param extraInfo 指定するsceneタブのvalue
             * @return VoiceUIVariableListHelperオブジェクト
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
             * String型の値を追加する.
             *
             * @param key   追加する値の名前
             * @param value 追加する文字列
             * @return VoiceUIVariableListHelperオブジェクト
             */
            public VoiceUIVariableListHelper AddStringValue(string key, string value)
            {
                VoiceUIVariable variable = new VoiceUIVariable(ScenarioDefinitions.TAG_MEMORY_PERMANENT + key, VariableType.String);
                variable.StringValue = value;
                mList.Add(variable);
                return this;
            }

            /**
             * float型の値を追加する.
             *
             * @param key   追加する値の名前
             * @param value 追加する数値
             * @return VoiceUIVariableListHelperオブジェクト
             */
            public VoiceUIVariableListHelper AddFloatValue(string key, float value)
            {
                VoiceUIVariable variable = new VoiceUIVariable(ScenarioDefinitions.TAG_MEMORY_PERMANENT + key, VariableType.Float);
                variable.FloatValue = value;
                mList.Add(variable);
                return this;
            }

            /**
             * boolean型の値を追加する.
             *
             * @param key   追加する値の名前
             * @param value 追加する真偽値
             * @return VoiceUIVariableListHelperオブジェクト
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