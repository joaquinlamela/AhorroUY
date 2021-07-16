package com.ahorrouy.api.model.product

import com.google.gson.annotations.SerializedName

data class ProductResponse(
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

    var isFavorite: Boolean
)
