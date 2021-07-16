package com.ahorrouy.api.model.product

import com.google.gson.annotations.SerializedName

data class ProductDiscountResponse(
    @SerializedName("id")
    val id: String,
    @SerializedName("name")
    val name: String,
    @SerializedName("description")
    val description: String,
    @SerializedName("imageUrl")
    val imageUrl: String,
    @SerializedName("minPrice")
    val minPrice: Double,
    @SerializedName("maxPrice")
    val maxPrice: Double,
    @SerializedName("currentPrice")
    val currentPrice: Double,
    @SerializedName("regularPrice")
    val regularPrice: Double,
    @SerializedName("discountPercentage")
    val discountPercentage: Double,
    @SerializedName("marketName")
    val marketName: String,

    var isFavorite: Boolean
)