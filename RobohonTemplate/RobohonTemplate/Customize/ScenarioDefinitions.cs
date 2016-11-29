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

namespace RobohonTemplate.Customize
{
    public static class ScenarioDefinitions
    {
        /**
        * sceneタグを指定する文字列
        */
        public const string TAG_SCENE = "scene";
        /**
         * accostタグを指定する文字列
         */
        public const string TAG_ACCOST = "accost";
        /**
         * target属性を指定する文字列
         */
        public const string ATTR_TARGET = "target";
        /**
         * function属性を指定する文字列
         */
        public const string ATTR_FUNCTION = "function";
        /**
         * memory_pを指定するタグ
         */
        public const string TAG_MEMORY_PERMANENT = "memory_p:";
        /**
         * function：アプリ終了を通知する.
         */
        public const string FUNC_END_APP = "end_app";
        /**
         * function：プロジェクタ起動を通知する.
         */
        public const string FUNC_START_PROJECTOR = "start_projector";
        /**
         * Package名.
         */
        private const string PACKAGE = "jp.co.bspr.robohontemplate";
        /**
         * シナリオ共通: controlタグで指定するターゲット名.
         */
        public const string TARGET = PACKAGE;
        /**
         * scene名: アプリ共通シーン
         */
        public const string SCENE_COMMON = PACKAGE + ".scene_common";
        /**
         * scene名: 特定シーン
         */
        public const string SCENE01 = PACKAGE + ".scene01";
        /**
         * accost名：こんにちは発話実行.
         */
        public const string ACC_HELLO = ScenarioDefinitions.PACKAGE + ".hello.say";
        /**
         * accost名：アプリ終了発話実行.
         */
        public const string ACC_END_APP = ScenarioDefinitions.PACKAGE + ".app_end.execute";
    }
}