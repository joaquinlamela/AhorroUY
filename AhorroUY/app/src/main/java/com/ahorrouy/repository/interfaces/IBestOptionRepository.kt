package com.ahorrouy.repository.interfaces

import com.ahorrouy.api.model.marketMap.BestOptionResponse
import com.ahorrouy.ui.marketMap.model.ProductSearchModel
import retrofit2.Response

interface IBestOptionRepository {
    suspend fun bestOptionsToBuy(
        token: String,
        productsToBuy: Array<ProductSearchModel>, longitude: Float,
        latitude: Float, maxDistance: Int
    ): Response<List<BestOptionResponse>>
}