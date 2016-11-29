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
using Android.Content.Res;
using Java.IO;

namespace RobohonTemplate.Common
{
    [Service(Enabled = true, Exported = false)]
    public class RegisterScenarioService : Service
    {
        /**
        * サービスで実行するコマンド：シナリオの登録.
        */
        public const int CMD_REQUEST_SCENARIO = 10;
        private static string TAG = nameof(RegisterScenarioService);
        /**
         * サービスで実行可能なコマンドのキー名.
         */
        private const string NAME_KEY_COMMAND = "key_cmd";
        /**
         * home用シナリオフォルダー名.
         */
        private const string SCENARIO_FOLDER_HOME = "home";
        /**
         * other用シナリオフォルダー名.
         */
        private const string SCENARIO_FOLDER_OTHER = "other";

        private VoiceUIManager mVUIManager;

        public RegisterScenarioService()
        {
        }

        /**
         * サービスにコマンドを送信する.
         *
         * @param context    コンテキスト
         * @param baseIntent ベースとなるintent
         * @param command    コマンドの指定
         */
        public static void Start(Context context, Intent baseIntent, int command)
        {
            baseIntent.PutExtra(NAME_KEY_COMMAND, command);
            baseIntent.SetClass(context, typeof(RegisterScenarioService));
            context.StartService(baseIntent);
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException("Not yet implemented");
        }

        public override void OnCreate()
        {
            base.OnCreate();
            if (mVUIManager == null)
            {
                mVUIManager = VoiceUIManager.GetService(ApplicationContext);
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        //public override int OnStartCommand(Intent intent, int flags, int startId)
        {
            int cmd = intent.GetIntExtra(NAME_KEY_COMMAND, -1);
            if (cmd == -1)
            {
                Log.Error(TAG, "onStartCommand:not app key_command");
                return StartCommandResult.NotSticky;
            }

            Log.Debug(TAG, "onStartCommand cmd:" + cmd);
            switch (cmd)
            {
                //シナリオ登録コマンド受信.
                case CMD_REQUEST_SCENARIO:
                    //homeシナリオ登録.
                    RegisterScenario(true);
                    //home以外のシナリオ登録.
                    RegisterScenario(false);
                    StopSelf();
                    break;
                default:
                    break;
            }

            return StartCommandResult.NotSticky;
        }

        /**
         * シナリオの登録を行う.
         */
        private void RegisterScenario(Boolean home)
        {

            Log.Debug(TAG, "registerScenario-S:" + home.ToString());

            //基準フォルダー名取得.
            string baseFolderName = this.GetBaseFolderName(home);

            //Assetsフォルダーの基準フォルダーのファイル名リストを取得.
            AssetManager assetManager = Resources.Assets;
            string[] fileList = null;
            try
            {
                fileList = assetManager.List(baseFolderName);
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }

            //ローカルに引数.基準フォルダー作成.
            File localFolder = this.CreateBaseFolder(baseFolderName);

            //Assetsからローカルへhvmlファイルを全てコピー.            
            foreach (string fileName in fileList)
            {
                if (fileName.EndsWith(".hvml"))
                {
                    this.CopyHvmlFileFromAssetsToLocal(baseFolderName, localFolder.Path, fileName);
                }
            }

            //ローカルフォルダーの基準フォルダーのファイル名リストを取得.
            File[] files = localFolder.ListFiles();

            //ローカルフォルダーのhvmlファイルのシナリオを登録する.
            foreach (File file in files)
            {
                Log.Debug(TAG, "registerScenario file=" + file.AbsolutePath);
                int result = VoiceUIManager.VoiceuiError;
                try
                {
                    if (home == true)
                    {
                        //home用.
                        result = mVUIManager.RegisterHomeScenario(file.AbsolutePath);
                    }
                    else
                    {
                        //other.
                        result = mVUIManager.RegisterScenario(file.AbsolutePath);
                    }
                }
                catch (RemoteException e)
                {
                    e.PrintStackTrace();
                }
                if (result == VoiceUIManager.VoiceuiError)
                    Log.Error(TAG, "registerScenario:Error");
            }
            Log.Debug(TAG, "registerScenario-E:" + home.ToString());
        }

        /**
         * 基準フォルダー名取得.
         */
        private string GetBaseFolderName(Boolean home)
        {
            if (home == true)
            {
                //home用.
                return SCENARIO_FOLDER_HOME;
            }
            else
            {
                //other.
                return SCENARIO_FOLDER_OTHER;
            }
        }

        /**
         * 基準フォルダー作成.
         */
        private File CreateBaseFolder(string baseFolderName)
        {
            File folder = null;
            try
            {
                folder = new File(ApplicationContext.FilesDir, baseFolderName);
                if (!folder.Exists())
                {
                    folder.Mkdirs();
                }
                folder.SetReadable(true, false);
                folder.SetWritable(true, false);
                folder.SetExecutable(true, false);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            return folder;
        }

        /**
         * Assetsからローカルへhvmlファイルをコピー.
         */
        private void CopyHvmlFileFromAssetsToLocal(String baseFolderName, String localFolderName, String fileName)
        {
            File assetsFile = null;
            //InputStream inputStream = null;
            System.IO.Stream inputStream = null;
            File localFile = null;
            FileOutputStream fileOutputStream = null;
            byte[] buffer = null;
            try
            {
                // AssetsフォルダのhvmlファイルOpen
                assetsFile = new File(baseFolderName, fileName);
                inputStream = Resources.Assets.Open(assetsFile.Path);

                // ローカルフォルダーにhvmlファイル作成
                localFile = new File(localFolderName, fileName);
                if (localFile.Exists())
                {
                    localFile.Delete();
                }

                fileOutputStream = new FileOutputStream(localFile.Path);
                localFile.SetReadable(true, false);
                localFile.SetWritable(true, false);
                localFile.SetExecutable(true, false);
                buffer = new byte[1024];
                int length = 0;
                //while ((length = inputStream.Read(buffer)) >= 0)
                while ((length = inputStream.Read(buffer, 0, 1024)) > 0)
                {
                    fileOutputStream.Write(buffer, 0, length);
                }
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
            finally
            {
                if (fileOutputStream != null)
                {
                    try
                    {
                        fileOutputStream.Close();
                    }
                    catch (Exception e)
                    {
                    }
                }
                fileOutputStream = null;

                if (inputStream != null)
                {
                    try
                    {
                        inputStream.Close();
                    }
                    catch (Exception e)
                    {
                    }
                }
                inputStream = null;
                buffer = null;
                assetsFile = null;
                localFile = null;
                assetsFile = null;
            }
        }
    }
}