package com.ahorrouy.api.model.purchase

import com.google.gson.annotations.SerializedName

data class PurchaseModelResponse(
    @SerializedName("id")
    val Id: String,
    @SerializedName("amount")
    val Amount: Int,
    @SerializedName("marketName")
    val MarketName: String,
    @SerializedName("marketAddress")
    val MarketAddress: String,
    @SerializedName("purchaseDate")
    val PurchaseDate: String

)