package com.ahorrouy.ui.marketMap.model

import android.os.Parcelable
import com.google.gson.annotations.SerializedName
import kotlinx.parcelize.Parcelize

@Parcelize
class ProductSearchModel(
    @SerializedName("productId")
    val productId: String,
    @SerializedName("quantity")

    val quantity: Int
) : Parcelable