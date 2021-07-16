package com.ahorrouy.api.model.logout

import com.google.gson.annotations.SerializedName

data class LogoutRequest(
    @SerializedName("token")
    val Token: String
)