package com.ahorrouy.api.model.coupon

import com.google.gson.annotations.SerializedName

data class CouponGetResponse(
    @SerializedName("url")
    val Url: String,
    @SerializedName("deadline")
    val Deadline: String,
    @SerializedName("value")
    val Value: String,
    @SerializedName("marketCouponLogoURL")
    val MarketCouponLogoURL: String
)