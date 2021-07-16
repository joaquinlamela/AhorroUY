package com.ahorrouy.api.model.purchase

import com.google.gson.annotations.SerializedName

data class PurchaseModelRequest(
    @SerializedName("amount")
    val Amount: Int,
    @SerializedName("marketName")
    val MarketName: String,
    @SerializedName("marketAddress")
    val MarketAddress: String
)