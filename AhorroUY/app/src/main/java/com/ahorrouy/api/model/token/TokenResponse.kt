package com.ahorrouy.api.model.token

import android.os.Parcelable
import com.google.gson.annotations.SerializedName
import kotlinx.parcelize.Parcelize

@Parcelize
data class TokenResponse(
    @SerializedName("tokenValue")
    val tokenValue: String
) : Parcelable
