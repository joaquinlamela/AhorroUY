package com.ahorrouy.api.model.marketMap

import com.google.gson.annotations.SerializedName

data class BestOptionResponse(
    @SerializedName("marketName")
    val marketName: String,
    @SerializedName("marketAddress")
    val marketAddress: String,
    @SerializedName("marketLogo")
    val marketLogo: String,
    @SerializedName("priceForProducts")
    val priceForProducts: Double,
    @SerializedName("marketLongitude")
    val marketLongitude: Float,
    @SerializedName("marketLatitude")
    val marketLatitude: Float,
    @SerializedName("openTimeToday")
    val openTimeToday: String,
    @SerializedName("closeTimeToday")
    val closeTimeToday: String,
    @SerializedName("difference")
    val difference: Double
)
