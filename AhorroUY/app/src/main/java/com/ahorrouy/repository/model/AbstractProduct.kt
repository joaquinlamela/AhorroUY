package com.ahorrouy.repository.model

abstract class AbstractProduct(
    val id: String,
    val name: String,
    val description: String,
    val imageUrl: String,
    val minPrice: Double,
    val maxPrice: Double,
    var isFavorite: Boolean,
    var quantity: Int
)