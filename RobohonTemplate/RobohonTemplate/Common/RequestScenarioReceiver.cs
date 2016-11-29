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
using Android.Util;

namespace RobohonTemplate.Common
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(actions: new string[] { "jp.co.sharp.android.voiceui.REQUEST_SCENARIO" })]
    public class RequestScenarioReceiver : BroadcastReceiver
    {
        private const string TAG = nameof(RequestScenarioReceiver);

        /**
         * ÉVÉiÉäÉIìoò^Action.
         */
        private static string ACTION_REQUEST_SCENARIO = "jp.co.sharp.android.voiceui.REQUEST_SCENARIO";

        public override void OnReceive(Context context, Intent intent)
        {
            if (ACTION_REQUEST_SCENARIO == intent.Action)
            {
                //ÉVÉiÉäÉIìoò^óvãÅÇÃèÍçá
                Log.Debug(TAG, "onReceive-S:" + intent.Action);
                Intent baseIntent = new Intent();
                RegisterScenarioService.Start(context, baseIntent, RegisterScenarioService.CMD_REQUEST_SCENARIO);
                Log.Debug(TAG, "onReceive-E:" + intent.Action);
            }
            else
            {
                Log.Error(TAG, "onReceive Unknown action" + intent.Action);
            }
        }
    }
}