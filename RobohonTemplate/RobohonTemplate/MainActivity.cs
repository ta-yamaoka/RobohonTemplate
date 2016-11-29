using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using JP.CO.Sharp.Android.Voiceui;
using System.Collections.Generic;
using Android.Util;
using RobohonTemplate.Customize;
using RobohonTemplate.Util;
using JP.CO.Sharp.Android.RB.Projectormanager;
using static RobohonTemplate.Util.VoiceUIVariableUtil;

namespace RobohonTemplate
{
    [Activity(Label = "RobohonTemplate", MainLauncher = true, Icon = "@drawable/icon")]
    [IntentFilter(new[] { Intent.ActionMain },
        Categories = new[] { Intent.CategoryLauncher, "jp.co.sharp.android.rb.intent.category.LAUNCHER" })]
    [MetaData("use_projector", Value = "MainActivity")]
    public class MainActivity : Activity, MainActivityVoiceUIListener.MainActivityScenarioCallback
    {
        public static string TAG = nameof(MainActivity);

        protected static MainActivity Instance { get; set; }

        /**
        * 音声UI制御.
        */
        private VoiceUIManager mVoiceUIManager = null;
        /**
         * 音声UIイベントリスナー.
         */
        private MainActivityVoiceUIListener mMainActivityVoiceUIListener = null;
        /**
         * 音声UIの再起動イベント検知.
         */
        private VoiceUIStartReceiver mVoiceUIStartReceiver = null;
        /**
         * ホームボタンイベント検知.
         */
        private HomeEventReceiver mHomeEventReceiver;
        /**
         * プロジェクター状態変化イベント検知.
         */
        private ProjectorEventReceiver mProjectorEventReceiver;
        /**
         * プロジェクタ照射中のWakelock.
         */
        private Android.OS.PowerManager.WakeLock mWakelock;
        /**
         * 排他制御用.
         */
        private Object mLock = new Object();
        /**
         * プロジェクタ照射状態.
         */
        private bool isProjected = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Log.Verbose(TAG, "onCreate()");

            // HVMLが自動で登録されなかったので手動で登録する処理を呼び出す
            Intent i = new Intent("jp.co.sharp.android.voiceui.REQUEST_SCENARIO");
            var rec = new Common.RequestScenarioReceiver();
            rec.OnReceive(ApplicationContext, i);

            SetContentView(Resource.Layout.Main);

            // C#用の追加コード(インナークラスからアウタークラスへの参照用)
            Instance = this;

            //タイトルバー設定.
            SetupTitleBar();

            //ホームボタンの検知登録.
            mHomeEventReceiver = new HomeEventReceiver();
            IntentFilter filterHome = new IntentFilter(Intent.ActionCloseSystemDialogs);
            RegisterReceiver(mHomeEventReceiver, filterHome);

            //VoiceUI再起動の検知登録.
            mVoiceUIStartReceiver = new VoiceUIStartReceiver();
            IntentFilter filter = new IntentFilter(VoiceUIManager.ActionVoiceuiServiceStarted);
            RegisterReceiver(mVoiceUIStartReceiver, filter);

            //TODO プロジェクタイベントの検知登録(プロジェクター利用時のみ).
            SetProjectorEventReceiver();

            //発話ボタンの実装.
            Button Button = (Button)FindViewById(Resource.Id.accost);

            Button.Click += delegate
            {
                if (mVoiceUIManager != null)
                {
                    VoiceUIVariableListHelper helper = new VoiceUIVariableListHelper().AddAccost(ScenarioDefinitions.ACC_HELLO);
                    VoiceUIManagerUtil.UpdateAppInfo(mVoiceUIManager, helper.GetVariableList(), true);
                }

                // プロジェクターを動作させる場合コメント解除
                //StartService(GetIntentForProjector());
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
            Log.Verbose(TAG, "onResume()");

            //VoiceUIManagerのインスタンス取得.
            if (mVoiceUIManager == null)
            {
                mVoiceUIManager = VoiceUIManager.GetService(ApplicationContext);
            }
            //MainActivityVoiceUIListener生成.
            if (mMainActivityVoiceUIListener == null)
            {
                mMainActivityVoiceUIListener = new MainActivityVoiceUIListener(this);
            }
            //VoiceUIListenerの登録.
            VoiceUIManagerUtil.RegisterVoiceUIListener(mVoiceUIManager, mMainActivityVoiceUIListener);

            //Scene有効化.
            VoiceUIManagerUtil.EnableScene(mVoiceUIManager, ScenarioDefinitions.SCENE_COMMON);
            VoiceUIManagerUtil.EnableScene(mVoiceUIManager, ScenarioDefinitions.SCENE01);
        }

        protected override void OnPause()
        {
            base.OnPause();
            Log.Verbose(TAG, "onPause()");

            //バックに回ったら発話を中止する.
            VoiceUIManagerUtil.StopSpeech();

            //VoiceUIListenerの解除.
            VoiceUIManagerUtil.UnregisterVoiceUIListener(mVoiceUIManager, mMainActivityVoiceUIListener);

            //Scene無効化.
            VoiceUIManagerUtil.DisableScene(mVoiceUIManager, ScenarioDefinitions.SCENE_COMMON);
            VoiceUIManagerUtil.DisableScene(mVoiceUIManager, ScenarioDefinitions.SCENE01);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Verbose(TAG, "onDestroy()");

            //ホームボタンの検知破棄.
            this.UnregisterReceiver(mHomeEventReceiver);

            //VoiceUI再起動の検知破棄.
            this.UnregisterReceiver(mVoiceUIStartReceiver);

            //TODO プロジェクタイベントの検知破棄(プロジェクター利用時のみ).
            //this.unregisterReceiver(mProjectorEventReceiver);

            //インスタンスのごみ掃除.
            mVoiceUIManager = null;
            mMainActivityVoiceUIListener = null;
            mProjectorEventReceiver = null;
        }

        /**
        * VoiceUIListenerクラスからのコールバックを実装する.
        */
        public void OnExecCommand(string command, IList<VoiceUIVariable> variables)
        {
            Log.Verbose(TAG, "OnExecCommand() : " + command);
            switch (command)
            {
                case ScenarioDefinitions.FUNC_END_APP:
                    Finish();
                    break;
                case ScenarioDefinitions.FUNC_START_PROJECTOR:
                    //TODO プロジェクタマネージャの開始(プロジェクター利用時のみ).
                    //if(!isProjected) {
                    //    startService(getIntentForProjector());
                    //}
                    break;
                default:
                    break;
            }
        }

        /**
        * タイトルバーを設定する.
        */
        private void SetupTitleBar()
        {
            Toolbar toolbar = (Toolbar)FindViewById(Resource.Id.toolbar);
            SetActionBar(toolbar);
        }

        /**
        * プロジェクターマネージャーの開始/停止用のIntentを設定する.
        */
        private Intent GetIntentForProjector()
        {
            Intent intent = new Intent();
            ComponentName componentName = new ComponentName(
                    ProjectorManagerServiceUtil.PackageName,
                    ProjectorManagerServiceUtil.ClassName);
            //逆方向で照射する
            intent.PutExtra(ProjectorManagerServiceUtil.ExtraProjectorOutput, ProjectorManagerServiceUtil.ExtraProjectorOutputValReverse);
            intent.SetComponent(componentName);
            return intent;
        }

        /**
         * プロジェクターの状態変化イベントを受け取るためのレシーバーをセットする.
         */
        private void SetProjectorEventReceiver()
        {
            Log.Verbose(TAG, "setProjectorEventReceiver()");
            if (mProjectorEventReceiver == null)
            {
                mProjectorEventReceiver = new ProjectorEventReceiver();
            }
            else
            {
                return;
            }
            IntentFilter intentFilter = new IntentFilter();
            intentFilter.AddAction(ProjectorManagerServiceUtil.ActionProjectorPrepare);
            intentFilter.AddAction(ProjectorManagerServiceUtil.ActionProjectorStart);
            intentFilter.AddAction(ProjectorManagerServiceUtil.ActionProjectorPause);
            intentFilter.AddAction(ProjectorManagerServiceUtil.ActionProjectorResume);
            intentFilter.AddAction(ProjectorManagerServiceUtil.ActionProjectorEnd);
            intentFilter.AddAction(ProjectorManagerServiceUtil.ActionProjectorEndError);
            intentFilter.AddAction(ProjectorManagerServiceUtil.ActionProjectorEndFatalError);
            intentFilter.AddAction(ProjectorManagerServiceUtil.ActionProjectorTerminate);
            RegisterReceiver(mProjectorEventReceiver, intentFilter);
        }


        /**
         * WakeLockを取得する.
         */
        private void AcquireWakeLock()
        {
            Log.Verbose(TAG, "acquireWakeLock()");
            PowerManager pm = (PowerManager)ApplicationContext.GetSystemService(Context.PowerService);

            lock (mLock)
            {
                if (mWakelock == null || !mWakelock.IsHeld)
                {
                    mWakelock = pm.NewWakeLock(
                            WakeLockFlags.ScreenDim             //PowerManager.SCREEN_DIM_WAKE_LOCK
                            | WakeLockFlags.AcquireCausesWakeup //PowerManager.ACQUIRE_CAUSES_WAKEUP
                            | WakeLockFlags.OnAfterRelease      //PowerManager.ON_AFTER_RELEASE
                            , nameof(MainActivity));
                    mWakelock.Acquire();
                }
            }
        }

        /**
         * WakeLockを開放する.
         */
        private void ReleaseWakeLock()
        {
            Log.Verbose(TAG, "releaseWakeLock()");
            lock (mLock)
            {
                if (mWakelock != null && mWakelock.IsHeld)
                {
                    mWakelock.Release();
                    mWakelock = null;
                }
            }
        }

        /**
         * ホームボタンの押下イベントを受け取るためのBroadcastレシーバークラス.<br>
         * <p>
         * アプリは必ずホームボタンで終了する..
         */
        [BroadcastReceiver(Enabled = true)]
        private class HomeEventReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                Log.Verbose(TAG, "Receive Home button pressed");
                // ホームボタン押下でアプリ終了する.
                Instance.Finish();
            }
        }

        /**
         * 音声UI再起動イベントを受け取るためのBroadcastレシーバークラス.<br>
         * <p>
         * 稀に音声UIのServiceが再起動することがあり、その場合アプリはVoiceUIの再取得とListenerの再登録をする.
         */
        [BroadcastReceiver(Enabled = true)]
        private class VoiceUIStartReceiver : BroadcastReceiver
        {

            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;
                if (VoiceUIManager.ActionVoiceuiServiceStarted == action)
                {
                    Log.Debug(TAG, "VoiceUIStartReceiver#onReceive():VOICEUI_SERVICE_STARTED");
                    //VoiceUIManagerのインスタンス取得.
                    Instance.mVoiceUIManager = VoiceUIManager.GetService(Instance.ApplicationContext);
                    if (Instance.mMainActivityVoiceUIListener == null)
                    {
                        Instance.mMainActivityVoiceUIListener = new MainActivityVoiceUIListener(Instance.ApplicationContext);
                    }
                    //VoiceUIListenerの登録.
                    VoiceUIManagerUtil.RegisterVoiceUIListener(Instance.mVoiceUIManager, Instance.mMainActivityVoiceUIListener);
                }
            }
        }

        /**
         * プロジェクターの状態変化時のイベントを受け取るためのBroadcastレシーバークラス.<br>
         * <p>
         * 照射開始時にはWakeLockの取得、終了時にはWakeLockの開放する.<br>
         * アプリ仕様に応じて必要な処理があれば実装すること.
         */
        [BroadcastReceiver(Enabled = true)]
        private class ProjectorEventReceiver : BroadcastReceiver
        {

            public override void OnReceive(Context context, Intent intent)
            {
                Log.Verbose(TAG, "ProjectorEventReceiver#onReceive():" + intent.Action);
                switch (intent.Action)
                {
                    case ProjectorManagerServiceUtil.ActionProjectorPrepare:
                    case ProjectorManagerServiceUtil.ActionProjectorPause:
                    case ProjectorManagerServiceUtil.ActionProjectorResume:
                        break;
                    case ProjectorManagerServiceUtil.ActionProjectorStart:
                        Instance.AcquireWakeLock();
                        Instance.isProjected = true;
                        break;
                    case ProjectorManagerServiceUtil.ActionProjectorEnd:
                    case ProjectorManagerServiceUtil.ActionProjectorEndFatalError:
                    case ProjectorManagerServiceUtil.ActionProjectorEndError:
                    case ProjectorManagerServiceUtil.ActionProjectorTerminate:
                        Instance.ReleaseWakeLock();
                        Instance.isProjected = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

