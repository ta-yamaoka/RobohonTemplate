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
        * scene�^�O���w�肷�镶����
        */
        public const string TAG_SCENE = "scene";
        /**
         * accost�^�O���w�肷�镶����
         */
        public const string TAG_ACCOST = "accost";
        /**
         * target�������w�肷�镶����
         */
        public const string ATTR_TARGET = "target";
        /**
         * function�������w�肷�镶����
         */
        public const string ATTR_FUNCTION = "function";
        /**
         * memory_p���w�肷��^�O
         */
        public const string TAG_MEMORY_PERMANENT = "memory_p:";
        /**
         * function�F�A�v���I����ʒm����.
         */
        public const string FUNC_END_APP = "end_app";
        /**
         * function�F�v���W�F�N�^�N����ʒm����.
         */
        public const string FUNC_START_PROJECTOR = "start_projector";
        /**
         * Package��.
         */
        private const string PACKAGE = "jp.co.bspr.robohontemplate";
        /**
         * �V�i���I����: control�^�O�Ŏw�肷��^�[�Q�b�g��.
         */
        public const string TARGET = PACKAGE;
        /**
         * scene��: �A�v�����ʃV�[��
         */
        public const string SCENE_COMMON = PACKAGE + ".scene_common";
        /**
         * scene��: ����V�[��
         */
        public const string SCENE01 = PACKAGE + ".scene01";
        /**
         * accost���F����ɂ��͔��b���s.
         */
        public const string ACC_HELLO = ScenarioDefinitions.PACKAGE + ".hello.say";
        /**
         * accost���F�A�v���I�����b���s.
         */
        public const string ACC_END_APP = ScenarioDefinitions.PACKAGE + ".app_end.execute";
    }
}