package com.ahorrouy.ui.marketMap.model

data class BestOptionRequestModel(
    val token: String,
    val productsToBuy: Array<ProductSearchModel>,
    val longitude: Float,
    val latitude: Float,
    val maxDistance: Int
)