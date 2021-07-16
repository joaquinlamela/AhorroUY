package com.ahorrouy.api.model.signup

import com.google.gson.annotations.SerializedName

data class SignupRequest(
    @SerializedName("name")
    val Name: String,
    @SerializedName("username")
    val Username: String,
    @SerializedName("password")
    val Password: String
)