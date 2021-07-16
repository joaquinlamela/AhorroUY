package com.ahorrouy.api.model.login

import com.google.gson.annotations.SerializedName

data class LoginRequest(
    @SerializedName("username")
    val Username: String,
    @SerializedName("password")
    val Password: String
)