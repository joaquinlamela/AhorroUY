package com.ahorrouy.firebase

import android.content.SharedPreferences
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat
import com.ahorrouy.R
import com.google.firebase.messaging.FirebaseMessagingService
import com.google.firebase.messaging.RemoteMessage


class MessageService : FirebaseMessagingService() {

    override fun onNewToken(token: String) {
        println("ESTE ES EL NUEVO TOKEN, ACA ESTOY ${token}")
        saveNewTokenInPref(token)
    }

    override fun onMessageReceived(remoteMessage: RemoteMessage) {
        super.onMessageReceived(remoteMessage)
        if (remoteMessage.notification != null) {

            val title = remoteMessage.notification!!.title
            val body = remoteMessage.notification!!.body

            var builder = NotificationCompat.Builder(this, "channel1")
                .setSmallIcon(R.drawable.bomba)
                .setContentTitle(title)
                .setContentText(body)
                .setAutoCancel(true)
                .setPriority(NotificationCompat.PRIORITY_HIGH)

            val managerCompat: NotificationManagerCompat = NotificationManagerCompat.from(this)
            managerCompat.notify(1, builder.build())
        }
    }

    private fun saveNewTokenInPref(token: String) {
        val preferencesInstance: SharedPreferences = getSharedPreferences(
            "Preferences",
            MODE_PRIVATE
        )
        val editor: SharedPreferences.Editor = preferencesInstance.edit()
        editor.putString("registration_id", token)
        editor.apply()
    }
}