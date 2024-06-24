using System;
using UnityEngine;
using Assets.SimpleAndroidNotifications;


public static class NotificationScheduler
{
    public static void ScheduleReminderNotifications()
	{
        
        string[] messages0 = new string[]{
            "Hurry Up! Puzzles waiting for you!", "Come on! The hidden objects waiting for you!"
        };
        string[] messages1 = new string[]{
            "Knock Knock. Are you there?", "Ready to return for a puzzle"
        };
        string[] messages2 = new string[]{
            "Knock Knock. Are you there?", "Ready to return for a puzzle"
        };

#if UNITY_ANDROID

			for (int i = 0; i < 3; i++) {
				NotificationManager.Cancel(i);
			}

            // 24 Saatlik Notification
            ScheduleNotification(0, TimeSpan.FromHours(24), messages0[UnityEngine.Random.Range(0, messages0.Length)]);
            Debug.Log("24 saat bildirimi kuruldu.");

            // 48 Saatlik Notification
            ScheduleNotification(1, TimeSpan.FromHours(48), messages1[UnityEngine.Random.Range(0, messages1.Length)]);
            Debug.Log("48 saat bildirimi kuruldu.");

            // 96 Saatlik Notification
            ScheduleNotification(2, TimeSpan.FromHours(96), messages2[UnityEngine.Random.Range(0, messages2.Length)]);
            Debug.Log("96 saat bildirimi kuruldu.");

#endif
    }

    private static void ScheduleNotification(int id, TimeSpan time, string message){ // Android İçin
        #if UNITY_ANDROID && !UNITY_EDITOR
            var notificationParams = new NotificationParams
	        {
	            Id = id,
				Delay = time,
                Title = "Hidden Master",
				Message =  message,
	            Ticker = "Ticker",
	            Sound = true,
	            Vibrate = true,
	            Light = true,
	            SmallIcon = NotificationIcon.Heart,
	            SmallIconColor = new Color(0, 0.5f, 0),
	            LargeIcon = "app_icon"
	        };
            NotificationManager.SendCustom(notificationParams);

            Debug.Log(id.ToString()+ " Bildirim kuruldu: " + time.TotalHours.ToString());
        #endif
    }

    public static void CancelAllScheduledNotifications()
    {
#if UNITY_ANDROID 
        NotificationManager.CancelAll();
#endif
    }
}
