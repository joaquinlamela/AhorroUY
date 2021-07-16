package com.ahorrouy.api.model.login

import com.google.gson.annotations.SerializedName

data class LoginResponse(
    @SerializedName("token")
    val Token: String,
)