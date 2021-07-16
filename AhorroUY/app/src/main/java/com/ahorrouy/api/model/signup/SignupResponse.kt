package com.ahorrouy.api.model.signup

import com.google.gson.annotations.SerializedName

data class SignupResponse(
    @SerializedName("id")
    val Id: String,
    @SerializedName("name")
    val Name: String,
    @SerializedName("username")
    val Username: String
)